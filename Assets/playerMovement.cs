using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerMovement : MonoBehaviour
{
    public GameObject shipLayer;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Transform cam;
    public Camera camCam;
    private GameObject[] ships;
    private Tilemap tempMap;
    private bool OnTile;
    private float tempTime;
    Vector2 movement;

    private Vector2 ShipVelocity;

    // Update is called once per frame
    void Update() // input
    {

        //print("on" + OnTile);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = Vector2.ClampMagnitude(movement, 1);



        //animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);
        animator.SetBool("Swimming", !OnTile);

        if (Input.GetAxis("Run") == 1)
            moveSpeed = 5;
        else
            moveSpeed = 4;

        if (OnTile == false)
            moveSpeed = 3f;


        if (OnTile) // change to only change mass if contact = wall of the floor im standing on. but good temp fix
        {
            rb.mass = 0;
        }
        else
            rb.mass = 1;


        /*
        Vector2 moveDirection = gameObject.GetComponent<Rigidbody2D>().velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        */


    }
    void FixedUpdate() // movement
    {


        var tempShip = SetShip(0);
        ShipVelocity = new Vector2(0, 0);
        for (var i = 0; i < tempShip.ships.Count; i++)
        {
            tempShip = SetShip(i);
            OnTile = false;

            if (tempShip.floorMap.HasTile(tempShip.floorMap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0))))
            {
                OnTile = true;
                break;

            }

        }

        //ShipVelocity = tempShip.floorMap.WorldToLocal(new Vector3(transform.position.x, transform.position.y,0)); //tempShip.floorMap.GetComponent<Rigidbody2D>().velocity;

        ShipVelocity = tempShip.floorMap.GetComponent<Rigidbody2D>().GetPointVelocity(new Vector2(transform.position.x, transform.position.y));
        if (!OnTile)
            ShipVelocity = new Vector2(0,0);
        Vector2 pz = camCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 camF = cam.up;
        Vector3 camR = cam.right;

        camF = camF.normalized;
        camR = camR.normalized;
        rb.velocity = ShipVelocity;
        //transform.position += (camF * (movement.y+ (ShipVelocity.y / 4)) + camR * (movement.x+ (ShipVelocity.x / 4))) * Time.fixedDeltaTime * moveSpeed; //new Vector3(movement.x, movement.y, 0) * Time.fixedDeltaTime *  moveSpeed;

        //transform.position += new Vector3((ShipVelocity.x/10), (ShipVelocity.y/10), 0);

        transform.position += (camF * movement.y + camR * movement.x) * Time.fixedDeltaTime * moveSpeed; //new Vector3(movement.x, movement.y, 0) * Time.fixedDeltaTime *  moveSpeed;
        //
        //Debug.Log(camF);
        //Debug.Log(camR);
        //if (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") != 0)
        //{
        //    Debug.Log(camF * movement.y + camR * movement.x);
        //}

        //camF.z = 0;
        //camR.z = 0;



        Vector3 dir = -movement;
        float angle = Mathf.Atan2(dir.x, -dir.y) * Mathf.Rad2Deg;
        angle += cam.localRotation.eulerAngles.z;
        //Debug.Log(angle);



        if (Input.GetAxis("Fire1") == 1 )
        {
            //print("rot start2");
            tempTime = Time.fixedTime+(float).5;
            //Vector2 lookDir = pz - rb.position;
            //angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        }



        if (movement.sqrMagnitude > 0 && tempTime < Time.fixedTime)
            rb.rotation = angle;

        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //rb.MovePosition(rb.position + (camF * movement.y + camR * movement.x) * Time.fixedDeltaTime * moveSpeed);
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    (List<GameObject> ships, List<GameObject> sprites, GameObject currentShip, GameObject spriteLayer, GameObject floor, GameObject wall, Tilemap floorMap, Tilemap wallMap) SetShip(int index)
    {
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
