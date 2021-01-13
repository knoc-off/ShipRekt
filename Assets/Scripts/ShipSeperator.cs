using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipSeperator : MonoBehaviour
{
    public bool enable;
    public GameObject shiplayers;
    public Camera cam;
    private Component[] Tiles;
    private Tilemap tempMap;
    public float mousex;
    public float mousey;
    private int lastframe;
    private Component[] TileMaps;
    private int maxnumber = 100;



    // Start is called before the first frame update
    void Start()
    {
        //Tiles = shiplayers.GetComponentsInChildren(typeof(Tilemap));
        TileMaps = GetComponentsInChildren(typeof(Tilemap));
        for (int i = 0; i < TileMaps.Length; i++) // loop through every Ship tilemap
        {
            tempMap = (Tilemap)TileMaps[i]; // might not have to set to temp? i cant type cast and set on same line though?
            tempMap.CompressBounds();       //compresses the bounds of the tilemap at program start. not useful unless pre-esablished ships are present.

        }

        Seperator();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pz = cam.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;
        mousex = pz.x;
        mousey = pz.y;

        if (Input.GetAxis("Fire1") == 1 && lastframe + 100 < Time.frameCount)
        {
            lastframe = Time.frameCount;

            if (enable)
            {
                //DetectSeparations(new int[,]{{ 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 } } );
                TileMaps = GetComponentsInChildren(typeof(Tilemap));
                for (int i = 0; i < TileMaps.Length; i++) // loop through every tilemap
                {
                    tempMap = (Tilemap)TileMaps[i];
                    if (tempMap.HasTile(tempMap.WorldToCell(new Vector3(pz.x, pz.y, 0))))
                    { // where click has tile
                        Seperator();
                    }
                }
            }
        }
    }




    void Seperator()
    {
        //DetectSeparations(new int[,]{{ 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 } } );
        TileMaps = GetComponentsInChildren(typeof(Tilemap));
        for (int i = 0; i < TileMaps.Length; i++) // loop through every tilemap
        {
            tempMap = (Tilemap)TileMaps[i];
            tempMap.CompressBounds();

            BoundsInt a = tempMap.cellBounds;

            //tempMap.SetTile(tempMap.WorldToCell(new Vector3(pz.x, pz.y, 0)), null);

            int[,] temparry = new int[a.size.y, a.size.x];      // temparry is instantiated it will digitize the tilemap int 1 = anything and 0 == null

            for (int y = 0; y < a.size.y; y++) // vert
            {
                for (int x = 0; x < a.size.x; x++) // horizontal
                {
                    if (tempMap.HasTile(tempMap.WorldToCell(tempMap.LocalToWorld(new Vector3(x + tempMap.origin.x, y + tempMap.origin.y, 0))))) // a.pos.x/y is NOT a good way of doing this. FIX THIS
                        temparry[y, x] = 1; //a.size.y - y - 1
                }
            }
            printArray(temparry);

            int _ = DetectSeparations(temparry);
            print(_);
            if (_ > 1)
                for (var t = 1; t <= _ && t < 100; t++)
                {
                    List<Vector2> offsets = new List<Vector2>();
                    offsets = getSeperation(temparry, t);

                    GameObject tempObj = Instantiate(tempMap.gameObject);
                    tempObj.transform.SetParent(tempMap.transform.parent);

                    Tilemap tempMap2 = (Tilemap)tempObj.GetComponent(typeof(Tilemap));
                    tempMap2.ClearAllTiles();


                    foreach (var z in offsets)
                    {
                        tempMap2.SetTile(tempMap.WorldToCell(tempMap.LocalToWorld(new Vector3(z.x + tempMap.origin.x, z.y + tempMap.origin.y, 0))), tempMap.GetTile(tempMap.WorldToCell(tempMap.LocalToWorld(new Vector3(z.x + tempMap.origin.x, z.y + tempMap.origin.y, 0)))));
                        tempMap.SetTile(tempMap.WorldToCell(tempMap.LocalToWorld(new Vector3(z.x + tempMap.origin.x, z.y + tempMap.origin.y, 0))), null);
                    }
                    tempMap.CompressBounds();
                    tempMap2.CompressBounds();

                }

            TileMaps = GetComponentsInChildren(typeof(Tilemap));
            print("size: " + a.size + " BoundsInt: " + a.position);

        }
    }

    int DetectSeparations(int[,] arry)// accept 2d array as a scene? the array will be based off of the tilemap | i need to create loop to add all non null as 1
    {
        List<Vector2> coord = new List<Vector2>();
        int num = 0;        //Num Stores the total Objects in the scene | instantiated here
        for (int y = 0; y < arry.GetLength(0); y++)
        {
            for (int x = 0; x < arry.GetLength(1); x++)
            {
                if (arry[y, x] == 1)
                    if (!coord.Contains(new Vector2(x, y)))
                    {
                        coord.AddRange(FillSearch(arry, x, y)); // add all to one 
                        num++;                                  //Num Stores the total Objects in the scene | incremented here
                                                                //print(printVecList(coord) + "\t" + num);
                    }
            }
        }
        return num;         // most should return AT LEAST one. if not, shit.
    }
    List<Vector2> getSeperation(int[,] arry, int seq)// accept 2d array as a scene? the array will be based off of the tilemap | i need to create loop to add all non null as 1
    {
        if (seq == 1)
            return new List<Vector2>();
        List<Vector2> coord = new List<Vector2>();
        int num = 0;        //Num Stores the total Objects in the scene | instantiated here
        for (int y = 0; y < arry.GetLength(0); y++)
        {
            for (int x = 0; x < arry.GetLength(1); x++)
            {
                if (arry[y, x] == 1)
                    if (!coord.Contains(new Vector2(x, y)))
                    {
                        coord.AddRange(FillSearch(arry, x, y)); // add all to one 
                        num++;                                  //Num Stores the total Objects in the scene | incremented here
                        if (num >= seq)
                            return FillSearch(arry, x, y);
                        //print(printVecList(coord) + "\t" + num);
                    }
            }
        }
        return new List<Vector2>();
    }

    // This function *Should* work perfectly now. removed the padding so it should not give offset vectors
    List<Vector2> FillSearch(int[,] arry, int strtX, int strtY) // pass in array of the tilemap where 1 == a tile 0 == null, Start X and Start Y is the location of where to start
                                                                // I should probably change all of the if _ == 1 to if _ != 0 so i can create a distinct tilemap.
    {
        if (arry[strtY, strtX] == 0)
            return new List<Vector2>();

        //printArray(arry);
        //printArray(PaddedArray);

        //Vector2[] VecArray = { new Vector2(1, 1), new Vector2(1, 1) }; // i should be able to add all of my x,y values to here to keep track of all found x,y
        List<Vector2> ToBeSearched = new List<Vector2>();
        List<Vector2> Searched = new List<Vector2>();
        ToBeSearched.Add(new Vector2(strtX, strtY));

        //print("Searched: " + Searched.Count + ", ToBeSearched: " + ToBeSearched.Count);
        //print(arry.GetLength(0)+""+arry.GetLength(1));
        maxnumber = 1000;
        while (ToBeSearched.Count > 0 && maxnumber > 0) // while the array ToBeSearched contains Something. ToBeSearched will rise exponentionally then fall expontially.
        {
            maxnumber--;
            //ToBeSearched.ForEach(delegate (Vector2 i)
            for (int K = 0; K < ToBeSearched.Count; K++)
            {
                Vector2 i = ToBeSearched[K];
                //Do A check to make sure that The to be searched is not out of bounds
                // this whole segment is to add surroundings
                // fix the out of bounds bug. idk probaly an easy fix i just cant be bothered rn

                if (i.x != arry.GetLength(1) - 1)
                    if (arry[(int)i.y, (int)i.x + 1] == 1)
                    {
                        if (!Searched.Contains(new Vector2(i.x + 1, i.y)) && !ToBeSearched.Contains(new Vector2(i.x + 1, i.y))) // check that its not already added
                            ToBeSearched.Add(new Vector2(i.x + 1, i.y));            // add the coord to the ToBeSearched list
                    }
                if (i.x != 0)
                    if (arry[(int)i.y, (int)i.x - 1] == 1)
                    {
                        if (!Searched.Contains(new Vector2(i.x - 1, i.y)) && !ToBeSearched.Contains(new Vector2(i.x - 1, i.y)))
                            ToBeSearched.Add(new Vector2(i.x - 1, i.y));
                    }
                if (i.y != arry.GetLength(0) - 1)
                    if (arry[(int)i.y + 1, (int)i.x] == 1)
                    {
                        if (!Searched.Contains(new Vector2(i.x, i.y + 1)) && !ToBeSearched.Contains(new Vector2(i.x, i.y + 1)))
                            ToBeSearched.Add(new Vector2(i.x, i.y + 1));
                    }
                if (i.y != 0)
                    if (arry[(int)i.y - 1, (int)i.x] == 1)
                    {
                        if (!Searched.Contains(new Vector2(i.x, i.y - 1)) && !ToBeSearched.Contains(new Vector2(i.x, i.y - 1)))
                            ToBeSearched.Add(new Vector2(i.x, i.y - 1));
                    }

                printVecList(Searched);
                Searched.Add(i);
                ToBeSearched.Remove(new Vector2(i.x, i.y));
                //print("Foreach end : Searched: " + Searched.Count + ", ToBeSearched: " + ToBeSearched.Count);

            }
            //print("while end : Searched: " + Searched.Count + ", ToBeSearched: " + ToBeSearched.Count);
        }

        //print("Searched: " + Searched.Count + ", ToBeSearched: " + ToBeSearched.Count);
        return Searched; // change this to return list of vectors that contians x,y | < done mostly with this | might profit from some sorting?
    }

    string printVecList(List<Vector2> VecList)
    {
        string strVec = "";
        for (int k = 0; k < VecList.Count; k++)
            strVec += VecList[k];
        return strVec;
    }

    void printArray(int[,] temparray)
    {
        for (int y = 0; y < temparray.GetLength(0); y++)
        {
            string row = "";
            for (int x = 0; x < temparray.GetLength(1); x++)
            {
                row += "\t" + temparray[y, x];
            }
            print(row);
        }
    }


}
