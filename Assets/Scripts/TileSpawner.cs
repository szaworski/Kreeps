using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : TileTypes
{
    public int shiftAmtXpos;
    public int shiftAmtYpos;
    public int shiftSpawnerXpos;
    public int shiftSpawnerYpos;
    public int numOfTimesPlaced;

    public bool checkTopOverlap;
    public bool checkOuterTopOverlap;
    public bool checkOuterTop2Overlap;
    public bool checkBottomOverlap;
    public bool checkOuterBottomOverlap;
    public bool checkOuterBottom2Overlap;
    public bool checkLeftOverlap;
    public bool checkOuterLeftOverlap;
    public bool checkOuterLeft2Overlap;
    public bool checkRightOverlap;
    public bool checkOuterRightOverlap;
    public bool checkOuterRight2Overlap;

    public bool checkTopRightOverlap;
    public bool checkOuterTopRightOverlap;
    public bool checkTopLeftOverlap;
    public bool checkOuterTopLeftOverlap;
    public bool checkBottomLeftOverlap;
    public bool checkOuterBottomLeftOverlap;
    public bool checkBottomRightOverlap;
    public bool checkOuterBottomRightOverlap;

    public string prependTileName;
    public string tileName;
    public string newTileName;
    public string moveDirection;

    public string[] curTiles;
    public bool[] validTiles;
    List<string> validTilesList;

    void Start()
    {
        PlaceStartingTile();
        numOfTimesPlaced = 0;
        curTiles = new string[6];
        validTiles = new bool[6];
    }

    void Update()
    {
        SpawnNewTile();
    }

    public void PlaceStartingTile()
    {
        tileName = "StartingTile";

        //Fetch the starting tile
        //GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load("BasicTiles/Tile3"));
        GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load("StartingTile"));
        GameObject TileHolder = GameObject.Find("TileHolder");

        //Place the starting tile
        GameObject tile = (GameObject)Instantiate(referenceStartTile, TileHolder.transform);
        tile.transform.SetParent(TileHolder.transform);
        tile.transform.position = new Vector2(0, 0);

        //Destroy the temporary reference object
        Destroy(referenceStartTile);

        MoveSpawnPos();
    }

    public void SpawnNewTile()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetNewTile();
            FindNewSpawnDirection();
            prependTileName = "BasicTiles/";
            //Debug.Log("Tile Pathway: " + prependTileName + tileName);

            //Find the tile in the resources folder
            //GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load("BasicTiles/Tile3"));
            GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load(prependTileName + tileName));
            GameObject TileHolder = GameObject.Find("TileHolder");

            //Place the new tile
            GameObject tile = (GameObject)Instantiate(referenceStartTile, TileHolder.transform);
            tile.transform.SetParent(TileHolder.transform);
            tile.transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos);

            //Destroy the temporary reference object
            Destroy(referenceStartTile);

            //increment numOfTimesPlaced
            numOfTimesPlaced++;

            //Move the Spawn position
            MoveSpawnPos();
        }
    }

    public void GetNewTile()
    {
        CheckTileOverlap();
        CheckForValidTiles();
        GetListOfValidTiles();
        ChooseRandTileFromList();
        SetNewTilePosition();
    }

    public void PrependTilenamePath()
    {


    }

    public void MoveSpawnPos()
    {
        if (tileName == "StartingTile")
        {
            transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos + 1);
        }

        else if (moveDirection == "up")
        {
            transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos + 1);
        }

        else if (moveDirection == "down")
        {
            transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos - 1);
        }

        else if (moveDirection == "left")
        {
            transform.position = new Vector2(shiftAmtXpos - 1, shiftAmtYpos);
        }

        else if (moveDirection == "right")
        {
            transform.position = new Vector2(shiftAmtXpos + 1, shiftAmtYpos);
        }
    }

    public void FindNewSpawnDirection()
    {
        tileName = newTileName;

        if (tileName == curTiles[0])
        {
            if (!checkTopOverlap)
            {
                moveDirection = "up";
            }

            else if (!checkBottomOverlap)
            {
                moveDirection = "down";
            }
        }

        else if (tileName == curTiles[1])
        {
            if (!checkRightOverlap)
            {
                moveDirection = "right";
            }

            else if (!checkLeftOverlap)
            {
                moveDirection = "left";
            }
        }

        else if (tileName == curTiles[2])
        {
            if (!checkRightOverlap)
            {
                moveDirection = "right";
            }

            else if (!checkBottomOverlap)
            {
                moveDirection = "down";
            }
        }

        else if (tileName == curTiles[3])
        {
            if (!checkLeftOverlap)
            {
                moveDirection = "left";
            }

            else if (!checkBottomOverlap)
            {
                moveDirection = "down";
            }
        }

        else if (tileName == curTiles[4])
        {
            if (!checkLeftOverlap)
            {
                moveDirection = "left";
            }

            else if (!checkTopOverlap)
            {
                moveDirection = "up";
            }
        }

        else if (tileName == curTiles[5])
        {
            if (!checkRightOverlap)
            {
                moveDirection = "right";
            }

            else if (!checkTopOverlap)
            {
                moveDirection = "up";
            }
        }
    }

    public void ChooseRandTileFromList()
    {
        var rand = new System.Random();
        int index = rand.Next(validTilesList.Count);
        //Debug.Log("The randomly Chosen tile is: " + validTilesList[index]);
        newTileName = validTilesList[index];
    }

    public void SetNewTilePosition()
    {
        Debug.Log("Current tile name: " + tileName);

        if (tileName == "StartingTile")
        {
            shiftAmtYpos += 1;
        }

        else if (tileName == curTiles[0])
        {
            if (!checkTopOverlap)
            {
                shiftAmtYpos += 1;
            }

            else if (!checkBottomOverlap)
            {
                shiftAmtYpos += -1;
            }
        }

        else if (tileName == curTiles[1])
        {
            if (!checkRightOverlap)
            {
                shiftAmtXpos += 1;
            }

            else if (!checkLeftOverlap)
            {
                shiftAmtXpos += -1;
            }
        }

        else if (tileName == curTiles[2])
        {
            if (!checkRightOverlap && checkLeftOverlap)
            {
                shiftAmtXpos += 1;
            }

            else if (!checkBottomOverlap && checkTopOverlap)
            {
                shiftAmtYpos += -1;
            }
        }

        else if (tileName == curTiles[3])
        {
            if (!checkLeftOverlap && checkRightOverlap)
            {
                shiftAmtXpos += -1;
            }

            else if (!checkBottomOverlap && checkTopOverlap)
            {
                shiftAmtYpos += -1;
            }
        }

        else if (tileName == curTiles[4])
        {
            if (!checkLeftOverlap && checkRightOverlap)
            {
                shiftAmtXpos += -1;
            }

            else if (!checkTopOverlap && checkBottomOverlap)
            {
                shiftAmtYpos += 1;
            }
        }

        else if (tileName == curTiles[5])
        {
            if (!checkRightOverlap && checkLeftOverlap)
            {
                shiftAmtXpos += 1;
            }

            else if (!checkTopOverlap && checkBottomOverlap)
            {
                shiftAmtYpos += 1;
            }
        }
    }

    public void GetListOfValidTiles()
    {
        validTilesList = new List<string>();
        curTiles = basicTiles;

        Debug.Log("Tile 1 bool: " + validTiles[0]);
        Debug.Log("Tile 2 bool: " + validTiles[1]);
        Debug.Log("Tile 3 bool: " + validTiles[2]);
        Debug.Log("Tile 4 bool: " + validTiles[3]);
        Debug.Log("Tile 5 bool: " + validTiles[4]);
        Debug.Log("Tile 6 bool: " + validTiles[5]);


        if (validTiles[0])
        {
            validTilesList.Add(curTiles[0]);
        }

        if (validTiles[1])
        {
            validTilesList.Add(curTiles[1]);
        }

        if (validTiles[2])
        {
            validTilesList.Add(curTiles[2]);
        }

        if (validTiles[3])
        {
            validTilesList.Add(curTiles[3]);
        }

        if (validTiles[4])
        {
            validTilesList.Add(curTiles[4]);
        }

        if (validTiles[5])
        {
            validTilesList.Add(curTiles[5]);
        }
    }

    public void CheckForValidTiles()
    {
        //Reset all values to false
        validTiles[0] = false;
        validTiles[1] = false;
        validTiles[2] = false;
        validTiles[3] = false;
        validTiles[4] = false;
        validTiles[5] = false;

        if (tileName == "StartingTile")
        {
            validTiles[0] = true;
            validTiles[2] = true;
            validTiles[3] = true;
        }

        else if (tileName == curTiles[0])
        {
            if (!checkTopOverlap)
            {
                CheckTopOverlaps();
            }

            else if (!checkBottomOverlap)
            {
                CheckBottomOverlaps();
            }
        }

        else if (tileName == curTiles[1])
        {
            if (!checkRightOverlap)
            {
                CheckRightOverlaps();
            }

            else if (!checkLeftOverlap)
            {
                CheckLeftOverlaps();
            }
        }

        else if (tileName == curTiles[2])
        {
            if (!checkRightOverlap)
            {
                CheckRightOverlaps();
            }

            else if (!checkBottomOverlap)
            {
                CheckBottomOverlaps();
            }
        }

        else if (tileName == curTiles[3])
        {
            if (!checkLeftOverlap)
            {
                CheckLeftOverlaps();
            }

            else if (!checkBottomOverlap)
            {
                CheckBottomOverlaps();
            }
        }

        else if (tileName == curTiles[4])
        {
            if (!checkTopOverlap)
            {
                CheckTopOverlaps();
            }

            else if (!checkLeftOverlap)
            {
                CheckLeftOverlaps();
            }
        }

        else if (tileName == curTiles[5])
        {
            if (!checkTopOverlap)
            {
                CheckTopOverlaps();
            }

            else if (!checkRightOverlap)
            {
                CheckRightOverlaps();
            }
        }
    }

    public void CheckTopOverlaps()
    {
        if (!checkTopOverlap)
        {
            validTiles[0] = true;
        }

        if (!checkRightOverlap)
        {
            validTiles[2] = true;
        }

        if (!checkLeftOverlap)
        {
            validTiles[3] = true;
        }
    }

    public void CheckBottomOverlaps()
    {
        if (!checkBottomOverlap)
        {
            validTiles[0] = true;
        }

        if (!checkRightOverlap)
        {
            validTiles[5] = true;
        }

        if (!checkLeftOverlap)
        {
            validTiles[4] = true;
        }
    }

    public void CheckRightOverlaps()
    {
        if (!checkRightOverlap)
        {
            validTiles[1] = true;
        }

        if (!checkTopOverlap)
        {
            validTiles[4] = true;
        }

        if (!checkBottomOverlap)
        {
            validTiles[3] = true;
        }
    }

    public void CheckLeftOverlaps()
    {
        if (!checkLeftOverlap)
        {
            validTiles[1] = true;
        }
        if (!checkTopOverlap)
        {
            validTiles[5] = true;
        }

        if (!checkBottomOverlap)
        {
            validTiles[2] = true;
        }
    }

    public void CheckTileOverlap()
    {
        Vector3 top = transform.position + new Vector3(0, 0.65f, 0);
        checkTopOverlap = Physics2D.OverlapBox(top, new Vector3(0.9f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkTopOverlap = " + checkTopOverlap);

        Vector3 outerTop = transform.position + new Vector3(0, 1.65f, 0);
        checkOuterTopOverlap = Physics2D.OverlapBox(outerTop, new Vector3(0.9f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkTopOverlap = " + checkTopOverlap);

        Vector3 outerTop2 = transform.position + new Vector3(0, 4f, 0);
        checkOuterTop2Overlap = Physics2D.OverlapBox(outerTop2, new Vector3(0.9f, 3f, 0), 0f, LayerMask.GetMask("GroundTile"));


        Vector3 bottom = transform.position - new Vector3(0, 0.65f, 0);
        checkBottomOverlap = Physics2D.OverlapBox(bottom, new Vector3(0.9f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomOverlap = " + checkBottomOverlap);

        Vector3 outerBottom = transform.position - new Vector3(0, 1.65f, 0);
        checkOuterBottomOverlap = Physics2D.OverlapBox(outerBottom, new Vector3(0.9f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomOverlap = " + checkBottomOverlap);

        Vector3 outerBottom2 = transform.position - new Vector3(0, 4f, 0);
        checkOuterBottom2Overlap = Physics2D.OverlapBox(outerBottom2, new Vector3(0.9f, 3f, 0), 0f, LayerMask.GetMask("GroundTile"));


        Vector3 left = transform.position - new Vector3(0.65f, 0, 0);
        checkLeftOverlap = Physics2D.OverlapBox(left, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkLeftOverlap = " + checkLeftOverlap);

        Vector3 outerLeft = transform.position - new Vector3(1.65f, 0, 0);
        checkOuterLeftOverlap = Physics2D.OverlapBox(outerLeft, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkLeftOverlap = " + checkLeftOverlap);

        Vector3 outerLeft2 = transform.position - new Vector3(4f, 0, 0);
        checkOuterLeft2Overlap = Physics2D.OverlapBox(outerLeft2, new Vector3(3f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));


        Vector3 right = transform.position + new Vector3(0.65f, 0, 0);
        checkRightOverlap = Physics2D.OverlapBox(right, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkRightOverlap = " + checkRightOverlap);

        Vector3 outerRight = transform.position + new Vector3(1.65f, 0, 0);
        checkOuterRightOverlap = Physics2D.OverlapBox(outerRight, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkRightOverlap = " + checkRightOverlap);

        Vector3 outerRight2 = transform.position + new Vector3(4f, 0, 0);
        checkOuterRight2Overlap = Physics2D.OverlapBox(outerRight2, new Vector3(3f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));


        Vector3 topRight = transform.position + new Vector3(1.35f, 1.35f, 0);
        checkTopRightOverlap = Physics2D.OverlapBox(topRight, new Vector3(0.25f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkTopRightOverlap = " + checkTopRightOverlap);

        Vector3 topLeft = transform.position + new Vector3(-1.35f, 1.35f, 0);
        checkTopLeftOverlap = Physics2D.OverlapBox(topLeft, new Vector3(0.25f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkTopLeftOverlap = " + checkTopLeftOverlap);

        Vector3 bottomLeft = transform.position + new Vector3(-1.35f, -1.35f, 0);
        checkBottomLeftOverlap = Physics2D.OverlapBox(bottomLeft, new Vector3(0.25f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomLeftOverlap = " + checkBottomLeftOverlap);

        Vector3 bottomRight = transform.position + new Vector3(1.35f, -1.35f, 0);
        checkBottomRightOverlap = Physics2D.OverlapBox(bottomRight, new Vector3(0.25f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomRightOverlap = " + checkBottomRightOverlap);


        Vector3 outerTopRight = transform.position + new Vector3(1.6f, 1.6f, 0);
        checkOuterTopRightOverlap = Physics2D.OverlapBox(outerTopRight, new Vector3(0.5f, 0.5f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkTopRightOverlap = " + checkTopRightOverlap);

        Vector3 outerTopLeft = transform.position + new Vector3(-1.6f, 1.6f, 0);
        checkOuterTopLeftOverlap = Physics2D.OverlapBox(outerTopLeft, new Vector3(0.5f, 0.5f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkTopLeftOverlap = " + checkTopLeftOverlap);

        Vector3 outerBottomLeft = transform.position + new Vector3(-1.6f, -1.6f, 0);
        checkOuterBottomLeftOverlap = Physics2D.OverlapBox(outerBottomLeft, new Vector3(0.5f, 0.5f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomLeftOverlap = " + checkBottomLeftOverlap);

        Vector3 outerBottomRight = transform.position + new Vector3(1.6f, -1.6f, 0);
        checkOuterBottomRightOverlap = Physics2D.OverlapBox(outerBottomRight, new Vector3(0.5f, 0.5f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomRightOverlap = " + checkBottomRightOverlap);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 top = transform.position + new Vector3(0, 0.65f, 0);
        Gizmos.DrawWireCube(top, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.red;
        Vector3 outerTop = transform.position + new Vector3(0, 1.65f, 0);
        Gizmos.DrawWireCube(outerTop, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.red;
        Vector3 outerTop2 = transform.position + new Vector3(0, 4f, 0);
        Gizmos.DrawWireCube(outerTop2, new Vector3(0.9f, 3f, 0));


        Gizmos.color = Color.blue;
        Vector3 bottom = transform.position - new Vector3(0, 0.65f, 0);
        Gizmos.DrawWireCube(bottom, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.blue;
        Vector3 outerBottom = transform.position - new Vector3(0, 1.65f, 0);
        Gizmos.DrawWireCube(outerBottom, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.blue;
        Vector3 outerBottom2 = transform.position - new Vector3(0, 4f, 0);
        Gizmos.DrawWireCube(outerBottom2, new Vector3(0.9f, 3f, 0));


        Gizmos.color = Color.yellow;
        Vector3 left = transform.position - new Vector3(0.65f, 0, 0);
        Gizmos.DrawWireCube(left, new Vector3(0.25f, 0.9f, 0));

        Gizmos.color = Color.yellow;
        Vector3 outerLeft = transform.position - new Vector3(1.65f, 0, 0);
        Gizmos.DrawWireCube(outerLeft, new Vector3(0.25f, 0.9f, 0));

        Gizmos.color = Color.yellow;
        Vector3 outerLeft2 = transform.position - new Vector3(4f, 0, 0);
        Gizmos.DrawWireCube(outerLeft2, new Vector3(3f, 0.9f, 0));


        Gizmos.color = Color.green;
        Vector3 right = transform.position + new Vector3(0.65f, 0, 0);
        Gizmos.DrawWireCube(right, new Vector3(0.25f, 0.9f, 0));

        Gizmos.color = Color.green;
        Vector3 outerRight = transform.position + new Vector3(1.65f, 0, 0);
        Gizmos.DrawWireCube(outerRight, new Vector3(0.25f, 0.9f, 0));

        Gizmos.color = Color.green;
        Vector3 outerRight2 = transform.position + new Vector3(4f, 0, 0);
        Gizmos.DrawWireCube(outerRight2, new Vector3(3f, 0.9f, 0));


        Gizmos.color = Color.white;
        Vector3 topRight = transform.position + new Vector3(0.65f, 0.65f, 0);
        Gizmos.DrawWireCube(topRight, new Vector3(0.25f, 0.25f, 0));

        Gizmos.color = Color.white;
        Vector3 topLeft = transform.position + new Vector3(-0.65f, 0.65f, 0);
        Gizmos.DrawWireCube(topLeft, new Vector3(0.25f, 0.25f, 0));

        Gizmos.color = Color.white;
        Vector3 bottomLeft = transform.position + new Vector3(-0.65f, -0.65f, 0);
        Gizmos.DrawWireCube(bottomLeft, new Vector3(0.25f, 0.25f, 0));

        Gizmos.color = Color.white;
        Vector3 bottomRight = transform.position + new Vector3(0.65f, -0.65f, 0);
        Gizmos.DrawWireCube(bottomRight, new Vector3(0.25f, 0.25f, 0));


        Gizmos.color = Color.white;
        Vector3 outerTopRight = transform.position + new Vector3(1.6f, 1.6f, 0);
        Gizmos.DrawWireCube(outerTopRight, new Vector3(0.5f, 0.5f, 0));

        Gizmos.color = Color.white;
        Vector3 outerTopLeft = transform.position + new Vector3(-1.6f, 1.6f, 0);
        Gizmos.DrawWireCube(outerTopLeft, new Vector3(0.5f, 0.5f, 0));

        Gizmos.color = Color.white;
        Vector3 outerBottomLeft = transform.position + new Vector3(-1.6f, -1.6f, 0);
        Gizmos.DrawWireCube(outerBottomLeft, new Vector3(0.5f, 0.5f, 0));

        Gizmos.color = Color.white;
        Vector3 outerBottomRight = transform.position + new Vector3(1.6f, -1.6f, 0);
        Gizmos.DrawWireCube(outerBottomRight, new Vector3(0.5f, 0.5f, 0));
    }
}
