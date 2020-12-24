using UnityEngine;

public class NeedleNorth : MonoBehaviour
{
    public Transform cam;
    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float rot = cam.transform.rotation.eulerAngles.z;
        //transform.rotation.eulerAngles.z = rot;
        //transform.localEulerAngles = new Vector3(0, 0, -rot);




        if (Time.fixedTime > time)
        {
            time = Time.fixedTime + (float).05;
            transform.localEulerAngles = new Vector3(0, 0, -rot);


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
