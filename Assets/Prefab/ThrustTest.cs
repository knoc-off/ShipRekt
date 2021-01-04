using UnityEngine;

public class ThrustTest : MonoBehaviour
{
    private ShipTupleClass ship = new ShipTupleClass();
    [Range(-20, 20)]
    public float force = 20f;
    private GameObject shipLayer;

    public float localPosX;
    public float localPosY;
    public float Rotation;


    // Start is called before the first frame update
    void Start()
    {
        localPosX = transform.localPosition.x;
        localPosY = transform.localPosition.y;
        Rotation = transform.localRotation.z;
        ship.shipLayer = GameObject.FindGameObjectWithTag("shipLayer");
        //shipLayer = GameObject.FindGameObjectWithTag("shipLayer");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var tempShip = ship.SetShip(0);
        for (var i = 0; i < tempShip.ships.Count; i++)
        {
            tempShip = ship.SetShip(i);

            if (tempShip.floorMap.HasTile(tempShip.floorMap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0))))
            {
                break;

            }

        }


        Animator Anim = GetComponent<Animator>();
        //Rigidbody2D ThrusterRB = GetComponent<Rigidbody2D>();

        Rigidbody2D RB = tempShip.floor.GetComponent<Rigidbody2D>();

        Anim.speed = RB.GetPointVelocity(new Vector2(transform.position.x, transform.position.y)).sqrMagnitude * Random.Range(0.75f, 1.25f);
        //var force = new Vector2(0, 5) * new Vector2(transform.rotation.x, transform.rotation.y);
        RB.AddForceAtPosition(transform.up * (force * 5), new Vector2(transform.position.x, transform.position.y));
    }

}
