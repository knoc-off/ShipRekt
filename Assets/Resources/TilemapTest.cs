using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{

    public Tile highlightTile;
    public Tilemap highlightMap;
    public Camera Cam;

    private Vector3 rotateValue;
    private Vector3Int previous;

    // Start is called before the first frame update
    /*
    void Start()
    {
        
    }
    */
    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3Int currentCell = highlightMap.WorldToCell(transform.position);
        currentCell.x += 1;
        //highlightMap.GetTransformMatrix(Vector3Int).rotation;
        rotateValue = highlightMap.transform.rotation.eulerAngles;
        Cam.transform.eulerAngles = Cam.transform.eulerAngles - rotateValue;
        //Debug.Log("Text: " + rotateValue);
        if (currentCell != previous)
        {
            // set the new tile
            highlightMap.SetTile(currentCell, highlightTile);
             
            // erase previous
            highlightMap.SetTile(previous, null);

            // save the new position for next frame
            previous = currentCell;
        }

    }
}
