using System.Collections.Generic;
using System.Threading;
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
    [Range(0.0f, 20.0f)]
    public float StepAmmount = 1;
    private FastNoiseLite noise = new FastNoiseLite();
    [Range(-1.0f, 1.0f)]
    public float mult = -.3f;
    [Range(0.001f, 2.0f)]
    public float cooldown;

    //[Range(-200.0f, 200.0f)]
    public float stepNumber = 0f;

    public int tempint;

    public float tempVal;

    public bool Animate = true;

    public Thread TexThread;

    public bool IsThreadRunning = false;



    //Thread Variables
    List<List<(Vector2 pos, Color[] pixles)>> VectorList2D;


    //Thread Variables

    List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>> TwoDtiles;


    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 70, 50, 30), "ThreadStart"))
    //        TexThread.Start();
    //}


    // Start is called before the first frame update
    void Start()
    {



        GetBackDat(0).rend.enabled = true;
        InitChunks();


        width = (int)TwoDtiles[0][0].obj.transform.localScale.x;
        height = (int)TwoDtiles[0][0].obj.transform.localScale.y;

        SetVariables();

        TexThread = new Thread(generate2D);


        //renderer = GetComponent<Renderer>();    // change this to look in children
        TexThread.Start();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if player is bottom left 
        if (Time.fixedTime > delay)
        {




            

            delay = Time.fixedTime + cooldown;




            stepNumber += StepAmmount;

            //print("thread state: "+ TexThread.ThreadState);

            if (TexThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 2; y++)
                    {

                        Texture2D tempa = new Texture2D(width, height);
                        tempa.SetPixels(VectorList2D[x][y].pixles);

                        tempa.filterMode = FilterMode.Point;
                        tempa.Apply();

                        TwoDtiles[y][x].rend.material.mainTexture = tempa;
                    }
                }

                if (cam.position.x < TwoDtiles[1][1].obj.transform.position.x - width / 2)
                {
                    print("left");
                    TwoDtiles.Insert(0, TwoDtiles[2]);
                    TwoDtiles.RemoveAt(3);

                }
                if (cam.position.x > TwoDtiles[1][1].obj.transform.position.x + width / 2)
                {
                    print("right");
                    TwoDtiles.Add(TwoDtiles[0]);
                    TwoDtiles.RemoveAt(0);
                }
                if (cam.position.y > TwoDtiles[1][1].obj.transform.position.y + height / 2)
                {
                    print("up");
                    foreach (var k in TwoDtiles)
                    {
                        k.Add(k[0]);
                        k.RemoveAt(0);
                    }


                }
                if (cam.position.y < TwoDtiles[1][1].obj.transform.position.y - width / 2)
                {
                    print("down");
                    foreach (var k in TwoDtiles)
                    {
                        k.Insert(0, k[2]);
                        k.RemoveAt(3);
                    }
                }
                transformChunks();

                SetVariables();
                TexThread = new Thread(generate2D);
                TexThread.IsBackground = true;
                TexThread.Start();
            }

            //if (TexThread.IsAlive)
            //{
            //    print("still running");
            //}
            //else
            //{
            //    print("Done");
            //    ;
            //}


                //    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                //try
                //{
                //    generate2D();
                //}
                //catch
                //{
                //    initilizeTiles();
                //    generate2D();
                //}
                //print("texture generation took : " + sw.ElapsedMilliseconds + " ms");
        }
    }
    void generate2D()//List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>> Tiles2D
    {
        IsThreadRunning = true;
        for (int x = 1; x >= -1; x--)
        {
            for (int y = 1; y >= -1; y--)
            {
                var offX = (VectorList2D[x + 1][y + 1].pos.x - (width / 2)) * (scale / width); //multiply offset by x/y to get proper offsets
                var offY = (VectorList2D[x + 1][y + 1].pos.y - (height / 2)) * (scale / height); //multiply offset by x/y to get proper offsets

                //TwoDtiles[x + 1][y + 1].rend.material.mainTexture = GenerateTexture(offX, offY);

                VectorList2D[x + 1][y + 1] = (new Vector2(VectorList2D[x + 1][y + 1].pos.x, VectorList2D[x + 1][y + 1].pos.y), GenerateTexture(offX, offY));

            }
        }
        IsThreadRunning = false;
        //TexThread.Abort();
    }
    Color[] GenerateTexture(float offX, float offY)
    {
        //Texture2D texture = new Texture2D(width, height);
        Color[] Colors = new Color[width * height];
        var index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y, offX, offY);
                //color.b = 256;


                color *= mult; // brightness. lower is brighter ie -1 vs -.3

                color += new Color(0.367f, 0.609f, 0.609f); // roughly the color that i want;

                Colors[index] = color;
                //texture.SetPixel(x, y, color);


                //texture.SetPixels(x, y, 5, 5, colors);
                index++;
            }
        }

        //texture.filterMode = FilterMode.Point;

        //texture.Apply();

        return Colors;
    }
    Color CalculateColor(int x, int y, float offX, float offY)

    {

        float xCoord = ((float)(x) / width * scale + offX); // tweak these functions to give a overall nicer zoom and everything
        float yCoord = ((float)(y) / height * scale + offY);// tweak these functions to give a overall nicer zoom and everything

        float sample = noise.GetNoise(xCoord, yCoord, stepNumber);//noise.GetSimplex(xCoord, yCoord); //noise.GetPerlin(xCoord,yCoord);//Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);


    }

    void SetVariables() // called outside of 2nd thread
    {


        noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        noise.SetFractalType(FastNoiseLite.FractalType.None);

        var Tiles = new List<(Vector2 pos, Color[] pixles)>(); // this adds all the instantiated tiles to a var
                                                                  //for (int i = 0; i < GetBackDat(0).Backings.Count; i++)
                                                                  //    Tiles.Add(GetBackDat(i));
        var Tiles2D = new List<List<(Vector2 pos, Color[] pixles)>>();
        int index = 0;

        for (int x = 1; x >= -1; x--)
        {
            for (int y = 1; y >= -1; y--)
            {
                Tiles.Add((new Vector2(TwoDtiles[x+1][y + 1].obj.transform.position.x, TwoDtiles[x + 1][y + 1].obj.transform.position.y), new Color[width*height])); // could do some optimisiation find size of colors
                index++;

            }
            Tiles.Reverse();
            Tiles2D.Add(new List<(Vector2 pos, Color[] pixles)>(Tiles)); // should end up with 3 rows of 3 tile tuples
            
            Tiles.Clear();
        }

        Tiles2D.Reverse();
        VectorList2D = Tiles2D;


    }

    void transformChunks()
    {

        Vector2 CenterTilePos = TwoDtiles[1][1].obj.transform.position;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                TwoDtiles[x + 1][y + 1].obj.transform.position = new Vector3(CenterTilePos.x + (x * width), CenterTilePos.y + (y * height), 0);

            }
        }



        //renderer.material.mainTexture = GenerateTexture();  // IF the chunk is moved run this & offset by pos

    }

    void InitChunks()
    {
        var tempBackings = GetBackDat(0);
        tempBackings.obj.name = "1";    // this is waht a prefab is for...
        for (int i = 2; i < 10; i++) // loop through to instantiate all 9 "tiles"
        {
            GameObject gameobj = Instantiate(tempBackings.obj);
            gameobj.transform.SetParent(tempBackings.obj.transform.parent);
            gameobj.name = "" + i;
        }


        initilizeTiles();

        //Tiles2D[0].Add(GetBackDat(0));
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
        TwoDtiles.RemoveAt(TwoDtiles.Count - 1);  // Removes the first "Eve" element in the list

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
