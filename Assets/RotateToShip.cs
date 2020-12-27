using UnityEngine;
using UnityEngine.Tilemaps;


public class RotateToShip : MonoBehaviour
{


    public GameObject shipLayers;
    private Component[] Tiles;
    private Tilemap tempMap;
    public Transform player;
    public int RotationSpeed = 20; // speed at which the camera revolves when standing on tile
    //public Tile highlightTile;
    //public Tilemap highlightMap;
    private float x;
    private float y;
    private Vector3 rotateValue;
    private GameObject[] ships;
    private Tilemap prevMap;
    private Vector3 local;


    public float sleepTime;

    private float rotateTime;

    private bool TurnLeft = false;
    private bool TurnRight = false;


    private void Start()
    {


    }


    // Update is called once per frame
    void Update()
    {


        // if button pressed set bool then say once diff is < a number to set bool back to false
        if (Input.GetAxis("Turn") == -1 && !TurnLeft && !TurnRight)
        {
            print("left");
            rotateTime = Time.fixedTime;
            TurnLeft = true;
        }
        // diff = Mathf.DeltaAngle(templ.eulerAngles.z, transform.eulerAngles.z+90);

        if (Input.GetAxis("Turn") == 1 && !TurnLeft && !TurnRight)
        {
            print("right");
            rotateTime = Time.fixedTime;
            TurnRight = true;
        }

        if (rotateTime + sleepTime > Time.fixedTime)
        {
            if (TurnLeft)
                RotateTo(GetNearest90Minus(), RotationSpeed * (sleepTime * (float)6.5));
            if (TurnRight)
                RotateTo(GetNearest90Plus(), RotationSpeed * (sleepTime * (float)6.5));
        }
        else
        {
            TurnLeft = false;
            TurnRight = false;
            RotateTo(GetNearest90(), RotationSpeed); // rotate to ships nearest 90 degree angle
        }






        //ships = GameObject.FindGameObjectsWithTag("Ship");

        //for (int i = 0; i < ships.Length; i++) // loop through every tilemap
        //{
        //    //floorMap = (Tilemap)TileMaps[i];
        //    tempMap = ships[i].GetComponentsInChildren<Tilemap>()[0];
        //}



        //y = Input.GetAxis("Mouse X");
        //x = Input.GetAxis("Mouse Y");
        //Debug.Log(x + ":" + y);
        //rotateValue = new Vector3(x, y * -1, 0);
        //rotateValue = highlightMap.transform.rotation.eulerAngles;
        //transform.eulerAngles = highlightMap.transform.rotation.eulerAngles;
    }

