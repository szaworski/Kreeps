using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float armor;
    [SerializeField] private int goldBounty;
    [SerializeField] private string type;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float hpRegen;
    [SerializeField] private float evasionChance;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text maxHealthText;
    [SerializeField] private TMP_Text armorText;
    [SerializeField] private GameObject HealthContainer;
    private float hpRegenCd;
    private int damage;

    [Header("Vars used for targeting")]
    private Vector3 lastPos;
    public float distanceTraveled;

    [Header("Damage Effect vars")]
    [SerializeField] private bool isTakingDamage;
    [SerializeField] private float iceSlowCd;
    [SerializeField] private bool iceSlowStatus;
    [SerializeField] private float iceSlowAmt;

    //Waypoint vars
    private bool isFacingLeft;
    private int waypointNum;
    private int currentWaypoint;
    private GameObject[] waypoints;

    void Awake()
    {
        iceSlowAmt = 0.12f;
        damage = 1;
        lastPos = transform.position;
        isFacingLeft = true;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        currentWaypoint = TileSpawner.numOfTimesPlaced;
        //Apply any bonus values to the monster
        ApplySpawnBonuses();
        //Set the health values to be shown
        healthText.SetText(health.ToString());
        maxHealthText.SetText(health.ToString());
        armorText.SetText(armor.ToString());

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

            //Subtract a point of health from the main base
            SubtractPlayerHealth();
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
            GameObject monsterTarget = projectileObj.GetComponent<Projectile>().target;
            float incomingDamage = projectileObj.GetComponent<Projectile>().damageValue;
            float projectileSpeed = projectileObj.GetComponent<Projectile>().projectileSpeed;
            float slowAmt = projectileObj.GetComponent<Projectile>().slowAmt;
            string damageType = projectileObj.GetComponent<Projectile>().damageType;
            //Debug.Log("Amount of incoming damage: " + incomingDamage);

            // Check the monster type against damage type and modify damage values
            switch (damageType)
            {
                case var _ when damageType.Contains("Fire"):

                    if (type == "Beast")
                    {
                        incomingDamage *= 2;
                    }

                    else if (type == "Brute")
                    {
                        incomingDamage /= 2;
                    }
                    break;

                case var _ when damageType.Contains("Ice"):

                    iceSlowCd = 0.45f + Time.time;

                    if (!iceSlowStatus || iceSlowStatus && slowAmt > iceSlowAmt)
                    {
                        // if a hit is detected with a greater slowAmt, adjust iceSlowAmt accordingly without stacking the values
                        if (iceSlowStatus && slowAmt > iceSlowAmt)
                        {
                            //Add back the current slow amount so we don't stack the slow values
                            moveSpeed += iceSlowAmt;
                        }

                        iceSlowStatus = true;
                        iceSlowAmt = slowAmt;
                        moveSpeed -= iceSlowAmt;
                    }

                    if (type == "Humanoid")
                    {
                        incomingDamage *= 2;
                    }

                    else if (type == "Undead")
                    {
                        incomingDamage /= 2;
                    }
                    break;

                case var _ when damageType.Contains("Thunder"):

                    if (type == "Brute")
                    {
                        incomingDamage *= 2;
                    }

                    else if (type == "Beast")
                    {
                        incomingDamage /= 2;
                    }
                    break;

                case var _ when damageType.Contains("Holy"):

                    if (type == "Undead")
                    {
                        incomingDamage *= 2;
                    }

                    else if (type == "Humanoid")
                    {
                        incomingDamage /= 2;
                    }
                    break;

                case var _ when damageType.Contains("Swift"):

                    if (type == "Pest")
                    {
                        incomingDamage *= 2;
                    }

                    else if (type == "Demon")
                    {
                        incomingDamage /= 2;
                    }
                    break;

                case var _ when damageType.Contains("Cosmic"):

                    if (type == "Demon")
                    {
                        incomingDamage *= 2;
                    }

                    else if (type == "Pest")
                    {
                        incomingDamage /= 2;
                    }
                    break;
            }

            //Adjust incomingDamage base on the armor value
            incomingDamage -= armor;

            //Subtract the amount of damage taken from the health variable (First, check for teleporting projectiles with unique animations)
            if (this.gameObject != null && projectileSpeed == 1)
            {
                float yShiftAmt = 0;
                float delayAmt = 0;

                if (damageType == ("Thunder"))
                {
                    yShiftAmt = 0.1f;
                    delayAmt = 0.45f;
                }

                //Spawn in the animation object
                GameObject damageAnimObj = (GameObject)Instantiate(Resources.Load("Animations/" + damageType), gameObject.transform);
                damageAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yShiftAmt, gameObject.transform.position.z);
                damageAnimObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                //Subtract health after the animation plays
                StartCoroutine(SubtractHealth(incomingDamage, other, damageType, delayAmt));
            }

            //For certain types of projectiles, make sure this monster object is the target of the projectile (Check for moving projectiles)
            else if (this.gameObject != null && this.gameObject == monsterTarget && projectileSpeed > 1)
            {
                StartCoroutine(SubtractHealth(incomingDamage, other, damageType, 0));
            }

            //For towers without moving projectiles, we don't check for a target (Finally, check for AOE projectiles)
            else if (projectileSpeed == 0)
            {
                GameObject damageAnimObj = (GameObject)Instantiate(Resources.Load("Animations/" + damageType), gameObject.transform);
                damageAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                damageAnimObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                //Subtract health after the animation plays
                if (damageType.Contains("Cosmic"))
                {
                    StartCoroutine(SubtractHealth(incomingDamage, other, damageType, 0.5f));
                }

                else 
                {
                    StartCoroutine(SubtractHealth(incomingDamage, other, damageType, 0));
                }
            }
        }
    }

    void ResetEffectAnims()
    {
        if (Time.time > iceSlowCd && iceSlowStatus)
        {
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
        health += 6 * TileSpawner.numOfRivers;
        armor += 1 * TileSpawner.numOfMountains;
        moveSpeed += 0.05f * TileSpawner.numOfForrests;
        hpRegen += 1 * TileSpawner.numOfGraveyards;
        evasionChance += 0.03f * TileSpawner.numOfSwamps;

        //Set the maxHealth value
        maxHealth = health;
    }

    void SubtractPlayerHealth()
    {
        PlayerHealth.newPlayerHealthValue -= damage;
    }

    IEnumerator DestroyMonster(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //After a delay, give gold to the player = to the "goldBounty" value
        PlayerHud.newGoldValue = PlayerHud.gold + goldBounty;
        Destroy(this.gameObject);
    }

    IEnumerator SubtractHealth(float incomingDamage, Collider2D projectileObj, string damageType, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (this.gameObject != null)
        {
            float randomFloat = Random.value;

            if (incomingDamage <= 0)
            {
                StartCoroutine(SpawnDamagePopup(incomingDamage, damageType, 0.25f));
            }

            //Make sure damage is greater than 0, and evasion does not occur
            else if (incomingDamage > 0 && randomFloat > evasionChance)
            {
                isTakingDamage = true;
                health -= incomingDamage;
                healthText.SetText(health.ToString());
                StartCoroutine(SpawnDamagePopup(incomingDamage, damageType, 0.25f));
            }

            else if (randomFloat <= evasionChance)
            {
                //Play "Miss" animation
                GameObject missAnimObj = (GameObject)Instantiate(Resources.Load("Animations/Miss"), gameObject.transform);
                missAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                missAnimObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            //Destroy the projectile game object after damage is received
            if (projectileObj != null)
            {
                yield return new WaitForSeconds(0.02f);
                Destroy(projectileObj.gameObject);
            }
        }
    }

    IEnumerator SpawnDamagePopup(float damageVal, string damageType, float delayTime)
    {
        string convertedDamageType = "";
        //Get the specifc damage type
        switch (damageType)
        {
            case var _ when damageType.Contains("Neutral"):
                convertedDamageType = "Neutral";
                break;
            case var _ when damageType.Contains("Fire"):
                convertedDamageType = "Fire";
                break;
            case var _ when damageType.Contains("Ice"):
                convertedDamageType = "Ice";
                break;
            case var _ when damageType.Contains("Thunder"):
                convertedDamageType = "Thunder";
                break;
            case var _ when damageType.Contains("Holy"):
                convertedDamageType = "Holy";
                break;
            case var _ when damageType.Contains("Swift"):
                convertedDamageType = "Swift";
                break;
            case var _ when damageType.Contains("Cosmic"):
                convertedDamageType = "Cosmic";
                break;
        }

        //Spawn the damage popup
        GameObject damagePopupObj = (GameObject)Instantiate(Resources.Load("MonsterAttributes/" + convertedDamageType + "DamagePopup"), gameObject.transform);
        damagePopupObj.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-0.02f, 0.02f), gameObject.transform.position.y + 0.25f, gameObject.transform.position.z);
        damagePopupObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (damageVal <= 0)
        {
            damagePopupObj.GetComponent<TextMeshPro>().text = "0";
        }

        else
        {
            damagePopupObj.GetComponent<TextMeshPro>().text = damageVal.ToString();
        }

        //Destroy the damage popup after a short delay
        yield return new WaitForSeconds(delayTime);
        Destroy(damagePopupObj.gameObject);
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
