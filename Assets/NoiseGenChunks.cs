using System;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenChunks : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 22f;
    private float delay = 0f; // how fast the update cycle is
    public Transform cam;
    public float offsetX;
    public float offsetY;
    public float RandOffsetX;
    public float RandOffsetY;
    public float div = 1;
    private FastNoiseLite noise = new FastNoiseLite();
    private Renderer renderer;
    [Range(-1.0f, 1.0f)]
    public float mult = -.3f;



    List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>> TwoDtiles;
    // Start is called before the first frame update
    void Start()
    {


        InitChunks();

        width = (int)TwoDtiles[0][0].obj.transform.localScale.x;
        height = (int)TwoDtiles[0][0].obj.transform.localScale.y;


        //renderer = GetComponent<Renderer>();    // change this to look in children

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime > delay)
        {


            

            delay = Time.fixedTime + 1;
            try
            {
                generate2D(TwoDtiles);
            }
            catch
            {
                initilizeTiles();
                generate2D(TwoDtiles);
            }

        }
    }

    void transformChunks()
    {





        //renderer.material.mainTexture = GenerateTexture();  // IF the chunk is moved run this & offset by pos

    }

    void InitChunks()   
    {
        var tempBackings = GetBackDat(0);
        tempBackings.obj.name = "1";
        for (int i = 2; i < 10; i++) // loop through to instantiate all 9 "tiles"
        {
            GameObject gameobj = Instantiate(tempBackings.obj);
            gameobj.transform.SetParent(tempBackings.obj.transform.parent);
            gameobj.name ="" + i;
        }


        initilizeTiles();
        
        //Tiles2D[0].Add(GetBackDat(0));

        generate2D(TwoDtiles);

    }

    void initilizeTiles()
    {

        noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        noise.SetFractalType(FastNoiseLite.FractalType.None);

        var Tiles = new List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>(); // this adds all the instantiated tiles to a var
        //for (int i = 0; i < GetBackDat(0).Backings.Count; i++)
        //    Tiles.Add(GetBackDat(i));
        var Tiles2D = new List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>>();
        int index = 0;

        for (int x = 1; x >= -1; x--)
        {
            for (int y = 1; y >= -1; y--)
            {
                Tiles.Add(GetBackDat(index));
                index++;
                
            }
            Tiles.Reverse();
            Tiles2D.Add(new List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>(Tiles)); // should end up with 3 rows of 3 tile tuples
            
            Tiles.Clear();
        }

        
        TwoDtiles = Tiles2D;
    }
    void generate2D(List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>> Tiles2D)
    {
        int index = 0;
        for (int x = 1; x >= -1; x--)
        {
            for (int y = 1; y >= -1; y--)
            {
                // multiply by the width 
                Tiles2D[x + 1][y + 1].obj.transform.position = new Vector3(x * Tiles2D[x + 1][y + 1].obj.transform.localScale.x, y * Tiles2D[x + 1][y + 1].obj.transform.localScale.y, 0);
                var offX = scale * x; //Tiles2D[x + 1][y + 1].obj.transform.position.x + 
                var offY = scale * y; //Tiles2D[x + 1][y + 1].obj.transform.position.y + 
                
                Tiles2D[x + 1][y + 1].rend.material.mainTexture = GenerateTexture(offX,offY);

                index++;

            }
        }
    }
    Texture2D GenerateTexture(float offX, float offY)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y, offX, offY);
                //color.b = 256;
                var div = 1;

                color *= mult; // brightness. lower is brighter ie -1 vs -.3
                
                color += new Color((float)(0.3671875 * div), (float)(0.609 * div), (float)(.609 * div)); // roughly the color that i want;


                texture.SetPixel(x, y, color);


                //texture.SetPixels(x, y, 5, 5, colors);
            }
        }
        texture.filterMode = FilterMode.Point;

        texture.Apply();
        texture.filterMode = FilterMode.Point;

        return texture;
    }
    Color CalculateColor(int x, int y, float offX, float offY)

    {

        float xCoord = ((float)(x) / width * scale + offX); // tweak these functions to give a overall nicer zoom and everything
        float yCoord = ((float)(y) / height * scale + offY);// tweak these functions to give a overall nicer zoom and everything

        float sample = noise.GetNoise(xCoord, yCoord);//noise.GetSimplex(xCoord, yCoord); //noise.GetPerlin(xCoord,yCoord);//Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);


    }
    (int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj) GetBackDat(int index)
    {
        GameObject obj;
        Renderer rend;

        List<GameObject> Backings;
        Backings = getObjWithTag(recursiveFindChildren(transform.gameObject), "BackGround");
        //print("index/ships: " + index + " " + (ships.Count - 1));

        //print("ship: " + ships[index].name);
        //GameObject k = ships[index];
        obj = Backings[index];                         // the index of 0 here is just to turn the list into obj
        rend = obj.GetComponent<Renderer>();

        Vector2 pos = new Vector2(obj.transform.position.x, obj.transform.position.y);

        // Initilise touple below and return.
        (int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj) d;
        d = (index, pos, rend, Backings, obj);

        return d; // tuple with EVERYTHING.
    }


    void MoveDir()
    {

        //left

        TwoDtiles.Insert(0, TwoDtiles[2]);  // Inserts "Eve" at the first position in the list
        TwoDtiles.RemoveAt(TwoDtiles.Count-1);  // Removes the first "Eve" element in the list

    }


    // return tuple of game object, renderer
    (GameObject obj, Renderer rend, List<GameObject> Backings) SetBack(int index)
    {
        GameObject obj;
        Renderer rend;

        List<GameObject> Backings;
        Backings = getObjWithTag(recursiveFindChildren(transform.gameObject), "BackGround");
        //print("index/ships: " + index + " " + (ships.Count - 1));

        //print("ship: " + ships[index].name);
        //GameObject k = ships[index];
        obj = Backings[index];                         // the index of 0 here is just to turn the list into obj
        rend = obj.GetComponent<Renderer>();
        // Initilise touple below and return.
        (GameObject obj, Renderer rend, List<GameObject> Backings) d;
        d = (obj, rend, Backings);

        return d; // tuple with EVERYTHING.
    }


    List<GameObject> getObjWithTag(List<GameObject> a, string tag)
    {
        List<GameObject> listOfChildren = new List<GameObject>();
        foreach (GameObject b in a)
        {
            if (b.tag == tag)
            {
                // print(b.name);
                listOfChildren.Add(b.gameObject);
            }
        }


        return listOfChildren;
    }
    List<GameObject> recursiveFindChildren(GameObject obj)    // obj is a game objet that gets searched for other game objects
    {
        List<GameObject> listOfChildren = new List<GameObject>();
        foreach (Transform g in obj.transform.GetComponentsInChildren<Transform>())
        {
            listOfChildren.Add(g.gameObject);
        }
        listOfChildren.Remove(obj);
        return listOfChildren;
    }







    //string printList(List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>> objlist) // print and return list of gameobjects
    //{

    //    objlist[1][1].index();


    //    string strVec = "";
    //    foreach (GameObject k in objlist)
    //        strVec += " " + k.name;
    //    print(strVec);
    //    return strVec;
    //}
















}
