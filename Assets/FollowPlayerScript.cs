using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{

    public Transform player;

    public float SmoothSpeed = 0.125f;

    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;


    private void Start()
    {
        transform.position = player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, SmoothSpeed); //player.position + offset;


    }
}
