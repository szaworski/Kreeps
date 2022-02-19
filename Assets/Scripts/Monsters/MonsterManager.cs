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
        if (Input.GetMouseButtonDown(1))
        {
            //Spawn the monster object
            GameObject wolf = (GameObject)Instantiate(Resources.Load("Monsters/Forrest/Wolf"));
            wolf.transform.position = GameObject.Find("TileManager").transform.position;
        }
    }
}
