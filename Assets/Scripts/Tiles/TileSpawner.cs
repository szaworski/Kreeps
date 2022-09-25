﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TileSpawner : CardsLists
{
    private int shiftAmtXpos;
    private int shiftAmtYpos;
    private int numOfUpRayHits;
    private int numOfDownRayHits;
    private int numOfLeftRayHits;
    private int numOfRightRayHits;
    private bool checkTopOverlap;
    private bool checkBottomOverlap;
    private bool checkLeftOverlap;
    private bool checkRightOverlap;
    private bool checkOuterTopOverlap;
    private bool checkOuterBottomOverlap;
    private bool checkOuterLeftOverlap;
    private bool checkOuterRightOverlap;
    private bool[] validTiles;
    private string[] curTiles;
    private string newTileName;
    private string prependTileName;
    private string spawnDirection;
    private List<string> validTilesList;
    private GameObject card1Obj;
    private GameObject card2Obj;
    private GameObject card3Obj;
    private GameObject card4Obj;
    private GameObject card5Obj;
    [SerializeField] private TMP_Text locationSelectText;
    [SerializeField] private TMP_Text monsterSelectText;
    [SerializeField] private TMP_Text shopSelectText;

    void Awake()
    {
        curTiles = new string[6];
        validTiles = new bool[6];
        PlaceStartingTile();
    }

    void Update()
    {
        GetAndShowTileCards();
        DestroyMonsterCards();
        DestroyShopCards();
    }

    public void PlaceStartingTile()
    {
        //Fetch the starting tile
        GlobalVars.tileName = "StartingTile";
        GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load("Tiles/StartingTile"));
        GameObject TileHolder = GameObject.Find("TileHolder");

        //Place the starting tile
        GameObject tile = (GameObject)Instantiate(referenceStartTile, TileHolder.transform);
        tile.transform.SetParent(TileHolder.transform);
        tile.transform.position = new Vector2(0, 0);
        GlobalVars.tileCounters["numOfTimesPlaced"]++;

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
        //Get the new tile
        CheckTileOverlap();
        CheckForValidTiles();
        GetListOfValidTiles();
        ChooseRandTileFromList();
        GlobalVars.tileName = newTileName;
        Debug.Log("Tile Pathway: " + prependTileName + GlobalVars.tileName);

        //Find the tile in the resources folder
        GameObject referenceStartTile = (GameObject)Instantiate(Resources.Load(prependTileName + GlobalVars.tileName));
        GameObject TileHolder = GameObject.Find("TileHolder");

        //Place the new tile
        GameObject tile = (GameObject)Instantiate(referenceStartTile, TileHolder.transform);
        tile.transform.SetParent(TileHolder.transform);
        tile.transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos);

        //increment numOfTimesPlaced
        GlobalVars.tileCounters["numOfTimesPlaced"]++;

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

    public void MoveSpawnPos()
    {
        switch (spawnDirection)
        {
            case "up":
                transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos + 1);
                break;
            case "down":
                transform.position = new Vector2(shiftAmtXpos, shiftAmtYpos - 1);
                break;
            case "left":
                transform.position = new Vector2(shiftAmtXpos - 1, shiftAmtYpos);
                break;
            case "right":
                transform.position = new Vector2(shiftAmtXpos + 1, shiftAmtYpos);
                break;
        }
    }

    public void FindNewSpawnDirection()
    {
        switch (GlobalVars.tileName)
        {
            case "StartingTile":
                spawnDirection = "up";
                break;
            case string value when value == curTiles[0]:
                if (!checkTopOverlap)
                    spawnDirection = "up";
                else if (!checkBottomOverlap)
                    spawnDirection = "down";
                break;
            case string value when value == curTiles[1]:
                if (!checkRightOverlap)
                    spawnDirection = "right";
                else if (!checkLeftOverlap)
                    spawnDirection = "left";
                break;
            case string value when value == curTiles[2]:
                if (!checkRightOverlap)
                    spawnDirection = "right";
                else if (!checkBottomOverlap)
                    spawnDirection = "down";
                break;
            case string value when value == curTiles[3]:
                if (!checkLeftOverlap)
                    spawnDirection = "left";
                else if (!checkBottomOverlap)
                    spawnDirection = "down";
                break;
            case string value when value == curTiles[4]:
                if (!checkLeftOverlap)
                    spawnDirection = "left";
                else if (!checkTopOverlap)
                    spawnDirection = "up";
                break;
            case string value when value == curTiles[5]:
                if (!checkRightOverlap)
                    spawnDirection = "right";
                else if (!checkTopOverlap)
                    spawnDirection = "up";
                break;
        }
    }

    public void GetAndShowTileCards()
    {
        //Check if all monsters have been spawned for the wave and that all monsters are dead, then prompt for card selection
        if (GlobalVars.allMonstersAreSpawned && GameObject.Find("TileManager").transform.childCount == 0 && GlobalVars.playerHealth > 0) //Input.GetKeyDown(KeyCode.Space)
        {
            StartCoroutine(FadeMusic.StartFade(GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>(), 1f, GlobalVars.musicVolume * 0.5f));
            GlobalVars.waveEnded = true;
            GlobalVars.allMonstersAreSpawned = false;

            List<string> currentCardList = null;
            string card1 = null;
            string card2 = null;
            string card3 = null;

            //Get the correct Tile card list based on the number of waves finished. (Copy the list from TileCards.cs)
            if (GlobalVars.tileCounters["numOfTimesPlaced"] < 10)
            {
                currentCardList = tier1TileCards.ToList();
            }

            else if (GlobalVars.tileCounters["numOfTimesPlaced"] >= 10 && GlobalVars.tileCounters["numOfTimesPlaced"] < 20)
            {
                GlobalVars.currTier = "Tier2";
                GlobalVars.currTierNum = 2;
                currentCardList = tier2TileCards.ToList();
                GlobalVars.kreepSpawnRate = 0.45f;
            }

            else if (GlobalVars.tileCounters["numOfTimesPlaced"] >= 20 && GlobalVars.tileCounters["numOfTimesPlaced"] < 30)
            {
                GlobalVars.currTier = "Tier3";
                GlobalVars.currTierNum = 3;
                currentCardList = tier3TileCards.ToList();
                GlobalVars.kreepSpawnRate = 0.4f;

                if (GlobalVars.tileCounters["numOfTimesPlaced"] == 20)
                {
                    ChangeSong("Song2");
                }
            }

            else if (GlobalVars.tileCounters["numOfTimesPlaced"] >= 30 && GlobalVars.tileCounters["numOfTimesPlaced"] < 40)
            {
                GlobalVars.currTier = "Tier4";
                GlobalVars.currTierNum = 4;
                currentCardList = tier4TileCards.ToList();
            }

            else if (GlobalVars.tileCounters["numOfTimesPlaced"] >= 40 && GlobalVars.tileCounters["numOfTimesPlaced"] < 50)
            {
                GlobalVars.currTier = "Tier5";
                GlobalVars.currTierNum = 5;
                currentCardList = tier5TileCards.ToList();

                if (GlobalVars.tileCounters["numOfTimesPlaced"] == 40)
                {
                    ChangeSong("Song3");
                }
            }

            //Create a list of 3 unique random cards from the current card list
            for (int i = 0; i < 3; i++)
            {
                //Fetch a random element from the card list
                int index = Random.Range(0, currentCardList.Count);
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

            card1Obj = (GameObject)Instantiate(Resources.Load("UI/TileCards/" + GlobalVars.currTier + "/" + card1), cardSlot1.transform);
            card2Obj = (GameObject)Instantiate(Resources.Load("UI/TileCards/" + GlobalVars.currTier + "/" + card2), cardSlot2.transform);
            card3Obj = (GameObject)Instantiate(Resources.Load("UI/TileCards/" + GlobalVars.currTier + "/" + card3), cardSlot3.transform);

            card1Obj.transform.position = cardSlot1.transform.position;
            card2Obj.transform.position = cardSlot2.transform.position;
            card3Obj.transform.position = cardSlot3.transform.position;
            locationSelectText.enabled = true;
        }

        //Destory all tile card game objects after a selection is made. See Card.cs
        if (GlobalVars.triggerTileCardDestruction)
        {
            Destroy(card1Obj.gameObject);
            Destroy(card2Obj.gameObject);
            Destroy(card3Obj.gameObject);
            //Spawn the new tile
            SpawnNewTile();
            //Reset this bool for next card selection later
            GlobalVars.triggerTileCardDestruction = false;
            locationSelectText.enabled = false;
            //Call GetAndShowMonsterCards() to spawn the Monster card options
            GetAndShowMonsterCards();
        }
    }

    public void GetAndShowMonsterCards()
    {
        List<string> currentCardList = null;
        string card1 = null;
        string card2 = null;

        switch (GlobalVars.tileCardSelected)
        {
            //Tier 1
            case "Forest":
                currentCardList = forestMonsterCards.ToList();
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

            //Tier 2
            case "Desert":
                currentCardList = desertMonsterCards.ToList();
                break;
            case "Thicket":
                currentCardList = thicketMonsterCards.ToList();
                break;
            case "Tundra":
                currentCardList = tundraMonsterCards.ToList();
                break;
            case "Cavern":
                currentCardList = cavernMonsterCards.ToList();
                break;
            case "Settlement":
                currentCardList = settlementMonsterCards.ToList();
                break;
            case "Seashore":
                currentCardList = seashoreMonsterCards.ToList();
                break;

            //Tier 3
            case "CanyonCrossing":
                currentCardList = canyonCrossingMonsterCards.ToList();
                break;
            case "CrimsonPlain":
                currentCardList = crimsonPlainMonsterCards.ToList();
                break;
            case "Crypt":
                currentCardList = cryptMonsterCards.ToList();
                break;
            case "EmeraldCave":
                currentCardList = emeraldCaveMonsterCards.ToList();
                break;
            case "Marsh":
                currentCardList = marshMonsterCards.ToList();
                break;
            case "Sewer":
                currentCardList = sewerMonsterCards.ToList();
                break;

            //Tier 4
            case "CrystalCave":
                currentCardList = crystalCaveMonsterCards.ToList();
                break;
            case "FrozenPassage":
                currentCardList = frozenPassageMonsterCards.ToList();
                break;
            case "InfernalWoods":
                currentCardList = infernalWoodsMonsterCards.ToList();
                break;
            case "SacredGrounds":
                currentCardList = sacredGroundsMonsterCards.ToList();
                break;
            case "TaintedCanal":
                currentCardList = taintedCanalMonsterCards.ToList();
                break;
            case "VolcanicRavine":
                currentCardList = volcanicRavineMonsterCards.ToList();
                break;

            //Tier 5
            case "AncestralForest":
                currentCardList = ancestralForestMonsterCards.ToList();
                break;
            case "CelestialPlane":
                currentCardList = celestialPlaneMonsterCards.ToList();
                break;
            case "CorruptedIsle":
                currentCardList = corruptedIsleMonsterCards.ToList();
                break;
            case "MysticMountain":
                currentCardList = mysticMountainMonsterCards.ToList();
                break;
            case "OceanAbyss":
                currentCardList = oceanAbyssMonsterCards.ToList();
                break;
            case "Underworld":
                currentCardList = underworldMonsterCards.ToList();
                break;
        }

        //Create a list of 2 unique random cards from the current card list
        for (int i = 0; i < 2; i++)
        {
            //Fetch a random element from the card list
            int index = Random.Range(0, currentCardList.Count);
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

        card1Obj = (GameObject)Instantiate(Resources.Load("UI/MonsterCards/" + GlobalVars.currTier + "/" + GlobalVars.tileCardSelected + "/" + card1), cardSlot4.transform);
        card2Obj = (GameObject)Instantiate(Resources.Load("UI/MonsterCards/" + GlobalVars.currTier + "/" + GlobalVars.tileCardSelected + "/" + card2), cardSlot5.transform);

        card1Obj.transform.position = cardSlot4.transform.position;
        card2Obj.transform.position = cardSlot5.transform.position;
        monsterSelectText.enabled = true;
    }

    public void DestroyMonsterCards()
    {
        //Destory all monster card game objects after a selection is made. See Card.cs
        if (GlobalVars.triggerMonsterCardDestruction)
        {
            Destroy(card1Obj.gameObject);
            Destroy(card2Obj.gameObject);
            //Reset this bool for next card selection later
            GlobalVars.triggerMonsterCardDestruction = false;
            monsterSelectText.enabled = false;
            Debug.Log("Monster Cards Destoryed");

            if ((GlobalVars.tileCounters["numOfTimesPlaced"] - 1) % 1 == 0)
            {
                GetAndShowShopCards();
            }

            else
            {
                GlobalVars.showStartWaveInstructions = true;
                Resources.UnloadUnusedAssets();
            }
        }
    }

    public void GetAndShowShopCards()
    {
        List<string> weaponCardList = null;
        List<string> powerUpCardList = powerUpCards.ToList();
        string card1 = null;
        string card2 = null;
        string card3 = null;
        string card4 = null;
        string card5 = "SkipCard";

        switch (GlobalVars.bonusStats["EquipmentLvl"])
        {
            case 1:
                weaponCardList = tier1WeaponCards.ToList();
                break;
            case 2:
                weaponCardList = tier2WeaponCards.ToList();
                break;
            case 3:
                weaponCardList = tier3WeaponCards.ToList();
                break;
            case 4:
                weaponCardList = tier4WeaponCards.ToList();
                break;
            case 5:
                weaponCardList = tier5WeaponCards.ToList();
                break;
        }

        //Create a list of 1 unique random weapon cards from the current weapon card list
        for (int i = 0; i < 1; i++)
        {
            //Fetch a random element from the card list
            int index = Random.Range(0, weaponCardList.Count);
            string selectedCard = weaponCardList[index];

            if (selectedCard == GlobalVars.currentWeapon)
            {
                //Remove the already held weapon from the list
                weaponCardList.RemoveAt(index);
                //Fetch a random element from the card list (After removing the already held weapon from the list)
                index = Random.Range(0, weaponCardList.Count);
                selectedCard = weaponCardList[index];
            }
            else
            {
                //Remove the selected card from the list so we dont repeat any cards
                weaponCardList.RemoveAt(index);
            }

            //Set each selected card to its correlating slot
            switch (i)
            {
                case 0:
                    card1 = selectedCard;
                    //Debug.Log("Weapon Card 1: " + card1);
                    break;
            }
        }

        //Create a list of 3 unique random powerup cards from the powerup card list
        for (int i = 0; i < 3; i++)
        {
            //Check for and remove invalid powerups from the list
            foreach (string item in powerUpCardList.ToList())
            {
                if (item.Contains("Up") && GlobalVars.bonusExtraStats[item + "Lvl"] == 4)
                {
                    powerUpCardList.Remove(item);
                }
            }

            //Fetch a random element from the card list
            int index = Random.Range(0, powerUpCardList.Count);
            string selectedCard = powerUpCardList[index];

            //Remove the selected card from the list so we dont repeat any cards
            powerUpCardList.RemoveAt(index);

            //Set each selected card to its correlating slot
            switch (i)
            {
                case 0:
                    if (!selectedCard.Contains("Up"))
                    {
                        card2 = selectedCard + GlobalVars.bonusStats[selectedCard + "Lvl"];
                    }
                    else
                    {
                        card2 = selectedCard + GlobalVars.bonusExtraStats[selectedCard + "Lvl"];
                    }
                    break;
                case 1:
                    if (!selectedCard.Contains("Up"))
                    {
                        card3 = selectedCard + GlobalVars.bonusStats[selectedCard + "Lvl"];
                    }
                    else
                    {
                        card3 = selectedCard + GlobalVars.bonusExtraStats[selectedCard + "Lvl"];
                    }
                    break;
                case 2:
                    if (!selectedCard.Contains("Up"))
                    {
                        card4 = selectedCard + GlobalVars.bonusStats[selectedCard + "Lvl"];
                    }
                    else
                    {
                        card4 = selectedCard + GlobalVars.bonusExtraStats[selectedCard + "Lvl"];
                    }
                    break;
            }
        }

        //Instantiate the Shop Cards
        GameObject cardSlot1 = GameObject.Find("ShopCardSlot1");
        GameObject cardSlot2 = GameObject.Find("ShopCardSlot2");
        GameObject cardSlot3 = GameObject.Find("ShopCardSlot3");
        GameObject cardSlot4 = GameObject.Find("ShopCardSlot4");
        GameObject cardSlot5 = GameObject.Find("ShopCardSlot5");

        card1Obj = (GameObject)Instantiate(Resources.Load("UI/WeaponCards/" + "Tier" + GlobalVars.bonusStats["EquipmentLvl"] + "/" + card1), cardSlot1.transform);
        card2Obj = (GameObject)Instantiate(Resources.Load("UI/PowerUpCards/" + card2), cardSlot2.transform);
        card3Obj = (GameObject)Instantiate(Resources.Load("UI/PowerUpCards/" + card3), cardSlot3.transform);
        card4Obj = (GameObject)Instantiate(Resources.Load("UI/PowerUpCards/" + card4), cardSlot4.transform);
        card5Obj = (GameObject)Instantiate(Resources.Load("UI/PowerUpCards/" + card5), cardSlot5.transform);

        card1Obj.transform.position = cardSlot1.transform.position;
        card2Obj.transform.position = cardSlot2.transform.position;
        card3Obj.transform.position = cardSlot3.transform.position;
        card4Obj.transform.position = cardSlot4.transform.position;
        card5Obj.transform.position = cardSlot5.transform.position;
        shopSelectText.enabled = true;
    }

    public void DestroyShopCards()
    {
        //Destory all monster card game objects after a selection is made. See Card.cs
        if (GlobalVars.triggerShopCardDestruction)
        {
            Destroy(card1Obj.gameObject);
            Destroy(card2Obj.gameObject);
            Destroy(card3Obj.gameObject);
            Destroy(card4Obj.gameObject);
            Destroy(card5Obj.gameObject);
            //Reset this bool for next card selection later
            GlobalVars.triggerShopCardDestruction = false;
            shopSelectText.enabled = false;
            GlobalVars.showStartWaveInstructions = true;
            Resources.UnloadUnusedAssets();
            Debug.Log("Shop Cards Destoryed");
        }
    }

    public void ChooseRandTileFromList()
    {
        int index = Random.Range(0, validTilesList.Count);
        Debug.Log("The randomly Chosen tile is: " + validTilesList[index]);
        newTileName = validTilesList[index];
    }

    public void GetListOfValidTiles()
    {
        validTilesList = new List<string>();
        prependTileName = "Tiles/" + GlobalVars.currTier + "/" + GlobalVars.tileCardSelected + "Tiles/";
        GlobalVars.tileCounters[GlobalVars.tileCardSelected]++;
        curTiles = new string[] { GlobalVars.tileCardSelected + "Tile1", GlobalVars.tileCardSelected + "Tile2", GlobalVars.tileCardSelected + "Tile3", GlobalVars.tileCardSelected + "Tile4", GlobalVars.tileCardSelected + "Tile5", GlobalVars.tileCardSelected + "Tile6" };

        for (int i = 0; i < 6; i++)
        {
            if (validTiles[i])
            {
                validTilesList.Add(curTiles[i]);
            }
        }
    }

    public void CheckForValidTiles()
    {
        //Reset all values to false
        for (int i = 0; i < 6; i++)
        {
            validTiles[i] = false;
        }

        switch (GlobalVars.tileName)
        {
            case "StartingTile":
                //validTiles[0] = true;
                validTiles[2] = true;
                validTiles[3] = true;
                shiftAmtYpos += 1;
                break;
            case string value when value == curTiles[0]:
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
                break;
            case string value when value == curTiles[1]:
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
                break;
            case string value when value == curTiles[2]:
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
                break;
            case string value when value == curTiles[3]:
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
                break;
            case string value when value == curTiles[4]:
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
                break;
            case string value when value == curTiles[5]:
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
                break;
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
    }

    public void ChangeSong(string newSong)
    {
        StartCoroutine(FadeMusic.StartFade(GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>(), 4f, 0));
        StartCoroutine(FadeMusic.EndSong(GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>(), 4.5f));
        GlobalVars.currentSong = newSong;
        StartCoroutine(FadeMusic.StartNewSong(GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>(), 8f));
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
        */
    }
}
