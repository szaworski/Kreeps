using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponAttackRadius;
    [SerializeField] private GameObject weaponCdSlider;
    [SerializeField] private Vector3 mouseScreenPosition;
    [SerializeField] private Vector3 mouseWorldPosition;
    private bool monsterIsInRadius;
    public float attackCd;
    public bool GetMonsterIsInRadius
    {
        get { return monsterIsInRadius; }
    }
    public float GetSetAttackCd
    {
        get { return attackCd; }
        set { attackCd = value; }
    }

    public GameObject currentTarget;
    public new Camera camera;

    [Header("Weapon attributes")]
    public float[] startingStats = new float[] {0, 0, 0, 0, 0 };
    public float[] bonusStats = new float[] { 0, 0, 0, 0, 0 };
    public float damage;
    public float projectileSpeed;
    public float attackSpeed;
    public float attackRange;
    public float critChance;
    public float slowAmt;
    public string damageType;
    public LineRenderer attackRadius;

    void Awake()
    {
        damageType = "Neutral";
        weaponAttackRadius = this.transform.GetChild(0).gameObject;
        weaponCdSlider = this.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        GetBonus();
        AddBonus();
        DrawAttackRadius();
        //Follow the mouse cursor with this object
        mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, camera.nearClipPlane));
        this.transform.position = mouseWorldPosition;

        if (GlobalVars.weaponIsSelected)
        {
            CheckAttackRadius();
            weaponCdSlider.SetActive(true);

            if (GameObject.Find("TileManager").transform.childCount == 0)
            {
                weaponAttackRadius.SetActive(false);
                weaponCdSlider.SetActive(false);
            }

            else if (!weaponAttackRadius.activeInHierarchy && Input.GetMouseButtonDown(1))
            {
                weaponAttackRadius.SetActive(true);
            }
            else if (weaponAttackRadius.activeInHierarchy && Input.GetMouseButtonDown(1))
            {
                weaponAttackRadius.SetActive(false);
            }
        }

        else if (!GlobalVars.weaponIsSelected)
        {
            weaponAttackRadius.SetActive(false);
            weaponCdSlider.SetActive(false);
        }
    }

    public void CheckAttackRadius()
    {
        Collider2D[] monstersInRadius = Physics2D.OverlapCircleAll(this.transform.position, attackRange, LayerMask.GetMask("Monster"));

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
        {
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
    }

    public void CreateProjectile(GameObject target)
    {
        if (Time.time > attackCd && target != null && monsterIsInRadius)
        {
            string prependProjectileName = "";

            if (projectileSpeed > 1)
            {
                prependProjectileName = "Weapons/Projectiles/";
            }

            else
            {
                prependProjectileName = "Weapons/Projectiles/WithAnim/";
            }

            currentTarget = target;

            if (projectileSpeed <= 1)
            {
                GameObject projectile = (GameObject)Instantiate(Resources.Load(prependProjectileName + damageType), this.transform);
                projectile.transform.position = this.transform.position;
                attackCd = attackSpeed + Time.time;
            }

            else if (projectileSpeed > 1)
            {
                GameObject projectile = (GameObject)Instantiate(Resources.Load(prependProjectileName + damageType), GameObject.Find("WeaponProjectiles").transform);
                projectile.transform.position = this.transform.position;
                attackCd = attackSpeed + Time.time;
            }
        }
    }

    public void CreateAoeDamageRadius()
    {
        if (Time.time > attackCd && monsterIsInRadius)
        {
            GameObject aoeRadius = (GameObject)Instantiate(Resources.Load("Weapons/Projectiles/Aoe/" + damageType), this.transform);
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
        if (bonusStats[0] < GlobalVars.bonusStats[damageType])
        {
            bonusStats[0] = GlobalVars.bonusStats[damageType];
        }

        switch (damageType)
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
                if (bonusStats[1] < GlobalVars.bonusExtraStats["ThunderSpeedUp"])
                {
                    bonusStats[1] = GlobalVars.bonusExtraStats["ThunderSpeedUp"];
                }
                break;

            case "Holy":
                if (bonusStats[2] < GlobalVars.bonusExtraStats["HolyRangeUp"])
                {
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
        if (damage != startingStats[0] + bonusStats[0])
        {
            damage = startingStats[0] + bonusStats[0];
        }

        if (attackSpeed != startingStats[1] - bonusStats[1])
        {
            attackSpeed = startingStats[1] - bonusStats[1];
        }

        if (attackRange != startingStats[2] + bonusStats[2])
        {
            attackRange = startingStats[2] + bonusStats[2];
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
}
