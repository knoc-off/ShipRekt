using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ListStuffer
{
    public int index;
    public Vector2 pos;
    public Renderer rend;
    public GameObject obj;
    public Color[] pixles;

    public ListStuffer()
    {
        index = -1;
        pos = new Vector2();
        rend = null;
        obj = null;
    }

    public ListStuffer(int i, Vector2 p, Renderer r, GameObject o)
    {
        index = i;
        pos = p;
        rend = r;
        obj = o;
    }
    public ListStuffer(int i, Vector2 p, Renderer r, GameObject o, int wid, int heig)
    {
        index = i;
        pos = p;
        rend = r;
        obj = o;
        pixles = new Color[wid * heig];
    }

}



public class MultiThreadChunkGen : MonoBehaviour
{
    public int width = 40;
    public int height = 40;
    public float scale = 200f;
    public float stepNumber = 0f;
    public float Brightness = 1.15f;
    public Thread TexThread;

    private FastNoiseLite noise = new FastNoiseLite();

    //public List<List<(Vector2 pos, Color[] pixles, int index)>> VectorList2D;
    //public List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>> TwoDtiles;


    //public List<List<BackgroundVariableTest>> TwoDimList = new List<List<BackgroundVariableTest>>();
    public List<List<ListStuffer>> TwoDimList = new List<List<ListStuffer>>();
    //public List<List<BackgroundVariableTest>> SmallLists = new List<List<BackgroundVariableTest>>();


    public void Left()
    {

    }
    public void Right()
    {

    }
    public void Up()
    {

    }
    public void Down()
    {

    }




    // Start is called before the first frame update
    void Start()
    {
        //List<List<LargeList>> LargeLists = new List<List<LargeList>>();

        List<List<ListStuffer>> tempTest = new List<List<ListStuffer>>();

        List<ListStuffer> test = new List<ListStuffer>();

        tempTest.Add(new List<ListStuffer>());
        tempTest[0][0].index = 2;

        print("aaa " + tempTest[0][0].index);

        //VectorList2D = new List<List<(Vector2 pos, Color[] pixles, int index)>>();
        //TwoDtiles = new List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>>();
        InitChunks();

        TexThread = new Thread(generate2D);


        //renderer = GetComponent<Renderer>();    // change this to look in children
        TexThread.Start();


    }

    // Update is called once per frame
    void Update()
    {


        //transformChunks(); // transforms chunks around the center of the 2d list
        if (TexThread.ThreadState == System.Threading.ThreadState.Stopped)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Texture2D tempa = new Texture2D(width, height);
                    tempa.SetPixels(TwoDimList[x][y].pixles);


                    tempa.filterMode = FilterMode.Point;
                    tempa.Apply();

                    TwoDimList[y][x].rend.material.mainTexture = tempa;

                    //SmallLists[x][y].index = 1;// = LargeLists[x][y].index;
                    //VectorList2D[x][y].index = TwoDtiles[x][y].index;
                }
            }






            TexThread = new Thread(generate2D);
            TexThread.IsBackground = true;
            TexThread.Start();


        }
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

        var Tiles = new List<ListStuffer>(); // this adds all the instantiated tiles to a var

        var Tiles2D = new List<List<ListStuffer>>();
        int index = 0;

        for (int x = 0; x > 3; x++)
        {
            for (int y = 0; y > 3; y++)
            {
                Tiles.Add(GetBackDat(index));
                index++;

            }
            Tiles.Reverse();
            Tiles2D.Add(new List<ListStuffer>(Tiles)); // should end up with 3 rows of 3 tile tuples

            Tiles.Clear();
        }

        TwoDimList = Tiles2D;
        print("cnt: " + Tiles2D.Count + "   cnt 2: " + Tiles2D[0].Count);

    }
    ListStuffer GetBackDat(int index)
    {

        GameObject obj;
        Renderer rend;

        List<GameObject> Backings = getObjWithTag(recursiveFindChildren(transform.gameObject), "BackGround");

        obj = Backings[index];
        rend = obj.GetComponent<Renderer>();

        Vector2 pos = new Vector2(obj.transform.position.x, obj.transform.position.y);

        return new ListStuffer(index, pos, rend, obj); // struct with EVERYTHING.
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




    void generate2D()//List<List<(int index, Vector2 pos, Renderer rend, List<GameObject> Backings, GameObject obj)>> Tiles2D
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                print("x: " + x + ", y: " + y);

                var offX = (TwoDimList[x + 1][y + 1].pos.x - (width / 2)) * (scale / width); //multiply offset by x/y to get proper offsets
                var offY = (TwoDimList[x + 1][y + 1].pos.y - (height / 2)) * (scale / height); //multiply offset by x/y to get proper offsets

                //TwoDtiles[x + 1][y + 1].rend.material.mainTexture = GenerateTexture(offX, offY);

                TwoDimList[x + 1][y + 1].pixles = GenerateTexture(offX, offY);

            }
        }
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


                color *= Brightness; // brightness. lower is brighter ie -1 vs -.3

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

    void transformChunks()
    {

        Vector2 CenterTilePos = TwoDimList[1][1].obj.transform.position;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                print("x: " + x + ", y: " + y);
                TwoDimList[x + 1][y + 1].obj.transform.position = new Vector3(CenterTilePos.x + (x * width), CenterTilePos.y + (y * height), 0);

            }
        }



        //renderer.material.mainTexture = GenerateTexture();  // IF the chunk is moved run this & offset by pos

    }

}
