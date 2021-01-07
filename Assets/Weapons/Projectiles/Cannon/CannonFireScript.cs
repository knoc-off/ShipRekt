using System.Collections.Generic;
using UnityEngine;

public class CannonFireScript : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Camera camCam;
    public float force = 20f;
    private Vector2 pz;
    private float angle;
    private float frames;
    public float rotateSpeed = 0.03f;
    public ParticleSystem SmokeParticle;

    public Animator animator;

    public bool inCollider = false;

    //private CircleCollider2D collide;

    private int ammo = 20;
    private bool shot = true;
    //private bool Rotate = false;


    void Start()
    {
        angle = transform.eulerAngles.z;
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        //camCam = collision.gameObject.gameObject.GetComponent<Camera>();
        //force = 20f;
        //pz = collision.gameObject.gameObject.GetComponent<Camera>();
        //angle = ;
        //frames = ;
        if (collision.gameObject.tag == "player")
        {
            GameObject tempGobj = collision.gameObject.transform.parent.gameObject;
            //print("enter "+ tempGobj.GetComponent<Camera>().name);
            //print(recursiveFindChildren());
            //print("name2 "+collision.gameObject.GetComponentInParent);
            inCollider = true;
            foreach(var tempObj in recursiveFindChildren(tempGobj))
            {
                if (tempObj.GetComponent<Camera>())
                {
                    print(tempObj.GetComponent<Camera>().name);
                    camCam = tempObj.GetComponent<Camera>();
                    break;
                }

            }
            print(camCam.name);

        }


    }
    void OnTriggerExit2D(Collider2D collision)
    {
        print("exit " + collision.gameObject.name);

        if(collision.gameObject.tag == "player")
            inCollider = false;
    }
    //void FixedUpdate()
    //{
    //    angle += transform.parent.parent.GetComponent<Rigidbody2D>().angularVelocity/2;
    //}
    void Update()
    {
        //gameObject.transform.parent.rotation
        //print();
        if (shot)
            animator.SetBool("Fire", false);
        foreach (GameObject a in GameObject.FindGameObjectsWithTag("player"))
        {
            bool tempbool = false;
            if (inCollider)
            {
                tempbool = true;
                if (Input.GetButtonDown("Fire1") && shot)
                {
                    //animator.SetBool("Fire", false);
                    var floorRot = transform.parent.parent.GetComponent<Transform>().InverseTransformPoint(camCam.ScreenToWorldPoint(Input.mousePosition));

                    //Rotate = true;
                    shot = false;
                    frames = Time.fixedTime + 1.2f;
                    //print("rot start1");
                    //rotationDegreesAmount = angle;
                    pz = floorRot;
                    Vector2 lookDir = pz - (Vector2)transform.localPosition;
                    angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                    print("angle " + angle + " Atan2+delta ");
                }
                //if (Rotate)
                //{
                //    //transform.eulerAngles = new Vector3(0, 0, angle);
                //    RotateTo(angle);
                //}

                if (RotateTo(angle) && Time.fixedTime > frames && !shot)
                {
                    ammo--;
                    shot = true;
                    //if(ammo > 0)
                    //rigbdy.AddForce(rigbdy.transform.up * -1000);
                    // = firePoint.gameObject.GetComponent<ParticleSystem>();
                    SmokeParticle.Play();

                    animator.SetBool("Fire", true);

                    shoot();
                }


            }
            if (!tempbool)
                shot = true;
        }
        //else
        //{
        //    angle = 0;
        //}

    }

    bool RotateTo(float angle)
    {
        //Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 newRotation = new Vector3(0, 0, angle);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(transform.eulerAngles.z, transform.eulerAngles.z + Mathf.DeltaAngle(transform.eulerAngles.z, angle), rotateSpeed)); //Quaternion.Slerp(transform.rotation, newRotation, .05f);
        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, angle)) < 2)
            return true;
        return false;
    }
    void shoot()
    {

        //print("shoot");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * force, ForceMode2D.Impulse);
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



}
