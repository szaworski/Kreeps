using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static string selectedMonster;
    public static string prependMonsterName;
    List<string> monsterList;

    void Awake()
    {
        monsterList = new List<string>();
    }

    void Update()
    {
        SpawnMonster();
    }

    public void SpawnMonster()
    {
        //Using right mouse click for testing. Eventually this function will be called in a for loop to spawn a list of monsters
        if (Input.GetMouseButtonDown(1))
        {
            //Spawn the monster object
            GameObject wolf = (GameObject)Instantiate(Resources.Load("Monsters/Forrest/Wolf"));
            wolf.transform.position = GameObject.Find("TileManager").transform.position;
        }
    }

    public void AddToMonsterList()
    {
        //Not used for anything yet
        switch (selectedMonster)
        {
            case "Wolf":
                monsterList.Add("Wolf");
                break;

            case "zombie":
                monsterList.Add("Zombie");
                break;

            case "goblin":
                monsterList.Add("Goblin");
                break;
        }
    }

    public void PrependMonsterPath()
    {
        if (TileSpawner.tileName.Contains("Forrest"))
        {
            prependMonsterName = "Monsters/Forrest/";
        }

        else
        {

        }
    }
}