    float GetNearest90Minus()
    {
        Tiles = shipLayers.GetComponentsInChildren(typeof(Tilemap));
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempMap = (Tilemap)Tiles[i];
            if (tempMap.HasTile(tempMap.WorldToCell(new Vector3(player.transform.position.x, player.transform.position.y, 0))))
            {



                Vector3 rotVal = Tiles[i].transform.rotation.eulerAngles;       // ship up this should snap to 90
                //print(rotVal.z); // add 90 check if thats closer to origin 



                //print(rotVal.z + "  " + transform.eulerAngles.z);
                //rotVal = Tiles[i].transform.rotation.eulerAngles;

                Quaternion rotation = Quaternion.Euler(0, 0, rotVal.z);
                Quaternion templ = Quaternion.Euler(0, 0, rotVal.z);




                float diff = 99999;

                for (int k = 0; k < 4; k++)
                {

                    if (Mathf.Abs(Mathf.DeltaAngle(rotation.eulerAngles.z, transform.eulerAngles.z)) < Mathf.Abs(diff))
                    {
                        diff = Mathf.DeltaAngle(rotation.eulerAngles.z, transform.eulerAngles.z);
                        templ = rotation;
                    }
                    rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 90);

                }



                //diff = Mathf.DeltaAngle(templ.eulerAngles.z, transform.eulerAngles.z - 90);
                //transform.eulerAngles = templ.eulerAngles; // Tiles[i].transform.rotation.eulerAngles


                return diff + 70; // if anything breaks its gonna be here//




            }
        }
        return 0;
    }


    float GetNearest90Plus()
    {
        Tiles = shipLayers.GetComponentsInChildren(typeof(Tilemap));
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempMap = (Tilemap)Tiles[i];
            if (tempMap.HasTile(tempMap.WorldToCell(new Vector3(player.transform.position.x, player.transform.position.y, 0))))
            {



                Vector3 rotVal = Tiles[i].transform.rotation.eulerAngles;       // ship up this should snap to 90
                //print(rotVal.z); // add 90 check if thats closer to origin 



                //print(rotVal.z + "  " + transform.eulerAngles.z);
                //rotVal = Tiles[i].transform.rotation.eulerAngles;

                Quaternion rotation = Quaternion.Euler(0, 0, rotVal.z);
                Quaternion templ = Quaternion.Euler(0, 0, rotVal.z);




                float diff = 99999;

                for (int k = 0; k < 4; k++)
                {

                    if (Mathf.Abs(Mathf.DeltaAngle(rotation.eulerAngles.z, transform.eulerAngles.z)) < Mathf.Abs(diff))
                    {
                        diff = Mathf.DeltaAngle(rotation.eulerAngles.z, transform.eulerAngles.z);
                        templ = rotation;
                    }
                    rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 90);

                }




                //diff = Mathf.DeltaAngle(templ.eulerAngles.z, transform.eulerAngles.z - 90);
                //transform.eulerAngles = templ.eulerAngles; // Tiles[i].transform.rotation.eulerAngles


                return diff - 70; // if anything breaks its gonna be here//




            }
        }
        return 0;
    }


    float GetNearest90()
    {
        Tiles = shipLayers.GetComponentsInChildren(typeof(Tilemap));
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempMap = (Tilemap)Tiles[i];
            if (tempMap.HasTile(tempMap.WorldToCell(new Vector3(player.transform.position.x, player.transform.position.y, 0))))
            {



                Vector3 rotVal = Tiles[i].transform.rotation.eulerAngles;       // ship up this should snap to 90
                //print(rotVal.z); // add 90 check if thats closer to origin 



                //print(rotVal.z + "  " + transform.eulerAngles.z);
                //rotVal = Tiles[i].transform.rotation.eulerAngles;

                Quaternion rotation = Quaternion.Euler(0, 0, rotVal.z);
                Quaternion templ = Quaternion.Euler(0, 0, rotVal.z);




                float diff = 99999;

                for (int k = 0; k < 4; k++)
                {

                    if (Mathf.Abs(Mathf.DeltaAngle(rotation.eulerAngles.z, transform.eulerAngles.z)) < Mathf.Abs(diff))
                    {
                        diff = Mathf.DeltaAngle(rotation.eulerAngles.z, transform.eulerAngles.z);
                        templ = rotation;
                    }
                    rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 90);

                }



                //diff = Mathf.DeltaAngle(templ.eulerAngles.z, transform.eulerAngles.z - 90);
                //transform.eulerAngles = templ.eulerAngles; // Tiles[i].transform.rotation.eulerAngles


                return diff; // if anything breaks its gonna be here//




            }
        }
        return 0;
    }
    void FaceUp()
    {
        Tiles = shipLayers.GetComponentsInChildren(typeof(Tilemap));
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempMap = (Tilemap)Tiles[i];
            if (tempMap.HasTile(tempMap.WorldToCell(new Vector3(player.transform.position.x, player.transform.position.y, 0))))
            {
                Vector3 rotVal = Tiles[i].transform.rotation.eulerAngles;       // ship up this should snap to 90
                //print(rotVal.z); // add 90 check if thats closer to origin 

                //diff = Mathf.DeltaAngle(templ.eulerAngles.z, transform.eulerAngles.z - 90);
                transform.eulerAngles = rotVal; // Tiles[i].transform.rotation.eulerAngles


            }
        }
    }

    void RotateTo(float delta, float rotSpeed)
    {
        try
        {

            if (Time.frameCount % 2 == 0)   // just to create some sort of visual delay. could make update every frame for a smoother look
            {
                if (delta < 0)  // rotate pos/ neg
                    transform.eulerAngles += new Vector3(0, 0, Mathf.Abs(delta / rotSpeed)); // var higher for slower rotation lower for inverse
                else
                    transform.eulerAngles -= new Vector3(0, 0, Mathf.Abs(delta / rotSpeed)); // Tiles[i].transform.rotation.eulerAngles
            }
        }
        catch
        {
            print("don fucked"); // its don fucked
        }
        //RotateInProgress = true;
    }


}
