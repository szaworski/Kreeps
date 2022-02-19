using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int health;
    public int armor;
    public string type;
    public float moveSpeed;
    public int waypointNum;
    public int currentWaypoint;
    public GameObject[] waypoints;
    public GameObject currWaypoint;
    public bool isSpawned;

    private void Awake()
    {

    }

    void Update()
    {
        SpawnMonster();
        FollowWaypoints();
    }

    public void SpawnMonster()
    {
        if (Input.GetMouseButtonDown(1))
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            currentWaypoint = TileSpawner.numOfTimesPlaced;

            foreach (GameObject waypoint in waypoints)
            {
                waypointNum = waypoint.GetComponent<WaypointManager>().waypointNum;
            }

            //Spawn the monster object
            GameObject wolf = (GameObject)Instantiate(Resources.Load("Monsters/Forrest/Wolf"));
            wolf.transform.position = GameObject.Find("TileManager").transform.position;
            isSpawned = true;
        }
    }

    public void FollowWaypoints()
    {
        if (isSpawned)
        {
            if (waypointNum == currentWaypoint)
            {
                //Move the monsters position to the next waypoint node
                transform.position = Vector3.Lerp(transform.position, waypoints[currentWaypoint - 1].transform.position, 2 * Time.deltaTime);

                if (transform.position == waypoints[currentWaypoint -1].transform.position)
                {
                    currentWaypoint--;
                }
            }
        }
    }
}
