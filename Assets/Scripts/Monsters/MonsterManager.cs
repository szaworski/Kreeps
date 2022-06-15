using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private string prependMonsterName;
    private int monsterCount;
    private bool AllMonstersAreSpawned;
    private List<string> monsterList;

    public int GetMonsterCount
    {
        get { return monsterCount; }
    }
    public bool GetSetAllMonstersAreSpawned
    {
        get { return AllMonstersAreSpawned; }
        set { AllMonstersAreSpawned = value; }
    }

    GameObject tileManager;
    GameObject playerHud;
    TileSpawner tileSpawnerScript;
    PlayerHud playerHudScript;

    void Awake()
    {
        tileManager = GameObject.Find("TileManager");
        playerHud = GameObject.Find("PlayerHud");
        tileSpawnerScript = tileManager.GetComponent<TileSpawner>();
        playerHudScript = playerHud.GetComponent<PlayerHud>();

        monsterCount = 0;
        monsterList = new List<string>();
        AllMonstersAreSpawned = false;
    }

    void Update()
    {
        AddToMonsterList();
        SpawnMonsters(0.55f);
    }

    public void SpawnMonsters(float amtOfTime)
    {
        if (Input.GetKeyDown(KeyCode.Return) && playerHudScript.GetSetShowStartWaveInstructions && GameObject.Find("TileManager").transform.childCount == 0)
        {
            //Make sure that the game isn't paused
            if (!PauseMenuButtons.isPaused)
            {
                playerHudScript.GetSetShowStartWaveInstructions = false;
                //Need to call the GenerateMonsters method with a Coroutine to delay each iteration of the foreach loop
                StartCoroutine(GenerateMonsters(amtOfTime));
            }
        }
    }

    public IEnumerator GenerateMonsters(float amtOfTime)
    {
        foreach (string monster in monsterList)
        {
            yield return new WaitForSeconds(amtOfTime);

            //Spawn the monster object
            Debug.Log("Monster Path: " + monster);
            GameObject monsterObj = (GameObject)Instantiate(Resources.Load(monster), GameObject.Find("TileManager").transform);
            monsterObj.transform.position = GameObject.Find("TileManager").transform.position;
        }

        AllMonstersAreSpawned = true;
        //Debug.Log("Were all monsters spawned?: " + AllMonstersAreSpawned);
    }

    public void AddToMonsterList()
    {
        //After each tile is placed/Monster is selected, add the new monster to the list
        if (monsterCount < tileSpawnerScript.numOfTimesPlaced && tileSpawnerScript.triggerMonsterCardDestruction || monsterCount < tileSpawnerScript.numOfTimesPlaced && tileSpawnerScript.numOfTimesPlaced <= 1)
        {
            //Prepend the proper file path for the monster
            PrependMonsterPath();

            if (tileSpawnerScript.numOfTimesPlaced <= 1)
            {
                //Get a random starting monster from the 3 provided
                var rand = new System.Random();
                int startingMonsterIndex = rand.Next(3);
                switch (startingMonsterIndex)
                {
                    case 0:
                        tileSpawnerScript.monsterCardSelected = "Forest/Wolf";
                        break;
                    case 1:
                        tileSpawnerScript.monsterCardSelected = "Mountain/Ranger";
                        break;
                    case 2:
                        tileSpawnerScript.monsterCardSelected = "Graveyard/Zombie";
                        break;
                }
            }

            //Add the selected monster to the list
            monsterList.Add(prependMonsterName + tileSpawnerScript.monsterCardSelected);

            monsterCount++;
            Debug.Log("Monster Count: " + monsterCount);
            Debug.Log("Monster List: " + String.Join("\n", monsterList));
        }
    }

    public void PrependMonsterPath()
    {
        if (tileSpawnerScript.tileName.Contains("Starting"))
        {
            prependMonsterName = "Monsters/Tier1/";
        }

        else
        {
            prependMonsterName = "Monsters/" + tileSpawnerScript.currTier + "/" + tileSpawnerScript.tileCardSelected + "/";
        }
    }
}
