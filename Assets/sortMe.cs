using UnityEngine;

public class sortMe : MonoBehaviour
{
    public int OrderInLayer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        //renderer.sortingOrder = /*OrderInLayer*/;
        //renderer.renderingLayerMask = (uint)OrderInLayer;
        //renderer.sortingLayerID = OrderInLayer;
    }
}

