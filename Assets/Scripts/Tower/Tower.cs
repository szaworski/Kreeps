using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [HideInInspector] public GameObject currentTarget;
    private bool monsterIsInRadius;
    public bool GetMonsterIsInRadius
    {
        get { return monsterIsInRadius; }
    }
    public float attackCd;

    [Header("Tower attributes")]
    private float startingDamage;
    private float startingAttackRange;
    private float startingSpeed;
    private float startingCritChance;
    private float startingSlowAmt;
    private float bonusDamage;
    private float bonusSlowAmt;
    private float bonusRange;
    private float bonusSpeed;
    private float bonusCritChance;
    public float damage;
    public float bonusArmorDmg;
    public float projectileSpeed;
    public float attackSpeed;
    public float attackRange;
    public float critChance;
    public string damageType;
    public float slowAmt;
    public LineRenderer attackRadius;
    public bool hasRectangleRadius;
    public bool rectIsVertical;
    public bool triggerRadiusFlip;
    public bool isAoeWithProjectiles;

    [Header("Tower Stats")]
    public TMP_Text dmgText;
    public TMP_Text rofText;
    public TMP_Text rngText;

    [Header("Tower Upgrades")]
    public bool hasUpgrades;
    public string upgrade1;
    public string upgrade2;
    public string upgrade3;

    void Awake()
    {
        rectIsVertical = GlobalVars.selectedRectIsVertical;
        GetBonus();

        startingDamage = damage;
        startingSpeed = attackSpeed;
        startingAttackRange = attackRange;
        startingCritChance = critChance;
        startingSlowAmt = slowAmt;

        damage = startingDamage + bonusDamage;
        attackSpeed = startingSpeed + bonusSpeed;
        attackRange = startingAttackRange + bonusRange;
        critChance = startingCritChance + bonusCritChance;
        slowAmt = startingSlowAmt + bonusSlowAmt;
    }

    void Start()
    {
        DrawAttackRadius();

        //Set stats text
        dmgText.SetText(damage.ToString());
        rofText.SetText(attackSpeed.ToString() + "s");
        rngText.SetText(attackRange.ToString());
        dmgText.fontSize = 1.3f;
        rofText.fontSize = 1.3f;
        rngText.fontSize = 1.3f;
    }

    void Update()
    {
        if (triggerRadiusFlip)
        {
            DrawAttackRadius();
            triggerRadiusFlip = false;
        }

        GetBonus();
        AddBonus();
        CheckTowerRadius();
    }

    public void CheckTowerRadius()
    {
        Collider2D[] monstersInRadius;

        if (hasRectangleRadius)
        {
            //Vertical rectangle
            if (rectIsVertical)
            {
                monstersInRadius = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 0.375f, transform.position.y + attackRange * 1.5f), new Vector2(transform.position.x + 0.375f, transform.position.y - attackRange * 1.5f), LayerMask.GetMask("Monster"));
            }
            else
            {
                monstersInRadius = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - attackRange * 1.5f, transform.position.y + 0.375f), new Vector2(transform.position.x + attackRange * 1.5f, transform.position.y - 0.375f), LayerMask.GetMask("Monster"));
            }
        }

        else
        {
            monstersInRadius = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - attackRange, transform.position.y + attackRange), new Vector2(transform.position.x + attackRange, transform.position.y - attackRange), LayerMask.GetMask("Monster"));
        }

        //Check if any monsters are found in the radius (Use this for towers that shoot projectiles)
        if (monstersInRadius.Length >= 1)
        {
            monsterIsInRadius = true;

            //Get the distance that the first mosnter has traveled
            float farthestTraveledSoFar = monstersInRadius[0].gameObject.GetComponent<Monster>().distanceTraveled;
            //Set the first monster as the current target
            GameObject currentTarget = monstersInRadius[0].gameObject;

            //Loop through for each object found in the radius
            for (int i = 0; i < monstersInRadius.Length; i++)
            {
                if (!isAoeWithProjectiles)
                {
                    //Check the distance that each monster in the radius has traveled
                    float travelDistance = monstersInRadius[i].gameObject.GetComponent<Monster>().distanceTraveled;

                    //Check the distance that each monster has traveled so that we can target the object with the most distance
                    if (travelDistance > farthestTraveledSoFar)
                    {
                        //Set the new target if one object gains more distance than the current target
                        currentTarget = monstersInRadius[i].gameObject;
                        farthestTraveledSoFar = travelDistance;
                    }
                }
                else
                {
                    CreateAoeProjectiles(monstersInRadius[i].gameObject, monstersInRadius.Length, i);
                }
            }
            //Spawn the projectile to send at the target
            CreateProjectile(currentTarget);
        }

        //Check if any monsters are found in the radius
        else if (monstersInRadius.Length >= 1)
        {
            monsterIsInRadius = true;
        }

        else
        {
            monsterIsInRadius = false;
        }
    }

    public void CreateProjectile(GameObject target)
    {
        if (Time.time > attackCd && target != null && monsterIsInRadius)
        {
            string prependProjectileName = "";

            if (projectileSpeed > 1)
            {
                prependProjectileName = "Towers/Projectiles/";
            }

            else
            {
                prependProjectileName = "Towers/Projectiles/WithAnim/";
            }

            currentTarget = target;
            GameObject projectile = (GameObject)Instantiate(Resources.Load(prependProjectileName + damageType), this.transform);
            projectile.transform.position = this.transform.position;
            attackCd = attackSpeed + Time.time;
        }
    }

    public void CreateAoeProjectiles(GameObject target, int numOfMonsters, int currentNum)
    {
        if (Time.time > attackCd && target != null && monsterIsInRadius)
        {
            string prependProjectileName = "";

            if (projectileSpeed > 1)
            {
                prependProjectileName = "Towers/Projectiles/";
            }

            else
            {
                prependProjectileName = "Towers/Projectiles/WithAnim/";
            }

            currentTarget = target;
            GameObject projectile = (GameObject)Instantiate(Resources.Load(prependProjectileName + damageType), this.transform);
            projectile.transform.position = this.transform.position;

            if (numOfMonsters - 1 == currentNum)
            {
                attackCd = attackSpeed + Time.time;
            }
        }
    }

    void DrawAttackRadius()
    {
        attackRadius.widthMultiplier = 0.0075f;
        attackRadius.positionCount = 4;

        if (hasRectangleRadius)
        {
            if (rectIsVertical)
            {
                attackRadius.numCornerVertices = 5;
                attackRadius.SetPosition(0, new Vector3(transform.position.x - 0.375f, transform.position.y + attackRange * 1.5f));
                attackRadius.SetPosition(1, new Vector3(transform.position.x + 0.375f, transform.position.y + attackRange * 1.5f));
                attackRadius.SetPosition(2, new Vector3(transform.position.x + 0.375f, transform.position.y - attackRange * 1.5f));
                attackRadius.SetPosition(3, new Vector3(transform.position.x - 0.375f, transform.position.y - attackRange * 1.5f));
            }
            else
            {
                attackRadius.numCornerVertices = 5;
                attackRadius.SetPosition(0, new Vector3(transform.position.x - attackRange * 1.5f, transform.position.y + 0.375f));
                attackRadius.SetPosition(1, new Vector3(transform.position.x + attackRange * 1.5f, transform.position.y + 0.375f));
                attackRadius.SetPosition(2, new Vector3(transform.position.x + attackRange * 1.5f, transform.position.y - 0.375f));
                attackRadius.SetPosition(3, new Vector3(transform.position.x - attackRange * 1.5f, transform.position.y - 0.375f));
            }
        }

        else
        {
            attackRadius.numCornerVertices = 5;
            attackRadius.SetPosition(0, new Vector3(transform.position.x - attackRange, transform.position.y + attackRange));
            attackRadius.SetPosition(1, new Vector3(transform.position.x + attackRange, transform.position.y + attackRange));
            attackRadius.SetPosition(2, new Vector3(transform.position.x + attackRange, transform.position.y - attackRange));
            attackRadius.SetPosition(3, new Vector3(transform.position.x - attackRange, transform.position.y - attackRange));
        }
    }

    public void GetBonus()
    {
        string trimmedDamageType = Regex.Replace(damageType, @"[\d-]", string.Empty);

        if (bonusDamage < GlobalVars.bonusStats[trimmedDamageType])
        {
            bonusDamage = GlobalVars.bonusStats[trimmedDamageType];
        }

        switch (trimmedDamageType)
        {
            case "Fire":
                if (bonusSpeed < GlobalVars.bonusExtraStats["FireSpeedUp"])
                {
                    bonusSpeed = GlobalVars.bonusExtraStats["FireSpeedUp"];
                }
                break;

            case "Ice":
                if (bonusSlowAmt < GlobalVars.bonusExtraStats["IceSlowUp"])
                {
                    bonusSlowAmt = GlobalVars.bonusExtraStats["IceSlowUp"];
                }
                break;

            case "Thunder":
                if (bonusArmorDmg < GlobalVars.bonusExtraStats["ThunderArmorDmgUp"])
                {
                    bonusArmorDmg = GlobalVars.bonusExtraStats["ThunderArmorDmgUp"];
                }
                break;

            case "Holy":
                if (bonusRange < GlobalVars.bonusExtraStats["HolyRangeUp"])
                {
                    bonusRange = GlobalVars.bonusExtraStats["HolyRangeUp"];
                }
                break;

            case "Cosmic":
                if (bonusCritChance < GlobalVars.bonusExtraStats["CosmicCritChanceUp"])
                {
                    bonusCritChance = GlobalVars.bonusExtraStats["CosmicCritChanceUp"];
                }
                break;
        }
    }

    public void AddBonus()
    {
        if (damage != startingDamage + bonusDamage)
        {
            damage = startingDamage + bonusDamage;
            dmgText.SetText(damage.ToString());
        }

        if (attackRange != startingAttackRange + bonusRange)
        {
            attackRange = startingAttackRange + bonusRange;
            rngText.SetText(attackRange.ToString());
            DrawAttackRadius();
        }

        if (attackSpeed != startingSpeed - bonusSpeed)
        {
            attackSpeed = startingSpeed - bonusSpeed;
            rofText.SetText(attackSpeed.ToString() + "s");
        }

        if (critChance != startingCritChance + bonusCritChance)
        {
            critChance = startingCritChance + bonusCritChance;
        }

        if (slowAmt != startingSlowAmt + bonusSlowAmt)
        {
            slowAmt = startingSlowAmt + bonusSlowAmt;
        }
    }

    public void OnDrawGizmos()
    {
        // Draw a cirlce at the towers position
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, 0.6f);
    }
}
