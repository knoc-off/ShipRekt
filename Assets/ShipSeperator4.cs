using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ShipSeperator4 : MonoBehaviour
{
    public GameObject ShipLayer;        // dont actuall need this but it will probably improve overall stability and future-proofing
    public GameObject ShipPrefab;


    // Start is called before the first frame update
    public void Start()
    {
        ShipLayer = GameObject.FindGameObjectWithTag("shipLayer");

        compressbounds();
    }

    void compressbounds()
    {
        var ogShip = SetShip(0);
        for (int i = 0; i < ogShip.ships.Count; i++) // loop through every tilemap
        {
            ogShip = SetShip(i);
            ogShip.floorMap.CompressBounds();
            ogShip.wallMap.CompressBounds();
        }
    }
    void OnGUI()
    {
        if (GUILayout.Button("seperator-inator-40000"))
        {
            Seperator();
            Debug.Log(Time.fixedTime.ToString());
        }
    }

    // MAKE SURE THE INDEX DOES NOT GET EXCEEDED. NO SAFTY NET!
    void Seperator()
    {
        var ogShip = SetShip(0);
        //DetectSeparations(new int[,]{{ 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 } } ); // humble beginnings...
        for (int i = 0; i < ogShip.ships.Count; i++) // loop through every tilemap
        {
            ogShip = SetShip(i);
            //floorMap = (Tilemap)TileMaps[i];

            BoundsInt a = ogShip.floorMap.cellBounds;  // floormap bounds width / height

            //tempMap.SetTile(tempMap.WorldToCell(new Vector3(pz.x, pz.y, 0)), null);

            int[,] temparry = new int[a.size.y, a.size.x];      // temparry is instantiated it will digitize the tilemap int 1 = anything and 0 == null

            for (int y = 0; y < a.size.y; y++) // vert
            {
                for (int x = 0; x < a.size.x; x++) // horizontal
                {
                    if (ogShip.floorMap.HasTile(ogShip.floorMap.WorldToCell(ogShip.floorMap.LocalToWorld(new Vector3(x + ogShip.floorMap.origin.x, y + ogShip.floorMap.origin.y, 0))))) // this somehow works...
                        temparry[y, x] = 1; // essentially this boils down to if theres a tile put it in this array at the correct spot.
                }
            }
            //printArray(temparry);       // temp out

            int _ = DetectSeparations(temparry);    // detect sep and put in temp _
            //print(_);
            if (_ > 1)  // if there are more seperations than just 1. 1 being no sep
                for (var t = 1; t <= _ && t < 100; t++) // 100 is an arbatrary limit could be surpassed then cause detection errors not likely though
                {
                    List<Vector2> offsets = new List<Vector2>();    // offsets is a list of vectors that will be used to add offsets to the final seperation
                    offsets = getSeperation(temparry, t);


                    ogShip = SetShip(i);


                    var ship = Instantiate(ShipPrefab);
                    ship.transform.parent = ShipLayer.transform;

                    ship.transform.eulerAngles = ogShip.currentShip.transform.eulerAngles;
                    ship.transform.position = ogShip.currentShip.transform.position;
                    var newShip = SetShip(ship);
                    newShip.floor.transform.position = ogShip.floor.transform.position;
                    newShip.floor.transform.eulerAngles = ogShip.floor.transform.eulerAngles;

                    newShip.floor.GetComponent<Rigidbody2D>().velocity = ogShip.floor.GetComponent<Rigidbody2D>().velocity;


                    // loop through sprites see if they are on a tile if not delete it.

                    //wallMap = ships[i].GetComponentsInChildren<Tilemap>()[1];

                    foreach (var z in offsets) // loop through all of the vectors setting a new ship etc
                    {
                        //floor
                        newShip.floorMap.SetTile(ogShip.floorMap.LocalToCell(new Vector3(z.x + ogShip.floorMap.origin.x, z.y + ogShip.floorMap.origin.y, 0)), ogShip.floorMap.GetTile(ogShip.floorMap.WorldToCell(ogShip.floorMap.LocalToWorld(new Vector3(z.x + ogShip.floorMap.origin.x, z.y + ogShip.floorMap.origin.y, 0)))));
                        ogShip.floorMap.SetTile(ogShip.floorMap.LocalToCell(new Vector3(z.x + ogShip.floorMap.origin.x, z.y + ogShip.floorMap.origin.y, 0)), null);
                        //wall
                        newShip.wallMap.SetTile(ogShip.floorMap.LocalToCell(new Vector3(z.x + ogShip.floorMap.origin.x, z.y + ogShip.floorMap.origin.y, 0)), ogShip.wallMap.GetTile(ogShip.floorMap.WorldToCell(ogShip.floorMap.LocalToWorld(new Vector3(z.x + ogShip.floorMap.origin.x, z.y + ogShip.floorMap.origin.y, 0)))));
                        ogShip.wallMap.SetTile(ogShip.floorMap.LocalToCell(new Vector3(z.x + ogShip.floorMap.origin.x, z.y + ogShip.floorMap.origin.y, 0)), null);
                    }
                }
            // change how this works to allow for thrusters to work more dynamic if possible
            for (int o = 0; o < SetShip(0).ships.Count; o++) // o itterates through every ship
            {
                foreach (GameObject thruster in ogShip.thrusters)
                {

                    //print(new Vector3(sprite.transform.position.x, sprite.transform.position.y, 0));
                    //print(new Vector3(sprite.transform.localPosition.x, sprite.transform.localPosition.y, 0));
                    if (SetShip(o).floorMap.HasTile(SetShip(o).floorMap.WorldToCell(new Vector3(thruster.transform.position.x, thruster.transform.position.y, 0))))
                    {
                        //print("tile under sprite " + sprite.name + " name " + sprite.transform.parent.transform.parent.transform.parent.name);
                        var taglayer = thruster.transform.parent.tag;
                        var parentlayer = getObjWithTag(recursiveFindChildren(SetShip(o).currentShip), taglayer);
                        thruster.transform.parent = parentlayer[0].transform;
                        //var temp = thruster.GetComponent<ThrustTest>();


                    }
                }

                foreach (GameObject sprite in ogShip.sprites)
                {

                    //print(new Vector3(sprite.transform.position.x, sprite.transform.position.y, 0));
                    //print(new Vector3(sprite.transform.localPosition.x, sprite.transform.localPosition.y, 0));
                    if (SetShip(o).floorMap.HasTile(SetShip(o).floorMap.WorldToCell(new Vector3(sprite.transform.position.x, sprite.transform.position.y, 0))))
                    {
                        //print("tile under sprite " + sprite.name + " name " + sprite.transform.parent.transform.parent.transform.parent.name);
                        var taglayer = sprite.transform.parent.tag;
                        var parentlayer = getObjWithTag(recursiveFindChildren(SetShip(o).currentShip), taglayer);
                        sprite.transform.parent = parentlayer[0].transform;

                    }
                }

            }

            //print("size: " + a.size + " BoundsInt: " + a.position);

        }
        compressbounds();

    }

    // ship cooker boiles down the ship to seperate bits need to pass those bits onto the shipCreator() methods

    void ShipCooker(int seperations, int index, int[,] temparry)    // this could use improvements
    {


    }

    // will not check compatibility will just do stuff like add floor under walls/sprites if needed
    void shipCreater(string ShipName, Tilemap FloorMap, Tilemap WallMap, List<GameObject> Sprites, List<GameObject> Thrusters)
    {

        var ship = Instantiate(ShipPrefab);
        ship.transform.parent = ShipLayer.transform;
        var currentShip = SetShip(ship);
        ship.name = ShipName;
        currentShip.floorMap = FloorMap;
        currentShip.wallMap = WallMap;


        foreach (var sprite in Sprites)
        {
            var _ = Instantiate(sprite);
            _.transform.SetParent(currentShip.floorMap.transform);
            _.tag = "sprite";
            _.layer = 10;
        }

        foreach (var thruster in Thrusters)
        {
            var _ = Instantiate(thruster);
            _.transform.SetParent(currentShip.ThrusterLayer.transform);
            _.tag = "sprite";
            _.layer = 10;
        }

    }

    void shipCreater(GameObject ship, Tilemap FloorMap, Tilemap WallMap, List<GameObject> Sprites, List<GameObject> Thrusters)
    {


        var currentShip = SetShip(ship);
        ship.name = "" + currentShip.ships.Count;


        foreach (var sprite in Sprites)
        {
            var _ = Instantiate(sprite);
            _.transform.SetParent(currentShip.floorMap.transform);
            _.tag = "sprite";
            _.layer = 10;
        }

        foreach (var thruster in Thrusters)
        {
            var _ = Instantiate(thruster);
            _.transform.SetParent(currentShip.ThrusterLayer.transform);
            _.tag = "sprite";
            _.layer = 10;
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
        var maxnumber = 1000;
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

                //printVecList(Searched);
                Searched.Add(i);
                ToBeSearched.Remove(new Vector2(i.x, i.y));
                //print("Foreach end : Searched: " + Searched.Count + ", ToBeSearched: " + ToBeSearched.Count);

            }
            //print("while end : Searched: " + Searched.Count + ", ToBeSearched: " + ToBeSearched.Count);
        }

        //print("Searched: " + Searched.Count + ", ToBeSearched: " + ToBeSearched.Count);
        return Searched; // change this to return list of vectors that contians x,y | < done mostly with this | might profit from some sorting?
    }

    (List<GameObject> ships, List<GameObject> sprites, List<GameObject> thrusters, GameObject currentShip, GameObject spriteLayer, GameObject ThrusterLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) SetShip(int index)
    {
        List<GameObject> thrusters = new List<GameObject>();
        List<GameObject> ships1 = new List<GameObject>();
        List<GameObject> sprites1 = new List<GameObject>();
        GameObject currentShip1;
        GameObject spriteLayer1;
        GameObject floor1;
        GameObject wall1;
        GameObject ThrusterLayer;
        Tilemap floorMap1;
        Tilemap wallMap1;

        ships1 = getObjWithTag(recursiveFindChildren(ShipLayer), "Ship");
        //print("index/ships: " + index + " / " + (ships1.Count));

        // Initilise tuple below and return.
        (List<GameObject> ships, List<GameObject> sprites, List<GameObject> thrusters, GameObject currentShip, GameObject spriteLayer, GameObject ThrusterLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) d;

        //print("ship: " + ships[index].name);
        //GameObject k = ships[index];
        if (index >= 0 || index < ships1.Count)           // indexing might be off
        {
            currentShip1 = ships1[index];                         // the index of 0 here is just to turn the list into obj

            floor1 = getObjWithTag(recursiveFindChildren(ships1[index]), "floor")[0];
            wall1 = getObjWithTag(recursiveFindChildren(ships1[index]), "wall")[0];
            spriteLayer1 = getObjWithTag(recursiveFindChildren(ships1[index]), "sprites")[0];
            ThrusterLayer = getObjWithTag(recursiveFindChildren(ships1[index]), "ThrusterLayer")[0];
            floorMap1 = floor1.GetComponent<Tilemap>();
            wallMap1 = wall1.GetComponent<Tilemap>();
            sprites1 = recursiveFindChildren(spriteLayer1);
            thrusters = recursiveFindChildren(ThrusterLayer);
            sprites1.Remove(spriteLayer1);
            d = (ships1, sprites1, thrusters, currentShip1, spriteLayer1, ThrusterLayer, floor1, wall1, floorMap1, wallMap1);

        }
        else
        {
            Debug.LogError("OUT OF INDEX");
            d = (null, null, null, null, null, null, null, null, null, null);
        }


        return d; // tuple with EVERYTHING.


    }
    (List<GameObject> ships, List<GameObject> sprites, List<GameObject> thrusters, GameObject currentShip, GameObject spriteLayer, GameObject ThrusterLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) SetShip(GameObject Ship)
    {
        List<GameObject> thrusters = new List<GameObject>();
        List<GameObject> ships1 = new List<GameObject>();
        List<GameObject> sprites1 = new List<GameObject>();
        GameObject spriteLayer1;
        GameObject floor1;
        GameObject wall1;
        GameObject ThrusterLayer;
        Tilemap floorMap1;
        Tilemap wallMap1;

        ships1 = getObjWithTag(recursiveFindChildren(ShipLayer), "Ship");
        //print("index/ships: " + index + " " + (ships.Count - 1));

        //print("ship: " + ships[index].name);
        //GameObject k = ships[index];


        floor1 = getObjWithTag(recursiveFindChildren(Ship), "floor")[0];                            // the floor of ship "k"
        floorMap1 = floor1.GetComponent<Tilemap>();                                                // sets the tilemap floor
        wall1 = getObjWithTag(recursiveFindChildren(Ship), "wall")[0];                            // the wall of ship "k"
        wallMap1 = wall1.GetComponent<Tilemap>();                                                // Sets the tilemap wall
        spriteLayer1 = getObjWithTag(recursiveFindChildren(Ship), "sprites")[0];                // game object with sprites
        sprites1 = recursiveFindChildren(spriteLayer1);                                        // list of all sprites
        sprites1.Remove(spriteLayer1);                                                        // remove the self
        ThrusterLayer = getObjWithTag(recursiveFindChildren(Ship), "ThrusterLayer")[0];
        thrusters = recursiveFindChildren(ThrusterLayer);

        // Initilise touple below and return.
        (List<GameObject> ships, List<GameObject> sprites, List<GameObject> thrusters, GameObject currentShip, GameObject spriteLayer, GameObject ThrusterLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) d;
        d = (ships1, sprites1, thrusters, Ship, spriteLayer1, ThrusterLayer, floor1, wall1, floorMap1, wallMap1);

        return d; // tuple with EVERYTHING.


    }

    List<GameObject> getObjWithTag(List<GameObject> ListToBeSearched, string tag)
    {
        List<GameObject> listOfChildren = new List<GameObject>();
        foreach (GameObject b in ListToBeSearched)
        {
            if (b.tag == tag)
            {
                // print(b.name);
                listOfChildren.Add(b.gameObject);
            }
        }


        return listOfChildren;
    }


    List<GameObject> recursiveFindChildren(GameObject obj)    // obj is a game objet that gets searched for 
    {
        List<GameObject> listOfChildren = new List<GameObject>();
        foreach (Transform g in obj.transform.GetComponentsInChildren<Transform>())
        {
            listOfChildren.Add(g.gameObject);
        }

        return listOfChildren;
    }

    string printList(List<GameObject> objlist) // print and return list of gameobjects
    {
        string strVec = "";
        foreach (GameObject k in objlist)
            strVec += " " + k.name;
        print(strVec);
        return strVec;
    }
    string printTransList(List<Transform> VecList)
    {
        string strVec = "";
        foreach (Transform k in VecList)
            strVec += " " + k.name;
        print(strVec);
        return strVec;
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
