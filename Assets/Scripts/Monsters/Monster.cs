using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster attributes")]
    public float maxHealth;
    public float health;
    public float armor;
    public int goldBounty;
    public string type;
    public float moveSpeed;
    public float hpRegen;
    public float hpRegenCd;
    public float evasionChance;
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

    [Header("Damage Effect vars")]
    public bool isTakingDamage;
    public GameObject fireAnim;
    public float fireAnimCd;

    public GameObject iceAnim;
    public float iceAnimCd;
    public bool iceSlowStatus;
    public float iceSlowAmt;

    void Awake()
    {
        lastPos = transform.position;
        isFacingLeft = true;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        currentWaypoint = TileSpawner.numOfTimesPlaced;

        //Apply any bonus values to the monster
        ApplySpawnBonuses();

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
        ResetEffectAnims();
        ApplyHpRegen();

        if (isTakingDamage)
        {
            StartCoroutine(ShakeHpContainer(0.1f));
        }

        if (health <= 0)
        {
            StartCoroutine(DestroyMonster(0.1f));
        }

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
            float incomingDamage = projectileObj.GetComponent<Projectile>().damageValue;
            string damageType = projectileObj.GetComponent<Projectile>().damageType;
            //Debug.Log("Amount of incoming damage: " + incomingDamage);

            // Check the monster type against damage type and modify damage values
            switch (damageType)
            {
                case "Fire":

                    fireAnim.SetActive(true);
                    fireAnimCd = 0.5f + Time.time;

                    if (type == "Beast")
                    {
                        incomingDamage = (incomingDamage * 2);
                    }

                    else if (type == "Brute")
                    {
                        incomingDamage = (incomingDamage / 2);
                    }
                    break;

                case "Ice":

                    iceAnim.SetActive(true);
                    iceAnimCd = 0.45f + Time.time;

                    if (!iceSlowStatus)
                    {
                        iceSlowStatus = true;
                        moveSpeed -= iceSlowAmt;
                    }

                    if (type == "Humanoid")
                    {
                        incomingDamage = (incomingDamage * 2);
                    }

                    else if (type == "Undead")
                    {
                        incomingDamage = (incomingDamage / 2);
                    }
                    break;

                case "Thunder":

                    if (type == "Brute")
                    {
                        incomingDamage = (incomingDamage * 2);
                    }

                    else if (type == "Beast")
                    {
                        incomingDamage = (incomingDamage / 2);
                    }
                    break;

                case "Holy":
                    if (type == "Undead")
                    {
                        incomingDamage = (incomingDamage * 2);
                    }

                    else if (type == "Humanoid")
                    {
                        incomingDamage = (incomingDamage / 2);
                    }
                    break;

                case "Swift":
                    if (type == "Pest")
                    {
                        incomingDamage = (incomingDamage * 2);
                    }

                    else if (type == "Demon")
                    {
                        incomingDamage = (incomingDamage / 2);
                    }
                    break;

                case "Cosmic":
                    if (type == "Demon")
                    {
                        incomingDamage = (incomingDamage * 2);
                    }

                    else if (type == "Pest")
                    {
                        incomingDamage = (incomingDamage / 2);
                    }
                    break;
            }

            //Adjust incomingDamage base on the armor value
            incomingDamage -= armor;

            //Subtract the amount of damage taken from the health variable (Delay this for certain animations)
            if (damageType == "Thunder")
            {
                if (this.gameObject != null)
                {
                    //Spawn in the animation object
                    GameObject thunderAnimObj = (GameObject)Instantiate(Resources.Load("Animations/Thunder"), gameObject.transform);
                    thunderAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);

                    //Subtract health after the animation plays
                    StartCoroutine(SubtractHealth(incomingDamage, other, 0.45f));
                }
            }

            else if (damageType == "Holy")
            {
                if (this.gameObject != null)
                {
                    //Spawn in the animation object
                    GameObject holyAnimObj = (GameObject)Instantiate(Resources.Load("Animations/Holy"), gameObject.transform);
                    holyAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

                    //Subtract health after the animation plays
                    StartCoroutine(SubtractHealth(incomingDamage, other, 0));
                }
            }

            else
            {
                //Make sure damage is greater than 0, and evasion does not occur
                if (incomingDamage > 0 && Random.value > evasionChance)
                {
                    isTakingDamage = true;
                    health -= incomingDamage;
                    healthText.SetText(health.ToString());
                }
                //Destroy the projectile game object after damage is received
                Destroy(other.gameObject);
            }
        }
    }

    void ResetEffectAnims()
    {
        if (Time.time > fireAnimCd && fireAnim.activeSelf)
        {
            fireAnim.SetActive(false);
            fireAnimCd = 0.5f + Time.time;
        }

        if (Time.time > iceAnimCd && iceAnim.activeSelf)
        {
            iceAnim.SetActive(false);
            iceAnimCd = 0.45f + Time.time;
            iceSlowStatus = false;
            moveSpeed += iceSlowAmt;
        }
    }

    void ApplyHpRegen()
    {
        if (Time.time > hpRegenCd && hpRegen > 0)
        {
            if (health + hpRegen < maxHealth)
            {
                hpRegenCd = 1f + Time.time;
                health += hpRegen;
                healthText.SetText(health.ToString());
            }

            else if (health + hpRegen >= maxHealth)
            {
                hpRegenCd = 1f + Time.time;
                health = maxHealth;
                healthText.SetText(health.ToString());
            }
        }
    }

    void ApplySpawnBonuses()
    {
        //Apply any Tier 1 bonuses
        health += 3 * TileSpawner.numOfRivers;
        armor += 0.5f * TileSpawner.numOfMountains;
        moveSpeed += 0.05f * TileSpawner.numOfForrests;
        hpRegen += 0.5f * TileSpawner.numOfGraveyards;
        evasionChance += 0.03f * TileSpawner.numOfSwamps;

        //Set the maxHealth value
        maxHealth = health;
    }

    IEnumerator DestroyMonster(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //After a delay, give gold to the player = to the "goldBounty" value
        PlayerHud.newGoldValue = PlayerHud.gold + goldBounty;
        Destroy(this.gameObject);
    }

    IEnumerator SubtractHealth(float incomingDamage, Collider2D projectileObj, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (this.gameObject != null)
        {
            //Make sure damage is greater than 0, and evasion does not occur
            if (incomingDamage > 0 && Random.value > evasionChance)
            {
                isTakingDamage = true;
                health -= incomingDamage;
                healthText.SetText(health.ToString());
            }

            else
            {
                //Play "Miss" animation
                GameObject missAnimObj = (GameObject)Instantiate(Resources.Load("Animations/Miss"), gameObject.transform);
                missAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            //Destroy the projectile game object after damage is received
            Destroy(projectileObj.gameObject);
        }
    }

    IEnumerator ShakeHpContainer(float resetTimer)
    {
        //Shake the HP container
        HealthContainer.transform.position = new Vector3(gameObject.transform.position.x + Mathf.Sin(Time.time * 75) * 0.025f, HealthContainer.transform.position.y, HealthContainer.transform.position.z);

        //Stop the shake after a delay, and reset the position
        yield return new WaitForSeconds(resetTimer);
        HealthContainer.transform.position = new Vector3(gameObject.transform.position.x, HealthContainer.transform.position.y, HealthContainer.transform.position.z);
        isTakingDamage = false;
    }
}
