using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateMap : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    public float scale = 20f;
    public Tilemap bg;          //ocean
    private Component[] Tiles;  // stores tileMaps

    public int NumberOfTiles = 16;
    public int interval = 3;
    public Transform cam;
    public Transform player;
    Dictionary<int, Tile> openWith = new Dictionary<int, Tile>();
    public bool enable;
    public bool animated;
    private int numx = 0;
    private int numy = 0;
    public float frameInt;
    private Vector2 movement;
    private float offsetX = 0f;
    private float offsetY = 0f;
    private int randomframetime;

    //    public Tile highlightTile;
    //    public Component Tilepallet;
    //    public TileData palette;

    // Start is called before the first frame update
    void Start()
    {
        instate();
        bg.ClearAllTiles();

        offsetX = Random.Range(-999, 999);
        offsetY = Random.Range(-999, 999);

        numx = Random.Range(-999, 999);
        numy = Random.Range(-999, 999);


        DrawMap();

        //ReDrawMap();
        randomframetime = (int)frameInt;

    }

    // Update is called once per frame

    void instate()
    {
        openWith.Clear();

        for (var i = 0; i < NumberOfTiles; i++)
        {
            openWith.Add(i, Resources.Load<Tile>("Tiles/_ater_" + i)); // add all tiles to the hash dict

        }
    }

    void LateUpdate()
    {


        if (enable && animated)
        {

            if ((Time.frameCount % randomframetime) == 0)
            {

                randomframetime = 1 + Random.Range(interval, (int)(interval * (frameInt * CalcVal(numx, numy))));
                numx++;
                numy++;
                bg.ClearAllTiles();
                numx += 1;
                numy += 1;
                offsetX += (float)(0.001); // (float)(cam.position.x);
                offsetY += (float)(0.001); // (float)(cam.position.y); Random.Range(-1, 1) * (float)(CalcVal(numx, numy) * 

                //offsetY = Mathf.Round((float)cam.position.y);
                //bg.transform.position = new Vector3(cam.position.x, cam.position.y, 0);

                try         // This works perfectly! If an error is spotted \/
                {
                    DrawMap();
                }
                catch       // it will re-define the dictionary so no errors can happen
                {
                    instate();
                }
            }
        }
        else if (Time.frameCount % 10 == 0 && enable) // make this only "fire" if i grab 4 corneres and one == null <= wip
        {
            try
            {
                DrawMap();
            }
            catch
            {
                instate();
            }

        }
    }
    void ReDrawMap()
    {


        float PerTileNum = ((float)1) / NumberOfTiles; // calculate the ammount of tiles that should fit between 0-1
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float k = 0;
                int num = 0;
                var val = CalcVal(x, y);
                while (val > k && num < NumberOfTiles - 1)
                {
                    k += PerTileNum;
                    num++;
                }
                bg.SetTile(new Vector3Int(x - (width / 2), y - (height / 2), 0), openWith[num]);
                //if (prevnum != num)
                //    Debug.Log(num);
            }

        }

    }
    void DrawMap()
    {
        float PerTileNum = ((float)1) / NumberOfTiles; // calculate the ammount of tiles that should fit between 0-1

        //Debug.Log(PerTileNum);


        for (int x = (int)cam.position.x; x < width + (int)cam.position.x; x++)
        {
            for (int y = (int)cam.position.y; y < height + (int)cam.position.y; y++)
            {
                //var a = new Vector3Int((int)cam.position.x, (int)cam.position.y, 0);
                if (bg.GetTile(new Vector3Int(x - (width / 2), y - (height / 2), 0)) == null)
                {

                    float k = 0;
                    int num = 0;
                    var val = CalcVal(x, y);
                    while (val > k && num < NumberOfTiles - 1)
                    {
                        k += PerTileNum;
                        num++;
                    }
                    if (num > NumberOfTiles - 1)
                        num = NumberOfTiles - 1;
                    if (num < 0)
                        num = 0;
                    bg.SetTile(bg.WorldToCell(new Vector3Int(x - (width / 2), y - (height / 2), 0)), openWith[num]);
                    //bg.SetTile(bg.WorldToCell(new Vector3Int((int)player.position.x,(int)player.position.y, 0)), openWith[num]);
                    //if (prevnum != num)
                    //    Debug.Log(num);
                    //prevnum = num;
                }
            }

        }

    }


    float CalcVal(int x, int y)
    {
        float xCoord = (((float)x) / width) * scale + offsetX;
        float yCoord = (((float)y) / height) * scale + offsetY;
        //Debug.Log(xCoord +"    "+yCoord);
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return sample;

    }

}
