using UnityEngine;

public class NoiseBack : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 22f;
    private float delay = 0f;
    public Transform cam;
    public float offsetX;
    public float offsetY;
    public float RandOffsetX;
    public float RandOffsetY;
    public float divide;
    private bool direction = true;


    private void Start()
    {
        RandOffsetX = Random.Range(-999, 999);
        RandOffsetY = Random.Range(-999, 999);
        //RandOffsetX = 10;
        //RandOffsetY = 10;
    }

    // Start is called before the first frame update
    void Update()
    {
        transform.position = new Vector3(cam.position.x, cam.position.y, 0);
        if (Time.fixedTime > delay)
        {
            //offsetX = RandOffsetX;
            //offsetY = RandOffsetY;

            offsetX = cam.position.x / (10);
            offsetY = cam.position.y / (10);
            //offsetX += (float)(0.01);
            //offsetY += (float)(0.01);
            if (direction)
            {
                divide += (float).002;
                if (divide > 1)
                    direction = !direction;
            }
            else
            {
                divide -= (float).002;
                if (divide < -1)
                    direction = !direction;
            }

            delay = Time.fixedTime + (float).05;
            //scale += (float).02;
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = GenerateTexture();
        }
    }



    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                //color.b = 256;
                var div = 1;

                color *= (float)-.4;

                color += new Color((float)(0.3671875 * div), (float)(0.609 * div), (float)(.609 * div));


                texture.SetPixel(x, y, color);
                

                //texture.SetPixels(x, y, 5, 5, colors);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)

    {

        float xCoord = ((float)(x) / width * scale + offsetX) * divide;
        float yCoord = ((float)(y) / height * scale + offsetY) * divide;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);


    }
}
