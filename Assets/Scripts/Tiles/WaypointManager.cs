using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public int waypointNum;
    GameObject tileManager;
    TileSpawner tileSpawnerScript;

    void Awake()
    {
        tileManager = GameObject.Find("TileManager");
        tileSpawnerScript = tileManager.GetComponent<TileSpawner>();

        waypointNum = tileSpawnerScript.numOfTimesPlaced;
    }
}
