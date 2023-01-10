using System.Collections;
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
    [SerializeField] private GameObject ArmorObject;
    private float reducedMoveSpeed;
    private float hpRegenCd;
    private int damage;
    private Color green = new Vector4(0, 1, 0, 1);
    private Color yellow = new Vector4(1, 1, 0.02f, 1);
    private Color red = new Vector4(1, 0, 0, 1);

    [Header("Vars used for targeting")]
    private Vector3 lastPos;
    public float distanceTraveled;

    [Header("Damage Effect vars")]
    [SerializeField] private bool isTakingDamage;
    [SerializeField] private float iceSlowCd;
    [SerializeField] private bool iceSlowStatus;
    [SerializeField] private float iceSlowAmt;
    [SerializeField] private bool isPoisoned;
    [SerializeField] private bool isTakingPoisonDamage;

    //Waypoint vars
    private bool isFacingLeft;
    private int waypointNum;
    private int currentWaypoint;
    private GameObject[] waypoints;

    void Awake()
    {
        damage = 1;
        lastPos = transform.position;
        isFacingLeft = true;
        ArmorObject = HealthContainer.transform.GetChild(3).gameObject;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        currentWaypoint = GlobalVars.tileCounters["numOfTimesPlaced"];
        //Apply any bonus values to the monster
        ApplySpawnBonuses();
        //Set the health values to be shown
        healthText.SetText(health.ToString());
        maxHealthText.SetText(health.ToString());
        armorText.SetText(armor.ToString());

        healthText.color = green;
        maxHealthText.color = green;

        foreach (GameObject waypoint in waypoints)
        {
            waypointNum = waypoint.GetComponent<WaypointManager>().waypointNum;
        }

        if (armor <= 0)
        {
            ArmorObject.SetActive(false);
        }
    }

    void Update()
    {
        GetDistanceTraveled();
        FollowWaypoints();
        ResetEffectAnims();
        ApplyHpRegen();
        LimitMoveSpeedReduction();

        if (isTakingDamage)
        {
            StartCoroutine(ShakeHpContainer(0.1f));
        }

        if (health <= 0)
        {
            StartCoroutine(DestroyMonster(0.1f));
        }

        if (isPoisoned && !isTakingPoisonDamage && GlobalVars.bonusExtraStats["SwiftPsnDmgUp"] > 0)
        {
            StartCoroutine(inflictPoisonDamage(0.5f));
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
            GetSound("MonsterSounds", "PlayerDamage");

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
            float bonusArmorDamage = projectileObj.GetComponent<Projectile>().bonusArmorDamage;
            float projectileSpeed = projectileObj.GetComponent<Projectile>().projectileSpeed;
            float slowAmt = projectileObj.GetComponent<Projectile>().slowAmt;
            float critChance = projectileObj.GetComponent<Projectile>().critChance;
            string damageType = projectileObj.GetComponent<Projectile>().damageType;
            bool isWeapon = projectileObj.GetComponent<Projectile>().isWeapon;
            bool isCritHit = false;
            string prependWeaponAnim = "";
            //Debug.Log("Amount of incoming damage: " + incomingDamage);

            //Vars used to shift the position/delay different animations
            float yShiftAmt = 0;
            float delayAmt = 0;

            // Check the monster type against damage type and modify damage values
            switch (damageType)
            {
                case var _ when damageType.Contains("Fire"):

                    if (damageType.Contains("Fire2"))
                    {
                        yShiftAmt = 0.05f;
                        delayAmt = 0f;
                    }
                    else
                    {
                        yShiftAmt = 0f;
                        delayAmt = 0f;
                    }

                    if (type.Contains("Beast") && armor <= 0)
                    {
                        incomingDamage *= 1.5f;
                    }
                    else if (type.Contains("Brute"))
                    {
                        incomingDamage *= 0.5f;
                    }
                    break;

                case var _ when damageType.Contains("Ice"):

                    iceSlowCd = 0.8f + Time.time;

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

                    if (type.Contains("Humanoid") && armor <= 0)
                    {
                        incomingDamage *= 1.5f;
                    }
                    else if (type.Contains("Undead"))
                    {
                        incomingDamage *= 0.5f;
                    }
                    break;

                case var _ when damageType.Contains("Thunder"):

                    if (damageType.Contains("Thunder2"))
                    {
                        yShiftAmt = 0;
                        delayAmt = 0;
                    }
                    else if (!isWeapon || isWeapon && !GlobalVars.useSlashAnim)
                    {
                        yShiftAmt = 0.1f;
                        delayAmt = 0.1f;
                    }

                    if (type.Contains("Brute") && armor <= 0)
                    {
                        incomingDamage *= 1.5f;
                    }
                    else if (type.Contains("Beast"))
                    {
                        incomingDamage *= 0.5f;
                    }
                    break;

                case var _ when damageType.Contains("Holy"):

                    if (type.Contains("Undead") && armor <= 0)
                    {
                        incomingDamage *= 1.5f;
                    }
                    else if (type.Contains("Humanoid"))
                    {
                        incomingDamage *= 0.5f;
                    }
                    break;

                case var _ when damageType.Contains("Swift"):

                    if (type.Contains("Pest") && armor <= 0)
                    {
                        incomingDamage *= 1.5f;
                    }
                    else if (type.Contains("Demon"))
                    {
                        incomingDamage *= 0.5f;
                    }
                    break;

                case var _ when damageType.Contains("Cosmic"):

                    if (!isWeapon || isWeapon && !GlobalVars.useSlashAnim)
                    {
                        delayAmt = 0.5f;
                    }

                    if (type.Contains("Demon") && armor <= 0)
                    {
                        incomingDamage *= 1.5f;
                    }
                    else if (type.Contains("Pest"))
                    {
                        incomingDamage *= 0.5f;
                    }
                    break;
            }

            float randFloat;

            //Apply any special bonuses if applicable  
            switch (damageType)
            {
                case var _ when damageType.Contains("Fire"):

                    randFloat = Random.value;

                    if (randFloat <= 0.25f)
                    {
                        incomingDamage += GlobalVars.bonusExtraStats["FireBurnUp"];
                    }
                    break;

                case var _ when damageType.Contains("Thunder"):

                    randFloat = Random.value;

                    if (armor > 0)
                    {
                        incomingDamage += bonusArmorDamage;
                    }

                    if (randFloat <= critChance)
                    {
                        isCritHit = true;
                        incomingDamage *= 2;
                    }
                    break;

                case var _ when damageType.Contains("Swift"):

                    if (!isPoisoned)
                    {
                        StartCoroutine(triggerPoison(0.5f));
                    }
                    break;

                case var _ when damageType.Contains("Cosmic"):

                    randFloat = Random.value;

                    if (randFloat <= critChance)
                    {
                        isCritHit = true;
                        incomingDamage *= 2;
                    }
                    break;
            }

            //Round the damage value in case it has a decimal
            incomingDamage = Mathf.Floor(incomingDamage);

            if (isWeapon && GlobalVars.useSlashAnim)
            {
                prependWeaponAnim = "Weapons/";
            }

            //Subtract the amount of damage taken from the health variable (First, check for teleporting projectiles with unique animations)
            if (this.gameObject != null && this.gameObject == monsterTarget && projectileSpeed == 1)
            {
                //Spawn in the animation object
                GameObject damageAnimObj = (GameObject)Instantiate(Resources.Load("Animations/" + prependWeaponAnim + damageType), gameObject.transform);
                damageAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yShiftAmt, gameObject.transform.position.z);
                damageAnimObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                //Subtract health after the animation plays
                StartCoroutine(SubtractHealth(incomingDamage, other, damageType, isCritHit, delayAmt));
            }

            //For certain types of projectiles, make sure this monster object is the target of the projectile (Check for moving projectiles)
            else if (this.gameObject != null && this.gameObject == monsterTarget && projectileSpeed > 1)
            {
                StartCoroutine(SubtractHealth(incomingDamage, other, damageType, isCritHit, 0));
            }

            //For towers without moving projectiles, we don't check for a target (Finally, check for AOE projectiles)
            else if (projectileSpeed == 0)
            {
                GameObject damageAnimObj = (GameObject)Instantiate(Resources.Load("Animations/" + prependWeaponAnim + damageType), gameObject.transform);
                damageAnimObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yShiftAmt, gameObject.transform.position.z);
                damageAnimObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                //Subtract health after the animation plays
                StartCoroutine(SubtractHealth(incomingDamage, other, damageType, isCritHit, delayAmt));
            }
        }
    }

    void ResetEffectAnims()
    {
        if (Time.time > iceSlowCd && iceSlowStatus)
        {
            iceSlowStatus = false;

            if (moveSpeed == 0.25f)
            {
                moveSpeed = reducedMoveSpeed + iceSlowAmt;
            }

            else
            {
                moveSpeed += iceSlowAmt;
            }
        }
    }

    void LimitMoveSpeedReduction()
    {
        if (iceSlowStatus && moveSpeed <= 0.25f)
        {
            reducedMoveSpeed = moveSpeed;
            moveSpeed = 0.25f;
        }
    }

    void ApplyHpRegen()
    {
        if (Time.time > hpRegenCd && hpRegen > 0)
        {
            if (health + hpRegen < maxHealth)
            {
                hpRegenCd = 0.8f + Time.time;
                health += hpRegen;
                healthText.SetText(health.ToString());
                StartCoroutine(SpawnHpRegenPopup(hpRegen, 0.25f));

                //Change health color when necessary 
                if (health <= maxHealth * 0.7f && health >= maxHealth * 0.35f)
                {
                    healthText.color = yellow;
                    maxHealthText.color = yellow;
                }
                else if (health < maxHealth * 0.35f)
                {
                    healthText.color = red;
                    maxHealthText.color = red;
                }
                else if (health > maxHealth * 0.7f)
                {
                    healthText.color = green;
                    maxHealthText.color = green;
                }
            }

            else if (health + hpRegen >= maxHealth)
            {
                hpRegenCd = 0.8f + Time.time;
                health = maxHealth;
                healthText.SetText(health.ToString());
                healthText.color = green;
                maxHealthText.color = green;
                StartCoroutine(SpawnHpRegenPopup(hpRegen, 0.25f));
            }
        }
    }

    void ApplySpawnBonuses()
    {
        evasionChance += GlobalVars.bonusKreepStats["BonusEvasion"];
        moveSpeed += GlobalVars.bonusKreepStats["BonusMoveSpeed"];
        hpRegen += GlobalVars.bonusKreepStats["BonusHpRegen"];
        armor += GlobalVars.bonusKreepStats["BonusArmor"];
        health += GlobalVars.bonusKreepStats["BonusMaxHealth"];
        maxHealth = health;
    }

    void SubtractPlayerHealth()
    {
        GlobalVars.newPlayerHealthValue -= damage;
    }

    IEnumerator DestroyMonster(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //After a delay, give gold to the player = to the "goldBounty" value
        GlobalVars.newGoldValue = GlobalVars.gold + goldBounty;
        Destroy(this.gameObject);

        //Get a random death sound to play
        GetSound("MonsterSounds", "Death");
    }

    IEnumerator inflictPoisonDamage(float delayTime)
    {
        isTakingPoisonDamage = true;
        StartCoroutine(SubtractHealth(GlobalVars.bonusExtraStats["SwiftPsnDmgUp"], null, "Swift", false, 0));
        yield return new WaitForSeconds(delayTime);
        isTakingPoisonDamage = false;
    }

    IEnumerator triggerPoison(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isPoisoned = true;
    }

    IEnumerator SubtractHealth(float incomingDamage, Collider2D projectileObj, string damageType, bool isCrit, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (this.gameObject != null)
        {
            float randomFloat = Random.value;

            if (incomingDamage <= 0)
            {
                StartCoroutine(SpawnDamagePopup(incomingDamage, damageType, isCrit, 0.25f));
                GetSound("DamageSounds", damageType);
            }

            //Make sure damage is greater than 0, and evasion does not occur
            else if (incomingDamage > 0 && randomFloat > evasionChance)
            {
                isTakingDamage = true;

                if (armor <= 0)
                {
                    health -= incomingDamage;
                    healthText.SetText(health.ToString());
                }
                else
                {
                    float tempHpDmg = 0;

                    if (armor - incomingDamage < 0)
                    {
                        tempHpDmg = System.Math.Abs(armor - incomingDamage);
                    }

                    armor -= incomingDamage;

                    if (armor <= 0)
                    {
                        armorText.SetText("");
                        ArmorObject.SetActive(false);

                        if (armor < 0)
                        {
                            health -= tempHpDmg;
                            healthText.SetText(health.ToString());
                        }
                    }
                    else
                    {
                        armorText.SetText(armor.ToString());
                    }
                }

                StartCoroutine(SpawnDamagePopup(incomingDamage, damageType, isCrit, 0.25f));
                GetSound("DamageSounds", damageType);
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
                //Adding a slight pause to make sure damage is received from the projectile
                yield return new WaitForSeconds(0.1f);
                if (projectileObj != null)
                {
                    Destroy(projectileObj.gameObject);
                }
            }

            //Change health color when necessary 
            if (health <= maxHealth * 0.7f && health >= maxHealth * 0.35f)
            {
                healthText.color = yellow;
                maxHealthText.color = yellow;
            }
            else if (health < maxHealth * 0.35f)
            {
                healthText.color = red;
                maxHealthText.color = red;
            }
        }
    }

    IEnumerator SpawnDamagePopup(float damageVal, string damageType, bool isCrit, float delayTime)
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
        damagePopupObj.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-0.05f, 0.05f), gameObject.transform.position.y + 0.25f, gameObject.transform.position.z);
        damagePopupObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (damageVal <= 0)
        {
            damagePopupObj.GetComponent<TextMeshPro>().text = "0";
        }

        else
        {
            if (isCrit)
            {
                damagePopupObj.GetComponent<RectTransform>().localScale = new Vector3(1.25f, 1.25f, 1);
                damagePopupObj.GetComponent<TextMeshPro>().text = damageVal.ToString();
            }
            else
            {
                damagePopupObj.GetComponent<TextMeshPro>().text = damageVal.ToString();
            }
        }

        //Destroy the damage popup after a short delay
        yield return new WaitForSeconds(delayTime);
        Destroy(damagePopupObj.gameObject);
    }

    IEnumerator SpawnHpRegenPopup(float regenVal, float delayTime)
    {

        //Spawn the Hp Regen popup
        GameObject regenPopupObj = (GameObject)Instantiate(Resources.Load("MonsterAttributes/" + "HpRegenPopup"), gameObject.transform);
        regenPopupObj.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-0.03f, 0.03f), gameObject.transform.position.y + 0.25f, gameObject.transform.position.z);
        regenPopupObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (regenVal > 0)
        {
            regenPopupObj.GetComponent<TextMeshPro>().text = "+" + regenVal.ToString();
        }

        //Destroy the hp regen popup after a short delay
        yield return new WaitForSeconds(delayTime);
        Destroy(regenPopupObj.gameObject);
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

    public void GetSound(string soundObj, string soundName)
    {
        int soundIndex = Random.Range(0, 3);
        var indexString = (soundIndex + 1).ToString();

        if (soundObj == "DamageSounds")
        {
            GameObject.Find(soundObj + indexString).GetComponent<AudioManager>().PlaySound(soundName);
            //Debug.Log("Random damage sound: " + indexString);
        }

        else if (soundObj == "MonsterSounds" && soundName == "Death")
        {
            GameObject.Find(soundObj).GetComponent<AudioManager>().PlaySound(soundName + indexString);
        }

        else
        {
            GameObject.Find(soundObj).GetComponent<AudioManager>().PlaySound(soundName);
        }
    }
}
