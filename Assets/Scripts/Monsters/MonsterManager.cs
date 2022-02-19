using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    void Update()
    {
        SpawnMonster();
    }

    public void SpawnMonster()
    {
        //Using right mouse click for testing. Eventually this function will be called by a button press
        if (Input.GetMouseButtonDown(1))
        {
            //Spawn the monster object
            GameObject wolf = (GameObject)Instantiate(Resources.Load("Monsters/Forrest/Wolf"));
            wolf.transform.position = GameObject.Find("TileManager").transform.position;
        }
    }
}
