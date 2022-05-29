using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponAttackRadius;
    [SerializeField] private Vector3 mouseScreenPosition;
    [SerializeField] private Vector3 mouseWorldPosition;
    private bool monsterIsInRadius;
    private float attackCd;
    public GameObject currentTarget;
    public new Camera camera;

    [Header("Weapon attributes")]
    public float damage;
    public float projectileSpeed;
    public float attackSpeed;
    public float attackRange;
    public string damageType;
    public float slowAmt;
    public LineRenderer attackRadius;

    void Awake()
    {
        weaponAttackRadius = this.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        DrawAttackRadius();
        //Follow the mouse cursor with this object
        mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, camera.nearClipPlane));
        this.transform.position = mouseWorldPosition;

        if (MouseCursor.weaponIsSelected)
        {
            CheckAttackRadius();
            weaponAttackRadius.SetActive(true);

        }

        else if (!MouseCursor.weaponIsSelected && weaponAttackRadius.activeInHierarchy)
        {
            weaponAttackRadius.SetActive(false);
        }
    }

    public void CheckAttackRadius()
    {
        Collider2D[] monstersInRadius = Physics2D.OverlapCircleAll(this.transform.position, attackRange, LayerMask.GetMask("Monster"));

        if (Input.GetMouseButtonDown(0))
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
            GameObject projectile = (GameObject)Instantiate(Resources.Load(prependProjectileName + damageType), this.transform);
            projectile.transform.position = this.transform.position;
            attackCd = attackSpeed + Time.time;
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
}
