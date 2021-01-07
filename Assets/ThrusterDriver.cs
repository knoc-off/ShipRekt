using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterDriver : MonoBehaviour
{
    public GameObject thrusterLayer;
    [Range(-20, 20)] public List<float> thrusters = new List<float>();

    public void Start()
    {
        
        for(int i = 0; i < setThruster(0).thrusters.Count; i++)
        {
            thrusters.Add(setThruster(i).currentThruster.GetComponent<ThrustTest>().force);
        }
    }


    public void Update()
    {
        try
        {

        for (int i = 0; i < setThruster(0).thrusters.Count; i++)
        {
            setThruster(i).currentThruster.GetComponent<ThrustTest>().force = thrusters[i];
        }
        }
        catch
        {
            thrusters.Clear();
            for (int i = 0; i < setThruster(0).thrusters.Count; i++)
            {
                thrusters.Add(setThruster(i).currentThruster.GetComponent<ThrustTest>().force);
            }
            print("no thrusters");
        }
    }


    public (List<GameObject> thrusters, GameObject currentThruster, ParticleSystem particle) setThruster(int index)
    {
        if (thrusterLayer == null)
            Debug.LogError("thruster layer is null. set to obj");

        List<GameObject> thrusters = new List<GameObject>();
        GameObject currentThruster;
        ParticleSystem particle;

        thrusters = getObjWithTag(recursiveFindChildren(thrusterLayer), "Thruster");
        //print("index/ships: " + index + " " + (ships.Count - 1));

        //print("ship: " + ships[index].name);
        //GameObject k = ships[index];
        if(thrusters.Count != 0)
        {
        currentThruster = thrusters[index];                         // the index of 0 here is just to turn the list into obj
        particle = currentThruster.GetComponent<ParticleSystem>();
        }
        else
        {
            currentThruster = null;
            particle = null;
        }
        // Initilise touple below and return.

        (List<GameObject> thrusters, GameObject currentThruster, ParticleSystem particle) d;
        d = (thrusters, currentThruster, particle);

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
