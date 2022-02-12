using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : TileTypes
{
    public int curXpos = 0;
    public int curYpos = 0;
    public int shiftAmtXpos;
    public int shiftAmtYpos;
    public int numOfTimesPlaced;
    public bool checkTopOverlap;
    public bool checkBottomOverlap;
    public bool checkLeftOverlap;
    public bool checkRightOverlap;
    public string tileName;
    public string curTileName;

    public bool tile1IsValid;
    public bool tile2IsValid;
    public bool tile3IsValid;
    public bool tile4IsValid;
    public bool tile5IsValid;
    public bool tile6IsValid;
    List<string> validTiles;

    void Start()
    {
        PlaceStartingTile();
        numOfTimesPlaced = 0;
    }

    void Update()
    {
        SpawnNewTile();
    }

    public void PlaceStartingTile()
    {
        //Fetch the starting tile
        //GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load("BasicTiles/Tile3"));
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
            if (numOfTimesPlaced < 1)
            {
                tileName = "StartingTile";
                shiftAmtYpos += 1;
               //shiftAmtXpos += 1;
            }

            else
            {
                FetchNewTile();
            }

            //Fetch the new tile
            GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load(tileName));

            //Place the new tile
            GameObject tile = (GameObject)Instantiate(referenceStartTile, transform);
            tile.transform.position = new Vector2(curXpos + shiftAmtXpos, curYpos + shiftAmtYpos);

            //Destroy the temporary reference object
            Destroy(referenceStartTile);

            //increment numOfTimesPlaced
            numOfTimesPlaced++;
        }
    }

    public void FetchNewTile()
    {
        CheckTileOverlap();
        CheckForValidTiles();
        GetValidTiles();

    }

    public void GetValidTiles()
    {
        validTiles = new List<string>();

        if (tile1IsValid)
        {
            validTiles.Add(basicTiles[0]);
        }

        if (tile2IsValid)
        {
            validTiles.Add(basicTiles[1]);
        }

        if (tile3IsValid)
        {
            validTiles.Add(basicTiles[2]);
        }

        if (tile4IsValid)
        {
            validTiles.Add(basicTiles[3]);
        }

        if (tile5IsValid)
        {
            validTiles.Add(basicTiles[4]);
        }

        if (tile6IsValid)
        {
            validTiles.Add(basicTiles[5]);
        }
    }

public void CheckForValidTiles()
    {
        if (checkTopOverlap)
        {
            tile1IsValid = false;
            tile3IsValid = false;
            tile4IsValid = false;
        }

        else
        {
            tile1IsValid = true;
            tile3IsValid = true;
            tile4IsValid = true;
        }

        if (checkBottomOverlap)
        {
            tile1IsValid = false;
            tile5IsValid = false;
            tile6IsValid = false;
        }

        else
        {
            tile1IsValid = true;
            tile5IsValid = true;
            tile6IsValid = true;
        }

        if (checkLeftOverlap)
        {
            tile2IsValid = false;
            tile3IsValid = false;
            tile6IsValid = false;
        }

        else
        {
            tile2IsValid = true;
            tile3IsValid = true;
            tile6IsValid = true;
        }


        if (checkRightOverlap)
        {
            tile2IsValid = false;
            tile4IsValid = false;
            tile5IsValid = false;
        }

        else
        {
            tile2IsValid = true;
            tile4IsValid = true;
            tile5IsValid = true;
        }
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
