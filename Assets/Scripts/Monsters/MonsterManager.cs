using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static string selectedMonster;
    public static string prependMonsterName;
    public static int monsterCount;
    List<string> monsterList;

    void Awake()
    {
        monsterCount = 0;
        monsterList = new List<string>();
    }

    void Update()
    {
        AddToMonsterList();
        SpawnMonsters(0.9f);
    }

    public void SpawnMonsters(float amtOfTime)
    {
        //Using right mouse click for testing. Eventually this function will be called after a button press or event
        if (Input.GetMouseButtonDown(1))
        {
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
            GameObject monsterObj = (GameObject)Instantiate(Resources.Load(prependMonsterName + monster));
            monsterObj.transform.position = GameObject.Find("TileManager").transform.position;
        }

        /*
        if (Input.GetMouseButtonDown(1))
        {
            //Spawn the monster object
            GameObject wolf = (GameObject)Instantiate(Resources.Load("Monsters/Forrest/Wolf"));
            wolf.transform.position = GameObject.Find("TileManager").transform.position;
        }
        */
    }

    public void AddToMonsterList()
    {
        //After each tile is placed/Monster is selected, add the new monster to the list
        if (monsterCount < TileSpawner.numOfTimesPlaced)
        {
            //Prepend the proper file path for the monster
            PrependMonsterPath();

            //Setting the case value here for testing purposes. This value will eventually be set based on the players choice
            selectedMonster = "Wolf";

            switch (selectedMonster)
            {
                case "Wolf":
                    monsterList.Add("Wolf");
                    break;

                case "Zombie":
                    monsterList.Add("Zombie");
                    break;

                case "Goblin":
                    monsterList.Add("Goblin");
                    break;
            }

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

        if (TileSpawner.tileName.Contains("Forrest"))
        {
            prependMonsterName = "Monsters/Forrest/";
        }
    }
}
