﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static string selectedMonster;
    public static string prependMonsterName;
    public static int monsterCount;
    public static bool AllMonstersAreSpawned;
    List<string> monsterList;

    void Awake()
    {
        monsterCount = 0;
        monsterList = new List<string>();
    }

    void Update()
    {
        AddToMonsterList();
        SpawnMonsters(0.8f);
    }

    public void SpawnMonsters(float amtOfTime)
    {
        //Using enter for testing. Eventually this function will be called after a button press or event
        //Turned off for now to test tower placement
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerHud.showStartWaveInstructions = false;

            //Need to call the GenerateMonsters method with a Coroutine to delay each iteration of the foreach loop
            StartCoroutine(GenerateMonsters(amtOfTime));
        }
    }

    public IEnumerator GenerateMonsters(float amtOfTime)
    {
        foreach (string monster in monsterList)
        {
            yield return new WaitForSeconds(amtOfTime);

            //Spawn the monster object
            Debug.Log("Monster Path: " + prependMonsterName + monster);
            GameObject monsterObj = (GameObject)Instantiate(Resources.Load(prependMonsterName + monster), GameObject.Find("TileManager").transform);
            monsterObj.transform.position = GameObject.Find("TileManager").transform.position;
        }

        AllMonstersAreSpawned = true;
        //Debug.Log("Were all monsters spawned?: " + AllMonstersAreSpawned);
    }

    public void AddToMonsterList()
    {
        //After each tile is placed/Monster is selected, add the new monster to the list
        if (monsterCount < TileSpawner.numOfTimesPlaced && TileSpawner.triggerMonsterCardDestruction || monsterCount < TileSpawner.numOfTimesPlaced && TileSpawner.numOfTimesPlaced <= 1)
        {
            //Prepend the proper file path for the monster
            PrependMonsterPath();

            if (TileSpawner.numOfTimesPlaced <= 1)
            {
                selectedMonster = "Wolf";
            }

            else
            {
                selectedMonster = TileSpawner.monsterCardSelected;
            }

            //Add the selected monster to the list
            monsterList.Add(selectedMonster);

            monsterCount++;
            Debug.Log("Monster Count: " + monsterCount);
            Debug.Log("Monster List: " + String.Join("\n", monsterList));
        }
    }

    public void PrependMonsterPath()
    {
        if (TileSpawner.tileName.Contains("Starting"))
        {
            prependMonsterName = "Monsters/Forrest/";
        }

        switch (TileSpawner.tileCardSelected)
        {
            case "Forrest":
                prependMonsterName = "Monsters/Forrest/";
                break;

            case "Graveyard":
                prependMonsterName = "Monsters/Graveyard/";
                break;

            case "River":
                prependMonsterName = "Monsters/River/";
                break;

            case "Mountain":
                prependMonsterName = "Monsters/Mountain/";
                break;

            case "Swamp":
                prependMonsterName = "Monsters/Swamp/";
                break;
        }
    }
}
