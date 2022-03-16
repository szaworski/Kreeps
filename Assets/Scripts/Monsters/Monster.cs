using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster attributes")]
    public int health;
    public int armor;
    public int goldBounty;
    public string type;
    public float moveSpeed;
    public TMP_Text healthText;
    public TMP_Text maxHealthText;
    public GameObject HealthContainer;

    [Header("Waypoint vars")]
    public bool isFacingLeft;
    public int waypointNum;
    public int currentWaypoint;
    public GameObject[] waypoints;
    public GameObject currWaypoint;

    [Header("Vars used for targeting")]
    public Vector3 lastPos;
    public float distanceTraveled;

    [Header("Vars used for tile specific attributes")]
    public bool checkForrestOverlap;
    public bool checkGraveyardOverlap;
    public bool checkMountainOverlap;
    public bool checkRiverOverlap;
    public bool checkSwampOverlap;

    [Header("Animation vars")]
    public GameObject fireAnim;
    public float fireAnimCd;

    void Awake()
    {
        lastPos = transform.position;
        isFacingLeft = true;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        currentWaypoint = TileSpawner.numOfTimesPlaced;

        //Set the health values to be shown
        healthText.SetText(health.ToString());
        maxHealthText.SetText(health.ToString());

        foreach (GameObject waypoint in waypoints)
        {
            waypointNum = waypoint.GetComponent<WaypointManager>().waypointNum;
        }
    }

    void Update()
    {
        GetDistanceTraveled();
        FollowWaypoints();
        destroyMonster();
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
                //Stop the health text from rotating
                HealthContainer.transform.rotation = Quaternion.Euler(0, 0, 0);
                isFacingLeft = false;
            }

            //Flip the sprite if facing right and moving left
            if (!isFacingLeft && transform.position.x > waypoints[currentWaypoint].transform.position.x)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                //Stop the health text from rotating
                HealthContainer.transform.rotation = Quaternion.Euler(0, 0, 0);
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

    public void destroyMonster()
    {
        //Update health when damage is taken
        if (health <= 0)
        {
            //Give gold to the player = to the "goldBounty" value
            PlayerHud.newGoldValue = PlayerHud.gold + goldBounty;
            Destroy(this.gameObject);
        }
    }

    public void GetDistanceTraveled()
    {
        float distance = Vector3.Distance(lastPos, transform.position);
        distanceTraveled += distance;
        lastPos = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            GameObject projectileObj = other.gameObject;
            int incomingDamage = projectileObj.GetComponent<Projectile>().damageValue;
            string damageType = projectileObj.GetComponent<Projectile>().damageType;
            //Debug.Log("Amount of incoming damage: " + incomingDamage);

            // Check the monster type against damage type and modify damage values
            switch (damageType)
            {
                case "Fire":

                    fireAnim.SetActive(true);

                    if (type == "Beast")
                    {
                        incomingDamage = incomingDamage * 2;
                    }

                    else if (type == "Brute")
                    {
                        incomingDamage = incomingDamage / 2;
                    }
                    break;

                case "Ice":
                    if (type == "Humanoid ")
                    {
                        incomingDamage = incomingDamage * 2;
                    }

                    else if (type == "Undead")
                    {
                        incomingDamage = incomingDamage / 2;
                    }
                    break;

                case "Thunder":
                    if (type == "Brute ")
                    {
                        incomingDamage = incomingDamage * 2;
                    }

                    else if (type == "Beast")
                    {
                        incomingDamage = incomingDamage / 2;
                    }
                    break;

                case "Holy":
                    if (type == "Undead ")
                    {
                        incomingDamage = incomingDamage * 2;
                    }

                    else if (type == "Humanoid")
                    {
                        incomingDamage = incomingDamage / 2;
                    }
                    break;

                case "Swift":
                    if (type == "Vermin ")
                    {
                        incomingDamage = incomingDamage * 2;
                    }

                    else if (type == "Trickster")
                    {
                        incomingDamage = incomingDamage / 2;
                    }
                    break;

                case "Cosmic":
                    if (type == "Trickster ")
                    {
                        incomingDamage = incomingDamage * 2;
                    }

                    else if (type == "Vermin")
                    {
                        incomingDamage = incomingDamage / 2;
                    }
                    break;
            }

            //Subtract the amount of damage taken from the health variable
            health -= incomingDamage;
            healthText.SetText(health.ToString());
            //Destroy the projectile game object after damage is received
            Destroy(other.gameObject);

            if (Time.time > fireAnimCd)
            {
                fireAnim.SetActive(false);
                fireAnimCd = 0.2f + Time.time;
            }
        }


        //Check if the monster enter a forrest tile and was not already in one
        if (other.gameObject.tag == "ForrestTile" && !checkForrestOverlap)
        {
            EnterForrestTile();
        }

        //Check if the monster enters a different kind of map tile (This means the monster is exiting the current tile type. Also ignore "Projectile" objects)
        if (other.gameObject.tag != "ForrestTile" && !other.gameObject.tag.Contains("Projectile") && checkForrestOverlap)
        {
            ExitForrestTile();
        }
    }
    public void EnterForrestTile()
    {
        moveSpeed += 0.1f;
        checkForrestOverlap = true;
        //Debug.Log("Increased move speed");
    }

    public void ExitForrestTile()
    {
        moveSpeed -= 0.1f;
        checkForrestOverlap = false;
        //Debug.Log("Decreased move speed");
    }
}
