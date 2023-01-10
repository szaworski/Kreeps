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
    private float[] startingStats = new float[5];
    private float[] bonusStats = new float[5];
    public float damage;
    public float projectileSpeed;
    public float attackSpeed;
    public float attackRange;
    public float critChance;
    public string damageType;
    public float slowAmt;
    public float bonusDamage2;
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
        GetBonus();
        startingStats = new float[] { damage, attackSpeed, attackRange, critChance, slowAmt };

        damage = startingStats[0] + bonusStats[0];
        attackSpeed = startingStats[1] + bonusStats[1];
        attackRange = startingStats[2] + bonusStats[2];
        critChance = startingStats[3] + bonusStats[3];
        slowAmt = startingStats[4] + bonusStats[4];

        rectIsVertical = GlobalVars.selectedRectIsVertical;
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
                monstersInRadius = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 0.4f, transform.position.y + attackRange * 1.5f), new Vector2(transform.position.x + 0.4f, transform.position.y - attackRange * 1.5f), LayerMask.GetMask("Monster"));
            }
            else
            {
                monstersInRadius = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - attackRange * 1.5f, transform.position.y + 0.4f), new Vector2(transform.position.x + attackRange * 1.5f, transform.position.y - 0.4f), LayerMask.GetMask("Monster"));
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
        attackRadius.sortingOrder = 2;
        attackRadius.widthMultiplier = 0.0075f;
        attackRadius.positionCount = 4;

        if (hasRectangleRadius)
        {
            if (rectIsVertical)
            {
                attackRadius.numCornerVertices = 5;
                attackRadius.SetPosition(0, new Vector3(transform.position.x - 0.4f, transform.position.y + attackRange * 1.5f));
                attackRadius.SetPosition(1, new Vector3(transform.position.x + 0.4f, transform.position.y + attackRange * 1.5f));
                attackRadius.SetPosition(2, new Vector3(transform.position.x + 0.4f, transform.position.y - attackRange * 1.5f));
                attackRadius.SetPosition(3, new Vector3(transform.position.x - 0.4f, transform.position.y - attackRange * 1.5f));
            }
            else
            {
                attackRadius.numCornerVertices = 5;
                attackRadius.SetPosition(0, new Vector3(transform.position.x - attackRange * 1.5f, transform.position.y + 0.4f));
                attackRadius.SetPosition(1, new Vector3(transform.position.x + attackRange * 1.5f, transform.position.y + 0.4f));
                attackRadius.SetPosition(2, new Vector3(transform.position.x + attackRange * 1.5f, transform.position.y - 0.4f));
                attackRadius.SetPosition(3, new Vector3(transform.position.x - attackRange * 1.5f, transform.position.y - 0.4f));
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

        if (bonusStats[0] < GlobalVars.bonusStats[trimmedDamageType])
        {
            bonusStats[0] = GlobalVars.bonusStats[trimmedDamageType];
        }

        switch (trimmedDamageType)
        {
            case "Fire":
                if (bonusStats[1] < GlobalVars.bonusExtraStats["FireSpeedUp"])
                {
                    bonusStats[1] = GlobalVars.bonusExtraStats["FireSpeedUp"];
                }
                break;

            case "Ice":
                if (bonusStats[2] < GlobalVars.bonusExtraStats["IceRangeUp"] || bonusStats[4] < GlobalVars.bonusExtraStats["IceSlowUp"])
                {
                    bonusStats[2] = GlobalVars.bonusExtraStats["IceRangeUp"];
                    bonusStats[4] = GlobalVars.bonusExtraStats["IceSlowUp"];
                }
                break;

            case "Thunder":
                if (bonusStats[3] < GlobalVars.bonusExtraStats["ThunderCritChanceUp"])
                {
                    bonusStats[3] = GlobalVars.bonusExtraStats["ThunderCritChanceUp"];
                }
                break;

            case "Holy":
                if (bonusStats[1] < GlobalVars.bonusExtraStats["HolySpeedUp"] || bonusStats[2] < GlobalVars.bonusExtraStats["HolyRangeUp"])
                {
                    bonusStats[1] = GlobalVars.bonusExtraStats["HolySpeedUp"];
                    bonusStats[2] = GlobalVars.bonusExtraStats["HolyRangeUp"];
                }
                break;

            case "Swift":
                if (bonusStats[1] < GlobalVars.bonusExtraStats["SwiftSpeedUp"])
                {
                    bonusStats[1] = GlobalVars.bonusExtraStats["SwiftSpeedUp"];
                }
                break;

            case "Cosmic":
                if (bonusStats[2] < GlobalVars.bonusExtraStats["CosmicRangeUp"] || bonusStats[3] < GlobalVars.bonusExtraStats["CosmicCritChanceUp"])
                {
                    bonusStats[2] = GlobalVars.bonusExtraStats["CosmicRangeUp"];
                    bonusStats[3] = GlobalVars.bonusExtraStats["CosmicCritChanceUp"];
                }
                break;
        }
    }

    public void AddBonus()
    {
        if (damage != startingStats[0] + bonusStats[0] + bonusDamage2)
        {
            damage = startingStats[0] + bonusStats[0] + bonusDamage2;
            dmgText.SetText(Mathf.Round(damage).ToString());
        }

        if (attackSpeed != startingStats[1] - bonusStats[1])
        {
            attackSpeed = startingStats[1] - bonusStats[1];
            rofText.SetText(attackSpeed.ToString("F1") + "s");
        }

        if (attackRange != startingStats[2] + bonusStats[2])
        {
            attackRange = startingStats[2] + bonusStats[2];
            rngText.SetText(attackRange.ToString("F1"));
            DrawAttackRadius();
        }

        if (critChance != startingStats[3] + bonusStats[3])
        {
            critChance = startingStats[3] + bonusStats[3];
        }

        if (slowAmt != startingStats[4] + bonusStats[4])
        {
            slowAmt = startingStats[4] + bonusStats[4];
        }
    }

    public void OnDrawGizmos()
    {
        // Draw a cirlce at the towers position
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, 0.6f);
    }
}
