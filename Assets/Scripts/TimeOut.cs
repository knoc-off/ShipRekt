using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TimeOut : MonoBehaviour
{

    public GameObject HitEffect;
    public GameObject thisBullet;
    private float time;
    public bool timeOut;
    public float TimeOutTime;
    public float DestroyTimeOut = 2;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.fixedTime + TimeOutTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeOut)
        {
            if (time < Time.fixedTime)
            {
                Destroy(thisBullet);
                GameObject effect = Instantiate(HitEffect, transform.position, new Quaternion());
                Destroy(effect, DestroyTimeOut);
            }
        }
        else
            if (thisBullet.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < .6)
            Destroy(thisBullet);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var TempShip = SetShip(0);
        GameObject effect = Instantiate(HitEffect, transform.position, transform.rotation);
        effect.transform.eulerAngles = new Vector3(0, 0, -effect.transform.eulerAngles.z);
        Destroy(effect, DestroyTimeOut);
        Destroy(gameObject);

    }

    (List<GameObject> ships, List<GameObject> sprites, GameObject currentShip, GameObject spriteLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) SetShip(int index)
    {
        GameObject shipLayer = GameObject.FindGameObjectWithTag("shipLayer");
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
    List<GameObject> recursiveFindChildren(GameObject obj)    // obj is a game objet that gets searched for 
    {
        List<GameObject> listOfChildren = new List<GameObject>();
        foreach (Transform g in obj.transform.GetComponentsInChildren<Transform>())
        {
            listOfChildren.Add(g.gameObject);
        }

        return listOfChildren;
    }


}
