using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster attributes")]
    public int health;
    public int armor;
    public string type;
    public float moveSpeed;

    [Header("Waypoint vars")]
    public bool isFacingLeft;
    public int waypointNum;
    public int currentWaypoint;
    public GameObject[] waypoints;
    public GameObject currWaypoint;

    [Header("Values used for targeting")]
    public Vector3 lastPos;
    public float distanceTraveled;

    void Awake()
    {
        lastPos = transform.position;
        isFacingLeft = true;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        currentWaypoint = TileSpawner.numOfTimesPlaced;

        foreach (GameObject waypoint in waypoints)
        {
            waypointNum = waypoint.GetComponent<WaypointManager>().waypointNum;
        }
    }

    void Update()
    {
        GetDistanceTraveled();
        FollowWaypoints();
        UpdateHealth();
        //Debug.Log("Total distance traveled: " + distanceTraveled);
    }

    public void FollowWaypoints()
    {
        if (waypointNum == currentWaypoint)
        {
            //Flip the sprite if facing left and moving right
            if (isFacingLeft && transform.position.x < waypoints[currentWaypoint].transform.position.x)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                isFacingLeft = false;
            }

            //Flip the sprite if facing right and moving left
            if (!isFacingLeft && transform.position.x > waypoints[currentWaypoint].transform.position.x)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                isFacingLeft = true;
            }

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

    public void UpdateHealth()
    {
        //Update health when damage is taken
        if (health <= 0)
        {
            // Debug.Log("Total distance traveled: " + distanceTraveled);
            Destroy(this.gameObject);
        }
    }

    public void GetDistanceTraveled()
    {
        float distance = Vector3.Distance(lastPos, transform.position);
        distanceTraveled += distance;
        lastPos= transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            GameObject projectileObj = other.gameObject;
            int incomingDamage = projectileObj.GetComponent<Projectile>().damageValue;
            Debug.Log("Amount of incoming damage: " + incomingDamage);
            health -= incomingDamage;

            //Destroy the projectile game object after damage is received
            Destroy(other.gameObject);
        }
    }
}
