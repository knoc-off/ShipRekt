using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ShipTupleClass
{
    public GameObject shipLayer;   // could make it public and have it be able to be set to dynamicly change the shiplayer. idk what use that would be but it would be cool...
    public (List<GameObject> ships, List<GameObject> sprites, GameObject currentShip, GameObject spriteLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) SetShip(int index)
    {
        if (shipLayer == null)
            Debug.LogError("ShipLayer is null. set shiplayer to obj");

        List<GameObject> ships1 = new List<GameObject>();
        List<GameObject> sprites1 = new List<GameObject>();
        GameObject currentShip1;
        GameObject spriteLayer1;
        GameObject floor1;
        GameObject wall1;

        Tilemap floorMap1;
        Tilemap wallMap1;

        ships1 = getObjWithTag(recursiveFindChildren(shipLayer), "Ship");
        //print("index/ships: " + index + " " + (ships.Count - 1));

        //print("ship: " + ships[index].name);
        //GameObject k = ships[index];
        currentShip1 = ships1[index];                         // the index of 0 here is just to turn the list into obj
        floor1 = getObjWithTag(recursiveFindChildren(ships1[index]), "floor")[0];                   // the floor of ship "k"
        floorMap1 = floor1.GetComponent<Tilemap>();                                                // sets the tilemap floor
        wall1 = getObjWithTag(recursiveFindChildren(ships1[index]), "wall")[0];                   // the wall of ship "k"
        wallMap1 = wall1.GetComponent<Tilemap>();                                                // Sets the tilemap wall
        spriteLayer1 = getObjWithTag(recursiveFindChildren(ships1[index]), "sprites")[0];       // game object with sprites
        sprites1 = recursiveFindChildren(spriteLayer1);                                        // list of all sprites
        sprites1.Remove(spriteLayer1);                                                        // remove the self

        // Initilise touple below and return.
        (List<GameObject> ships, List<GameObject> sprites, GameObject currentShip, GameObject spriteLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) d;
        d = (ships1, sprites1, currentShip1, spriteLayer1, floor1, wall1, floorMap1, wallMap1);

        return d; // tuple with EVERYTHING.
    }
    public List<GameObject> getObjWithTag(List<GameObject> a, string tag)
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
    public List<GameObject> recursiveFindChildren(GameObject obj)    // obj is a game objet that gets searched for 
    {
        List<GameObject> listOfChildren = new List<GameObject>();
        foreach (Transform g in obj.transform.GetComponentsInChildren<Transform>())
        {
            listOfChildren.Add(g.gameObject);
        }

        return listOfChildren;
    }
}
