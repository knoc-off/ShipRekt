using UnityEngine;
using UnityEngine.Tilemaps;

public class GetTileMap : MonoBehaviour
{
    public GameObject shiplayers;
    private Component[] Tiles;

    // Update is called once per frame
    void Update()
    {
        Tiles = shiplayers.GetComponentsInChildren(typeof(Tilemap)); // hopefully i can itterate through the tiles with this somehow?
                                                                     //for(int i = 0; i < Tiles.Length; i++)

        //Debug.Log(Tiles[i]);

    }
}
