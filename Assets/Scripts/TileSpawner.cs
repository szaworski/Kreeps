using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public int curXpos = 0;
    public int curYpos = 0;
    public int shiftAmtXpos;
    public int shiftAmtYpos;
    public bool checkTopOverlap;
    public bool checkBottomOverlap;
    public bool checkLeftOverlap;
    public bool checkRightOverlap;

    void Start()
    {
        PlaceStartingTile();
    }

    void Update()
    {
        SpawnNewTile();
    }

    public void PlaceStartingTile()
    {
        //Fetch the starting tile
        GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load("StartingTile"));

        //Place the starting tile
        GameObject tile = (GameObject)Instantiate(referenceStartTile, transform);

        tile.transform.position = new Vector2(curXpos, curYpos);
        //Destroy the temporary reference object
        Destroy(referenceStartTile);
    }

    public void SpawnNewTile()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //shiftAmtYpos += 1;
            shiftAmtXpos += 1;

            String tileName = "StartingTile";

            //Fetch the new tile
            GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load(tileName));

            //Place the new tile
            GameObject tile = (GameObject)Instantiate(referenceStartTile, transform);
            tile.transform.position = new Vector2(curXpos + shiftAmtXpos, curYpos + shiftAmtYpos);

            //Destroy the temporary reference object
            Destroy(referenceStartTile);
            CheckTileOverlap();
        }
    }

    public void FetchNewTile()
    {

    }

    public void CheckTileOverlap()
    {
        Vector3 top = transform.position + new Vector3(0 + shiftAmtXpos, 0.65f + shiftAmtYpos, 0);
        checkTopOverlap = Physics2D.OverlapBox(top, new Vector3(0, 0, 0), 0f, LayerMask.GetMask("GroundTile"));
        Debug.Log("checkTopOverlap = " + checkTopOverlap);

        Vector3 bottom = transform.position - new Vector3(0 - shiftAmtXpos, 0.65f - shiftAmtYpos, 0);
        checkBottomOverlap = Physics2D.OverlapBox(bottom, new Vector3(0, 0, 0), 0f, LayerMask.GetMask("GroundTile"));
        Debug.Log("checkBottomOverlap = " + checkBottomOverlap);

        Vector3 left = transform.position - new Vector3(0.65f - shiftAmtXpos, 0 - shiftAmtYpos, 0);
        checkLeftOverlap = Physics2D.OverlapBox(left, new Vector3(0, 0, 0), 0f, LayerMask.GetMask("GroundTile"));
        Debug.Log("checkLeftOverlap = " + checkLeftOverlap);

        Vector3 right = transform.position + new Vector3(0.65f + shiftAmtXpos, 0 + shiftAmtYpos, 0);
        checkRightOverlap = Physics2D.OverlapBox(right, new Vector3(0, 0, 0), 0f, LayerMask.GetMask("GroundTile"));
        Debug.Log("checkRightOverlap = " + checkRightOverlap);
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 top = transform.position + new Vector3(0 + shiftAmtXpos, 0.65f + shiftAmtYpos, 0);
        Gizmos.DrawWireCube(top, new Vector3(1, 0.25f, 0));

        Gizmos.color = Color.blue;
        Vector3 bottom = transform.position - new Vector3(0 - shiftAmtXpos, 0.65f - shiftAmtYpos, 0);
        Gizmos.DrawWireCube(bottom, new Vector3(1, 0.25f, 0));

        Gizmos.color = Color.yellow;
        Vector3 left = transform.position - new Vector3(0.65f - shiftAmtXpos, 0 - shiftAmtYpos, 0);
        Gizmos.DrawWireCube(left, new Vector3(0.25f, 1, 0));

        Gizmos.color = Color.green;
        Vector3 right = transform.position + new Vector3(0.65f + shiftAmtXpos, 0 + shiftAmtYpos, 0);
        Gizmos.DrawWireCube(right, new Vector3(0.25f, 1, 0));
    }
}
