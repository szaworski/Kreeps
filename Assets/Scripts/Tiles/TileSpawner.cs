﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSpawner : TileTypes
{
    public int shiftAmtXpos;
    public int shiftAmtYpos;
    public int shiftSpawnerXpos;
    public int shiftSpawnerYpos;
    public static int numOfTimesPlaced;

    public int numOfUpRayHits;
    public int numOfDownRayHits;
    public int numOfLeftRayHits;
    public int numOfRightRayHits;

    public bool checkTopOverlap;
    public bool checkBottomOverlap;
    public bool checkLeftOverlap;
    public bool checkRightOverlap;
    public bool checkOuterTopOverlap;
    public bool checkOuterBottomOverlap;
    public bool checkOuterLeftOverlap;
    public bool checkOuterRightOverlap;
    public bool checkCenterOverlap;
    public bool checkTopRightOverlap;
    public bool checkTopLeftOverlap;
    public bool checkBottomLeftOverlap;
    public bool checkBottomRightOverlap;
    public static bool triggerTileCardDestruction;
    public static bool triggerMonsterCardDestruction;

    public static string prependTileName;
    public static string tileName;
    public static string newTileName;
    public static string spawnDirection;
    public static string tileCardSelected;
    public static string monsterCardSelected;

    public string[] curTiles;
    public bool[] validTiles;
    List<string> validTilesList;

    public GameObject card1Obj;
    public GameObject card2Obj;
    public GameObject card3Obj;

    //Tile Type counter values
    public static int numOfForrests;
    public static int numOfGraveyards;
    public static int numOfRivers;
    public static int numOfMountains;
    public static int numOfSwamps;

    void Awake()
    {
        numOfTimesPlaced = 0;
        PlaceStartingTile();
        curTiles = new string[6];
        validTiles = new bool[6];
    }

    void Update()
    {
        GetAndShowTileCards();
        DestroyMonsterCards();
        // SpawnNewTile(); is now called in GetAndShowTileCards() after a selection is made
    }

    public void PlaceStartingTile()
    {
        tileName = "StartingTile";

        //Fetch the starting tile
        GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load("Tiles/StartingTile"));
        GameObject TileHolder = GameObject.Find("TileHolder");

        //Place the starting tile
        GameObject tile = (GameObject)Instantiate(referenceStartTile, TileHolder.transform);
        tile.transform.SetParent(TileHolder.transform);
        tile.transform.position = new Vector2(0, 0);

        //increment numOfTimesPlaced
        numOfTimesPlaced++;

        //Place a waypoint node on the center of the tile
        GameObject waypoint = (GameObject)Instantiate(Resources.Load("Tiles/WaypointNode"), TileHolder.transform);
        waypoint.transform.SetParent(tile.transform);
        waypoint.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);

        //Destroy the temporary reference object
        Destroy(referenceStartTile);

        FindNewSpawnDirection();
        MoveSpawnPos();
    }

    public void SpawnNewTile()
    {
        //Is now called in GetAndShowTileCards() after a selection is made
        GetNewTile();
        tileName = newTileName;
        Debug.Log("Tile Pathway: " + prependTileName + tileName);

        //Find the tile in the resources folder
        GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load(prependTileName + tileName));
        GameObject TileHolder = GameObject.Find("TileHolder");

        //Place the new tile
        GameObject tile = (GameObject)Instantiate(referenceStartTile, TileHolder.transform);
        tile.transform.SetParent(TileHolder.transform);
        tile.transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos);

        //increment numOfTimesPlaced
        numOfTimesPlaced++;

        //Place a waypoint node on the center of the tile
        GameObject waypoint = (GameObject)Instantiate(Resources.Load("Tiles/WaypointNode"), TileHolder.transform);
        waypoint.transform.SetParent(tile.transform);
        waypoint.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);

        //Destroy the temporary reference objects
        Destroy(referenceStartTile);

        //Move the Spawn position
        FindNewSpawnDirection();
        //Debug.Log("Spawn direction: " + spawnDirection);
        MoveSpawnPos();
        //Use raycasts to detect distances from other tiles (Trying to detect dead end. In progress)
        CheckTilesWithRayCasts();
    }

    public void GetNewTile()
    {
        CheckTileOverlap();
        CheckForValidTiles();
        GetListOfValidTiles();
        ChooseRandTileFromList();
    }

    public void MoveSpawnPos()
    {
        if (tileName == "StartingTile")
        {
            transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos + 1);
        }

        else if (spawnDirection == "up")
        {
            transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos + 1);
        }

        else if (spawnDirection == "down")
        {
            transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos - 1);
        }

        else if (spawnDirection == "left")
        {
            transform.position = new Vector2(shiftAmtXpos - 1, shiftAmtYpos);
        }

        else if (spawnDirection == "right")
        {
            transform.position = new Vector2(shiftAmtXpos + 1, shiftAmtYpos);
        }
    }

    public void FindNewSpawnDirection()
    {
        if (tileName == "StartingTile")
        {
            spawnDirection = "up";
        }

        if (tileName == curTiles[0])
        {
            if (!checkTopOverlap)
            {
                spawnDirection = "up";
            }

            else if (!checkBottomOverlap)
            {
                spawnDirection = "down";
            }
        }

        else if (tileName == curTiles[1])
        {
            if (!checkRightOverlap)
            {
                spawnDirection = "right";
            }

            else if (!checkLeftOverlap)
            {
                spawnDirection = "left";
            }
        }

        else if (tileName == curTiles[2])
        {
            if (!checkRightOverlap)
            {
                spawnDirection = "right";
            }

            else if (!checkBottomOverlap)
            {
                spawnDirection = "down";
            }
        }

        else if (tileName == curTiles[3])
        {
            if (!checkLeftOverlap)
            {
                spawnDirection = "left";
            }

            else if (!checkBottomOverlap)
            {
                spawnDirection = "down";
            }
        }

        else if (tileName == curTiles[4])
        {
            if (!checkLeftOverlap)
            {
                spawnDirection = "left";
            }

            else if (!checkTopOverlap)
            {
                spawnDirection = "up";
            }
        }

        else if (tileName == curTiles[5])
        {
            if (!checkRightOverlap)
            {
                spawnDirection = "right";
            }

            else if (!checkTopOverlap)
            {
                spawnDirection = "up";
            }
        }
    }

    public void GetAndShowTileCards()
    {
        //Check if all monsters have been spawned for the wave and that all monsters are dead, then prompt for card selection
        if (MonsterManager.AllMonstersAreSpawned && GameObject.Find("TileManager").transform.childCount == 0) //Input.GetKeyDown(KeyCode.Space)
        {
            MonsterManager.AllMonstersAreSpawned = false;

            var rand = new System.Random();
            List<string> currentCardList = null;
            string card1 = null;
            string card2 = null;
            string card3 = null;

            //Get the correct Tile card list based on the number of waves finished. (Copy the list from TileCards.cs)
            if (numOfTimesPlaced < 20)
            {
                currentCardList = tier1TileCards.ToList();
                //Debug.Log("Card List: " + currentCardList[0] + " " + currentCardList[1] + " " + currentCardList[2] + " " + currentCardList[3] + " " + currentCardList[4]);
            }

            //Create a list of 3 unique random cards from the current card list
            for (int i = 0; i < 3; i++)
            {
                //Fetch a random element from the card list
                int index = rand.Next(currentCardList.Count);
                string selectedCard = currentCardList[index];

                //Remove the selected card from the list so we dont repeat any cards
                currentCardList.RemoveAt(index);

                //Set each selected card to its correlating slot
                switch (i)
                {
                    case 0:
                        card1 = selectedCard;
                        Debug.Log("Card 1: " + card1);
                        break;
                    case 1:
                        card2 = selectedCard;
                        Debug.Log("Card 2: " + card2);
                        break;
                    case 2:
                        card3 = selectedCard;
                        Debug.Log("Card 3: " + card3);
                        break;
                }
            }

            //Instantiate the Tile Cards
            GameObject cardSlot1 = GameObject.Find("TileCardSlot1");
            GameObject cardSlot2 = GameObject.Find("TileCardSlot2");
            GameObject cardSlot3 = GameObject.Find("TileCardSlot3");

            card1Obj = (GameObject)Instantiate(Resources.Load("UI/TileCards/" + card1), cardSlot1.transform);
            card2Obj = (GameObject)Instantiate(Resources.Load("UI/TileCards/" + card2), cardSlot2.transform);
            card3Obj = (GameObject)Instantiate(Resources.Load("UI/TileCards/" + card3), cardSlot3.transform);

            card1Obj.transform.position = cardSlot1.transform.position;
            card2Obj.transform.position = cardSlot2.transform.position;
            card3Obj.transform.position = cardSlot3.transform.position;
        }

        //Destory all tile card game objects after a selection is made. See Card.cs
        if (triggerTileCardDestruction)
        {
            Destroy(card1Obj.gameObject);
            Destroy(card2Obj.gameObject);
            Destroy(card3Obj.gameObject);
            //Spawn the new tile
            SpawnNewTile();
            //Reset this bool for next card selection later
            triggerTileCardDestruction = false;
            //Call GetAndShowMonsterCards() to spawn the Monster card options
            GetAndShowMonsterCards();
        }
    }

    public void GetAndShowMonsterCards()
    {
        var rand = new System.Random();
        List<string> currentCardList = null;
        string card1 = null;
        string card2 = null;

        switch (tileCardSelected)
        {
            case "Forrest":
                currentCardList = forrestMonsterCards.ToList();
                break;

            case "Graveyard":
                currentCardList = graveyardMonsterCards.ToList();
                break;

            case "River":
                currentCardList = riverMonsterCards.ToList();
                break;

            case "Mountain":
                currentCardList = mountainMonsterCards.ToList();
                break;

            case "Swamp":
                currentCardList = swampMonsterCards.ToList();
                break;
        }

        //Create a list of 2 unique random cards from the current card list
        for (int i = 0; i < 2; i++)
        {
            //Fetch a random element from the card list
            int index = rand.Next(currentCardList.Count);
            string selectedCard = currentCardList[index];

            //Remove the selected card from the list so we dont repeat any cards
            currentCardList.RemoveAt(index);

            //Set each selected card to its correlating slot
            switch (i)
            {
                case 0:
                    card1 = selectedCard;
                    Debug.Log("Monster Card 1: " + card1);
                    break;
                case 1:
                    card2 = selectedCard;
                    Debug.Log("Monster Card 2: " + card2);
                    break;
            }
        }

        //Instantiate the Monster Cards
        GameObject cardSlot4 = GameObject.Find("TileCardSlot4");
        GameObject cardSlot5 = GameObject.Find("TileCardSlot5");

        card1Obj = (GameObject)Instantiate(Resources.Load("UI/MonsterCards/" + tileCardSelected + "/" + card1), cardSlot4.transform);
        card2Obj = (GameObject)Instantiate(Resources.Load("UI/MonsterCards/" + tileCardSelected + "/" + card2), cardSlot5.transform);

        card1Obj.transform.position = cardSlot4.transform.position;
        card2Obj.transform.position = cardSlot5.transform.position;
    }

    public void DestroyMonsterCards()
    {
        //Destory all monster card game objects after a selection is made. See Card.cs
        if (triggerMonsterCardDestruction)
        {
            Destroy(card1Obj.gameObject);
            Destroy(card2Obj.gameObject);
            //Reset this bool for next card selection later
            triggerMonsterCardDestruction = false;
            PlayerHud.showStartWaveInstructions = true;
            Debug.Log("Monster Cards Destoryed");
        }
    }

    public void ChooseRandTileFromList()
    {
        var rand = new System.Random();
        int index = rand.Next(validTilesList.Count);
        Debug.Log("The randomly Chosen tile is: " + validTilesList[index]);
        newTileName = validTilesList[index];
    }

    public void GetListOfValidTiles()
    {
        validTilesList = new List<string>();

        //Check for which tile card was selected and set the "curTiles" array accordingly 
        switch (tileCardSelected)
        {
            case "Forrest":
                curTiles = forrestTiles;
                prependTileName = "Tiles/ForrestTiles/";
                numOfForrests += 1;
                break;

            case "Graveyard":
                curTiles = graveyardTiles;
                prependTileName = "Tiles/GraveyardTiles/";
                numOfGraveyards += 1;
                break;

            case "River":
                curTiles = riverTiles;
                prependTileName = "Tiles/RiverTiles/";
                numOfRivers += 1;
                break;

            case "Mountain":
                curTiles = mountainTiles;
                prependTileName = "Tiles/MountainTiles/";
                numOfMountains += 1;
                break;

            case "Swamp":
                curTiles = swampTiles;
                prependTileName = "Tiles/SwampTiles/";
                numOfSwamps += 1;
                break;
        }

        //curTiles = forrestTiles;
        //Debug.Log("Tile 1 bool: " + validTiles[0]);
        //Debug.Log("Tile 2 bool: " + validTiles[1]);
        //Debug.Log("Tile 3 bool: " + validTiles[2]);
        //Debug.Log("Tile 4 bool: " + validTiles[3]);
        //Debug.Log("Tile 5 bool: " + validTiles[4]);
        //Debug.Log("Tile 6 bool: " + validTiles[5]);

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

        //Debug.Log("Current tile name: " + tileName);

        if (tileName == "StartingTile")
        {
            validTiles[0] = true;
            validTiles[2] = true;
            validTiles[3] = true;
            shiftAmtYpos += 1;
        }

        else if (tileName == curTiles[0])
        {
            if (checkBottomOverlap && spawnDirection == "up")
            {
                CheckTopOverlaps();
                shiftAmtYpos += 1;
            }

            else if (checkTopOverlap && spawnDirection == "down")
            {
                CheckBottomOverlaps();
                shiftAmtYpos += -1;
            }
        }

        else if (tileName == curTiles[1])
        {
            if (checkLeftOverlap && spawnDirection == "right")
            {
                CheckRightOverlaps();
                shiftAmtXpos += 1;
            }

            else if (checkRightOverlap && spawnDirection == "left")
            {
                CheckLeftOverlaps();
                shiftAmtXpos += -1;
            }
        }

        else if (tileName == curTiles[2])
        {
            if (checkLeftOverlap && spawnDirection == "right")
            {
                CheckRightOverlaps();
                shiftAmtXpos += 1;
            }

            else if (checkTopOverlap && spawnDirection == "down")
            {
                CheckBottomOverlaps();
                shiftAmtYpos += -1;
            }
        }

        else if (tileName == curTiles[3])
        {
            if (checkRightOverlap && spawnDirection == "left")
            {
                CheckLeftOverlaps();
                shiftAmtXpos += -1;
            }

            else if (checkTopOverlap && spawnDirection == "down")
            {
                CheckBottomOverlaps();
                shiftAmtYpos += -1;
            }
        }

        else if (tileName == curTiles[4])
        {
            if (checkRightOverlap && spawnDirection == "left")
            {
                CheckLeftOverlaps();
                shiftAmtXpos += -1;
            }

            else if (checkBottomOverlap && spawnDirection == "up")
            {
                CheckTopOverlaps();
                shiftAmtYpos += 1;
            }
        }

        else if (tileName == curTiles[5])
        {
            if (checkBottomOverlap && spawnDirection == "up")
            {
                CheckTopOverlaps();
                shiftAmtYpos += 1;
            }

            else if (checkLeftOverlap && spawnDirection == "right")
            {
                CheckRightOverlaps();
                shiftAmtXpos += 1;
            }
        }
    }

    public void CheckTilesWithRayCasts()
    {
        //Reset Raycast hit num values
        numOfUpRayHits = 0;
        numOfDownRayHits = 0;
        numOfLeftRayHits = 0;
        numOfRightRayHits = 0;

        if (spawnDirection == "up" || spawnDirection == "down")
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 25f);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 25f);

            if (hitLeft)
            {
                if (hitLeft.collider.isTrigger)
                {
                    numOfLeftRayHits++;
                    //Debug.Log("Found object with leftward cast - distance: " + hitLeft.distance);
                    RaycastHit2D hitLeftUp = Physics2D.Raycast(hitLeft.point * 0.9f, Vector3.up, 12.5f);
                    RaycastHit2D hitLeftDown = Physics2D.Raycast(hitLeft.point * 0.9f, Vector3.down, 12.5f);

                    if (hitLeftUp)
                    {
                        if (hitLeftUp.collider.isTrigger)
                        {
                            numOfLeftRayHits++;
                            //Debug.Log("Found object with leftward/Up cast - distance: " + hitLeftUp.distance);
                        }
                    }

                    if (hitLeftDown)
                    {
                        if (hitLeftDown.collider.isTrigger)
                        {
                            numOfLeftRayHits++;
                            //Debug.Log("Found object with leftward/Down cast - distance: " + hitLeftDown.distance);
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("Nothing found with leftward raycast");
            }

            if (hitRight)
            {
                if (hitRight.collider.isTrigger)
                {
                    numOfRightRayHits++;
                    //Debug.Log("Found object with rightward cast - distance: " + hitRight.distance);
                    RaycastHit2D hitRightUp = Physics2D.Raycast(hitRight.point * 0.9f, Vector3.up, 12.5f);
                    RaycastHit2D hitRightDown = Physics2D.Raycast(hitRight.point * 0.9f, Vector3.down, 12.5f);

                    if (hitRightUp)
                    {
                        if (hitRightUp.collider.isTrigger)
                        {
                            numOfRightRayHits++;
                            //Debug.Log("Found object with rightward/Up cast - distance: " + hitRightUp.distance);
                        }
                    }

                    if (hitRightDown)
                    {
                        if (hitRightDown.collider.isTrigger)
                        {
                            numOfRightRayHits++;
                            //Debug.Log("Found object with rightward/Down cast - distance: " + hitRightDown.distance);
                        }
                    }
                }
                else
                {
                    //Debug.Log("Nothing found with rightward raycast");
                }
            }
        }

        else if (spawnDirection == "left" || spawnDirection == "right")
        {
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector3.up, 25f);
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector3.down, 25f);

            if (hitUp)
            {
                if (hitUp.collider.isTrigger)
                {
                    numOfUpRayHits++;
                    //Debug.Log("Found object with upward cast - distance: " + hitUp.distance);
                    RaycastHit2D hitUpRight = Physics2D.Raycast(hitUp.point * 0.9f, Vector3.right, 12.5f);
                    RaycastHit2D hitUpLeft = Physics2D.Raycast(hitUp.point * 0.9f, Vector3.left, 12.5f);

                    if (hitUpRight)
                    {
                        if (hitUpRight.collider.isTrigger)
                        {
                            numOfUpRayHits++;
                            //Debug.Log("Found object with upward/Right cast - distance: " + hitUpRight.distance);
                        }
                    }

                    if (hitUpLeft)
                    {
                        if (hitUpLeft.collider.isTrigger)
                        {
                            numOfUpRayHits++;
                            //Debug.Log("Found object with upward/Left cast - distance: " + hitUpLeft.distance);
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("Nothing found with upward raycast");
            }

            if (hitDown)
            {
                if (hitDown.collider.isTrigger)
                {
                    numOfDownRayHits++;
                    //Debug.Log("Found object with downward cast - distance: " + hitDown.distance);
                    RaycastHit2D hitDownRight = Physics2D.Raycast(hitDown.point * 0.9f, Vector3.right, 12.5f);
                    RaycastHit2D hitDownLeft = Physics2D.Raycast(hitDown.point * 0.9f, Vector3.left, 12.5f);

                    if (hitDownRight)
                    {
                        if (hitDownRight.collider.isTrigger)
                        {
                            numOfDownRayHits++;
                            //Debug.Log("Found object with downward/Right cast - distance: " + hitDownRight.distance);
                        }
                    }

                    if (hitDownLeft)
                    {
                        if (hitDownLeft.collider.isTrigger)
                        {
                            numOfDownRayHits++;
                            //Debug.Log("Found object with downward/Left cast - distance: " + hitDownLeft.distance);
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("Nothing found with downward raycast");
            }
        }
    }

    public void CheckTopOverlaps()
    {
        if (!checkTopOverlap && !checkOuterTopOverlap)
        {
            validTiles[0] = true;
        }

        if (!checkRightOverlap && numOfLeftRayHits > numOfRightRayHits)
        {
            validTiles[2] = true;
        }

        else if (!checkLeftOverlap && numOfRightRayHits > numOfLeftRayHits)
        {
            validTiles[3] = true;
        }

        else
        {
            //Debug.Log("Neither raycast num was greater");

            if (!checkRightOverlap)
            {
                validTiles[2] = true;
            }

            if (!checkLeftOverlap)
            {
                validTiles[3] = true;
            }
        }
    }

    public void CheckBottomOverlaps()
    {
        if (!checkBottomOverlap && !checkOuterBottomOverlap)
        {
            validTiles[0] = true;
        }

        if (!checkLeftOverlap && numOfRightRayHits > numOfLeftRayHits)
        {
            validTiles[4] = true;
        }

        else if (!checkRightOverlap && numOfLeftRayHits > numOfRightRayHits)
        {
            validTiles[5] = true;
        }

        else
        {
            //Debug.Log("Neither raycast num was greater");

            if (!checkRightOverlap)
            {
                validTiles[5] = true;
            }

            if (!checkLeftOverlap)
            {
                validTiles[4] = true;
            }
        }
    }

    public void CheckRightOverlaps()
    {
        if (!checkRightOverlap && !checkOuterRightOverlap)
        {
            validTiles[1] = true;
        }

        if (!checkBottomOverlap && numOfUpRayHits > numOfDownRayHits)
        {
            validTiles[3] = true;
        }

        else if (!checkTopOverlap && numOfDownRayHits > numOfUpRayHits)
        {
            validTiles[4] = true;
        }

        else
        {
            //Debug.Log("Neither raycast num was greater");

            if (!checkTopOverlap)
            {
                validTiles[4] = true;
            }

            if (!checkBottomOverlap)
            {
                validTiles[3] = true;
            }
        }
    }

    public void CheckLeftOverlaps()
    {
        if (!checkLeftOverlap && !checkOuterLeftOverlap)
        {
            validTiles[1] = true;
        }

        if (!checkTopOverlap && numOfDownRayHits > numOfUpRayHits)
        {
            validTiles[5] = true;
        }

        else if (!checkBottomOverlap && numOfUpRayHits > numOfDownRayHits)
        {
            validTiles[2] = true;
        }

        else
        {
            //Debug.Log("Neither raycast num was greater");

            if (!checkTopOverlap)
            {
                validTiles[5] = true;
            }

            if (!checkBottomOverlap)
            {
                validTiles[2] = true;
            }
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

        Vector3 bottom = transform.position - new Vector3(0, 0.65f, 0);
        checkBottomOverlap = Physics2D.OverlapBox(bottom, new Vector3(0.9f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomOverlap = " + checkBottomOverlap);

        Vector3 outerBottom = transform.position - new Vector3(0, 1.65f, 0);
        checkOuterBottomOverlap = Physics2D.OverlapBox(outerBottom, new Vector3(0.9f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkBottomOverlap = " + checkBottomOverlap);

        Vector3 left = transform.position - new Vector3(0.65f, 0, 0);
        checkLeftOverlap = Physics2D.OverlapBox(left, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkLeftOverlap = " + checkLeftOverlap);

        Vector3 outerLeft = transform.position - new Vector3(1.65f, 0, 0);
        checkOuterLeftOverlap = Physics2D.OverlapBox(outerLeft, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkLeftOverlap = " + checkLeftOverlap);

        Vector3 right = transform.position + new Vector3(0.65f, 0, 0);
        checkRightOverlap = Physics2D.OverlapBox(right, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkRightOverlap = " + checkRightOverlap);

        Vector3 outerRight = transform.position + new Vector3(1.65f, 0, 0);
        checkOuterRightOverlap = Physics2D.OverlapBox(outerRight, new Vector3(0.25f, 0.9f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkRightOverlap = " + checkRightOverlap);


        Vector3 center = transform.position + new Vector3(0, 0, 0);
        checkCenterOverlap = Physics2D.OverlapBox(center, new Vector3(0.25f, 0.25f, 0), 0f, LayerMask.GetMask("GroundTile"));
        //Debug.Log("checkTopRightOverlap = " + checkTopRightOverlap);

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
    }

    public void OnDrawGizmos()
    {
        // Commenting out gizmos for now. Turn these back on to see boxes used for tile detection
        /*
        Gizmos.color = Color.red;
        Vector3 top = transform.position + new Vector3(0, 0.65f, 0);
        Gizmos.DrawWireCube(top, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.red;
        Vector3 outerTop = transform.position + new Vector3(0, 1.65f, 0);
        Gizmos.DrawWireCube(outerTop, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.blue;
        Vector3 bottom = transform.position - new Vector3(0, 0.65f, 0);
        Gizmos.DrawWireCube(bottom, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.blue;
        Vector3 outerBottom = transform.position - new Vector3(0, 1.65f, 0);
        Gizmos.DrawWireCube(outerBottom, new Vector3(0.9f, 0.25f, 0));

        Gizmos.color = Color.yellow;
        Vector3 left = transform.position - new Vector3(0.65f, 0, 0);
        Gizmos.DrawWireCube(left, new Vector3(0.25f, 0.9f, 0));

        Gizmos.color = Color.yellow;
        Vector3 outerLeft = transform.position - new Vector3(1.65f, 0, 0);
        Gizmos.DrawWireCube(outerLeft, new Vector3(0.25f, 0.9f, 0));

        Gizmos.color = Color.green;
        Vector3 right = transform.position + new Vector3(0.65f, 0, 0);
        Gizmos.DrawWireCube(right, new Vector3(0.25f, 0.9f, 0));

        Gizmos.color = Color.green;
        Vector3 outerRight = transform.position + new Vector3(1.65f, 0, 0);
        Gizmos.DrawWireCube(outerRight, new Vector3(0.25f, 0.9f, 0));


        Gizmos.color = Color.white;
        Vector3 center = transform.position + new Vector3(0, 0, 0);
        Gizmos.DrawWireCube(center, new Vector3(0.25f, 0.25f, 0));

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
        */
    }
}
