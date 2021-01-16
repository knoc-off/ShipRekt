using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FollowPlayerScript : MonoBehaviour
{

    public Button CompassButton;

    public Transform player;

    public Camera camCam;

    public float SmoothSpeed = 0.125f;

    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;



    private void Start()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10) + offset;

        CompassButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        var pz = camCam.ScreenToWorldPoint(Input.mousePosition);

        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");

        Transform compassTransform = CompassButton.gameObject.transform;

        print("Compass pos: "+compassTransform.position);
        


        print("Mouse Pos: "+pz);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var temp = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, SmoothSpeed) + offset; //player.position + offset;
        temp.z = -10;
        transform.position = temp;
    }
}
