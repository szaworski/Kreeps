using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private string prependMonsterName;
    private List<string> monsterList;

    void Awake()
    {
        monsterList = new List<string>();
    }

    void Update()
    {
        AddToMonsterList();
        SpawnMonsters(0.55f);
    }

    public void SpawnMonsters(float amtOfTime)
    {
        if (Input.GetKeyDown(KeyCode.Return) && GlobalVars.showStartWaveInstructions && GameObject.Find("TileManager").transform.childCount == 0)
        {
            //Reset the music volume
            GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>().volume = GlobalVars.musicVolume;

            //Make sure that the game isn't paused
            if (!GlobalVars.isPaused)
            {
                GlobalVars.showStartWaveInstructions = false;
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

        GlobalVars.allMonstersAreSpawned = true;
        //Debug.Log("Were all monsters spawned?: " + AllMonstersAreSpawned);
    }

    public void AddToMonsterList()
    {
        //After each tile is placed/Monster is selected, add the new monster to the list
        if (GlobalVars.monsterCount < GlobalVars.tileCounters["numOfTimesPlaced"] && GlobalVars.triggerMonsterCardDestruction || GlobalVars.monsterCount < GlobalVars.tileCounters["numOfTimesPlaced"] && GlobalVars.tileCounters["numOfTimesPlaced"] <= 1)
        {
            //Prepend the proper file path for the monster
            PrependMonsterPath();

            if (GlobalVars.tileCounters["numOfTimesPlaced"] <= 1)
            {
                GlobalVars.monsterCardSelected = "Goon";
            }

            //Add the selected monster to the list
            monsterList.Add(prependMonsterName + GlobalVars.monsterCardSelected);

            GlobalVars.monsterCount++;
            Debug.Log("Monster Count: " + GlobalVars.monsterCount);
            Debug.Log("Monster List: " + String.Join("\n", monsterList));
        }
    }

    public void PrependMonsterPath()
    {
        if (GlobalVars.tileName.Contains("Starting"))
        {
            prependMonsterName = "Monsters/Tier1/";
        }

        else
        {
            prependMonsterName = "Monsters/" + GlobalVars.currTier + "/" + GlobalVars.tileCardSelected + "/";
        }
    }
}
