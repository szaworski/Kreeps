using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public int waypointNum;

    void Start()
    {
        waypointNum = GlobalVars.tileCounters["numOfTimesPlaced"];
    }
}
