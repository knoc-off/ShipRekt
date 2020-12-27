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
    public float divide = 55;
    private bool direction = true;
    private FastNoise noise = new FastNoise();
    private Renderer renderer;
    public float sizex;
    private void Start()
    {
        sizex = transform.localScale.x / width;
        RandOffsetX = Random.Range(-999, 999);
        RandOffsetY = Random.Range(-999, 999);
        renderer = GetComponent<Renderer>();
        //RandOffsetX = 10;
        //RandOffsetY = 10;
    }

    // Start is called before the first frame update
    void Update()
    {

        //if(transform.position.x > cam.transform.position.x + sizex)
        //    ;
        //float output = ((cam.position.x / sizex) + 1) * sizex;
        //transform.position = new Vector3(cam.position.x, cam.position.y, 0);
        if (Time.fixedTime > delay)
        {
            delay = Time.fixedTime + 1;
            offsetX = cam.position.x * (scale / divide);
            offsetY = cam.position.y * (scale / divide);
            renderer.material.mainTexture = GenerateTexture();
        }
    }



    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                //color.b = 256;
                var div = 1;

                color *= (float)-.3;

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

        float xCoord = ((float)(x) / width * scale + offsetX);
        float yCoord = ((float)(y) / height * scale + offsetY);

        float sample = noise.GetSimplex(xCoord, yCoord);//noise.GetSimplex(xCoord, yCoord); //noise.GetPerlin(xCoord,yCoord);//Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);


    }
}
