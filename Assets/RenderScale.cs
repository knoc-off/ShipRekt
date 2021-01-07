using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderScale : MonoBehaviour
{

    public Camera cam;
    public RenderTexture rendtext;
    public int divisor = 1;
    // Start is called before the first frame update
    
    void Start()
    {
        
        int width = cam.pixelWidth;
        int height = cam.pixelHeight;
        rendtext.width = width / divisor;
        rendtext.height = height / divisor;
        


    }

}
