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

    void Awake()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        currentWaypoint = TileSpawner.numOfTimesPlaced;

        foreach (GameObject waypoint in waypoints)
        {
            waypointNum = waypoint.GetComponent<WaypointManager>().waypointNum;
        }
    }

    void Update()
    {
        FollowWaypoints();
    }

    public void FollowWaypoints()
    {
        if (waypointNum == currentWaypoint)
        {
            //Move the monsters position to the next waypoint node
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, moveSpeed * Time.deltaTime);
        }

        if (transform.position == waypoints[currentWaypoint].transform.position && waypointNum > 0)
        {
            //When the monster reaches the position of the current waypoint,
            //we decrement currentWaypoint and waypoint by 1 to trigger the above if statement again until there are no more waypoints.
            currentWaypoint--;
            waypointNum--;
        }

        if (transform.position == GameObject.Find("MainBaseWaypoint").transform.position)
        {
            //Destroy the game object once the monster reaches the main base
            Destroy(this.gameObject);

            // Todo: Subtract a point of health from the main base
        }
    }
}
