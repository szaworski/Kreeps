using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [HideInInspector] public GameObject currentTarget;
    private bool monsterIsInRadius;
    public float attackCd;

    [Header("Tower attributes")]
    private float startingDamage;
    private float startingAttackRange;
    private float startingSpeed;
    private float startingCritChance;
    private float bonusDamage;
    private float bonusRange;
    private float bonusSpeed;
    private float bonusCritChance;
    public float damage;
    public float projectileSpeed;
    public float attackSpeed;
    public float attackRange;
    public float critChance;
    public string damageType;
    public float slowAmt;
    public LineRenderer attackRadius;

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

        startingDamage = damage;
        startingSpeed = attackSpeed;
        startingAttackRange = attackRange;
        startingCritChance = critChance;

        damage = startingDamage + bonusDamage;
        attackSpeed = startingSpeed + bonusSpeed;
        attackRange = startingAttackRange + bonusRange;
        critChance = startingCritChance + bonusCritChance;
    }

    void Start()
    {
        DrawAttackRadius();

        //Set stats text
        dmgText.SetText(damage.ToString());
        rofText.SetText(attackSpeed.ToString());
        rngText.SetText(attackRange.ToString());
        dmgText.fontSize = 1.3f;
        rofText.fontSize = 1.3f;
        rngText.fontSize = 1.3f;
    }

    void Update()
    {
        GetBonus();
        AddBonus();
        CheckTowerRadius();
    }

    public void CheckTowerRadius()
    {
        Collider2D[] monstersInRadius = Physics2D.OverlapCircleAll(this.transform.position, attackRange, LayerMask.GetMask("Monster"));

        //Check if any monsters are found in the radius (Use this for towers that shoot projectiles)
        if (monstersInRadius.Length >= 1 && projectileSpeed >= 1)
        {
            monsterIsInRadius = true;

            //Get the distance that the first mosnter has traveled
            float farthestTraveledSoFar = monstersInRadius[0].gameObject.GetComponent<Monster>().distanceTraveled;
            //Set the first monster as the current target
            GameObject currentTarget = monstersInRadius[0].gameObject;

            //Loop through for each object found in the radius
            for (int i = 0; i < monstersInRadius.Length; i++)
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
            //Spawn the projectile to send at the target
            CreateProjectile(currentTarget);
        }

        //Check if any monsters are found in the radius (Use this for towers that deal AOE damage in an area)
        else if (monstersInRadius.Length >= 1 && projectileSpeed == 0)
        {
            monsterIsInRadius = true;
            CreateAoeDamageRadius();
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

    public void CreateAoeDamageRadius()
    {
        if (Time.time > attackCd && monsterIsInRadius)
        {
            GameObject aoeRadius = (GameObject)Instantiate(Resources.Load("Towers/Projectiles/Aoe/" + damageType), this.transform);
            aoeRadius.transform.position = this.transform.position;
            attackCd = attackSpeed + Time.time;
        }
    }

    void DrawAttackRadius()
    {
        attackRadius.widthMultiplier = 0.01f;
        attackRadius.positionCount = 40;

        float deltaTheta = (2f * Mathf.PI) / 40;
        float theta = 0f;

        for (int i = 0; i < attackRadius.positionCount; i++)
        {
            Vector3 pos = new Vector3(attackRange * Mathf.Cos(theta), attackRange * Mathf.Sin(theta), 0f);
            attackRadius.SetPosition(i, transform.position + pos);
            theta += deltaTheta;
        }
    }

    public void GetBonus()
    {
        string trimmedDamageType = Regex.Replace(damageType, @"[\d-]", string.Empty);

        if (bonusDamage < GlobalVars.bonusStats[trimmedDamageType])
        {
            bonusDamage = GlobalVars.bonusStats[trimmedDamageType];
        }

        if (trimmedDamageType == "Ice" || trimmedDamageType == "Swift")
        {
            if (bonusRange < GlobalVars.bonusExtraStats[trimmedDamageType + "RangeUp"])
            {
                bonusRange = GlobalVars.bonusExtraStats[trimmedDamageType + "RangeUp"];
            }
        }

        if (trimmedDamageType == "Fire" || trimmedDamageType == "Holy")
        {
            if (bonusSpeed < GlobalVars.bonusExtraStats[trimmedDamageType + "SpeedUp"])
            {
                bonusSpeed = GlobalVars.bonusExtraStats[trimmedDamageType + "SpeedUp"];
            }
        }

        if (trimmedDamageType == "Thunder" || trimmedDamageType == "Cosmic")
        {
            if (bonusCritChance < GlobalVars.bonusExtraStats[trimmedDamageType + "CritChanceUp"])
            {
                bonusCritChance = GlobalVars.bonusExtraStats[trimmedDamageType + "CritChanceUp"];
            }
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
            rofText.SetText(attackSpeed.ToString());
        }

        if (critChance != startingCritChance + bonusCritChance)
        {
            critChance = startingCritChance + bonusCritChance;
        }
    }

    public void OnDrawGizmos()
    {
        // Draw a cirlce at the towers position
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, 0.6f);
    }
}
