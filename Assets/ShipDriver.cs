using System.Collections.Generic;
using UnityEngine;

public class ShipDriver : MonoBehaviour
{
    private ShipTupleClass ship = new ShipTupleClass();
    public GameObject thrusterLayer;
    [Range(-20, 20)] public List<float> thrusterForce = new List<float>();
    public List<(List<GameObject> thrusters, GameObject currentThruster, ParticleSystem particle, Rigidbody2D ShipFloorRB)> Thrusters;

    public void Start()
    {
        Thrusters = new List<(List<GameObject> thrusters, GameObject currentThruster, ParticleSystem particle, Rigidbody2D ShipFloorRB)>();
        ship.shipLayer = GameObject.FindGameObjectWithTag("shipLayer");

        InitThrusters();
    }


    public void Update()
    {

        // Need to change how the thrusters are rechognized  

        for (int i = 0; i < setThruster(0).thrusters.Count; i++)
        {
            setThruster(i).currentThruster.GetComponent<ThrustTest>().force = thrusterForce[i];
        }
        InitThrusters();
    }

    void InitThrusters()
    {
        thrusterForce.Clear();
        Thrusters.Clear();
        for (int i = 0; i < setThruster(0).thrusters.Count; i++)
        {
            thrusterForce.Add(setThruster(i).currentThruster.GetComponent<ThrustTest>().force);
            Thrusters.Add(setThruster(i));
        }
    }


    public (List<GameObject> thrusters, GameObject currentThruster, ParticleSystem particle, Rigidbody2D ShipFloorRB) setThruster(int index)
    {
        if (thrusterLayer == null)
            Debug.LogError("thruster layer is null. set to obj");

        List<GameObject> thrusters = new List<GameObject>();
        GameObject currentThruster;
        ParticleSystem particle;
        Rigidbody2D RB;
        thrusters = getObjWithTag(recursiveFindChildren(thrusterLayer), "Thruster");
        if (thrusters.Count != 0)
        {

            //print("index/ships: " + index + " " + (ships.Count - 1));

            //print("ship: " + ships[index].name);
            //GameObject k = ships[index];
            currentThruster = thrusters[index];                         // the index of 0 here is just to turn the list into obj
            particle = currentThruster.GetComponent<ParticleSystem>();


            var tempShip = ship.SetShip(0);
            for (var i = 0; i < tempShip.ships.Count; i++)
            {
                tempShip = ship.SetShip(i);

                if (tempShip.floorMap.HasTile(tempShip.floorMap.WorldToCell(new Vector3(currentThruster.transform.position.x, currentThruster.transform.position.y, 0))))
                {
                    break;

                }

            }
            RB = tempShip.floor.GetComponent<Rigidbody2D>();
        }
        else
        {
            currentThruster = null;
            particle = null;
            RB = null;
        }


        // Initilise touple below and return.
        (List<GameObject> thrusters, GameObject currentThruster, ParticleSystem particle, Rigidbody2D ShipFloorRB) d;
        d = (thrusters, currentThruster, particle, RB);

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
