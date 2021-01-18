using UnityEngine;

public class BackgroundVariableTest
{
    public int index;
    public Vector2 pos;
    public Renderer rend;
    public GameObject obj;
    public Color[] pixles;
    public BackgroundVariableTest(int i, Vector2 p, Renderer r, GameObject o)
    {
        index = i;
        pos = p;
        rend = r;
        obj = o;
    }
    public BackgroundVariableTest(int i, Vector2 p, Renderer r, GameObject o, int wid, int heig)
    {
        index = i;
        pos = p;
        rend = r;
        obj = o;
        pixles = new Color[wid * heig];
    }

    // color to texture




}
