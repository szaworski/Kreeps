using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower attributes")]
    [HideInInspector] public float attackCd;
    [HideInInspector] public GameObject currentTarget;
    [SerializeField] private bool monsterIsInRadius;
    public int damage;
    public int towerLvl;
    public float attackSpeed;
    public float attackRange;
    public string damageType;

    void Update()
    {
        CheckTowerRadius();
    }

    public void CheckTowerRadius()
    {
        Collider2D[] monstersInRadius = Physics2D.OverlapCircleAll(this.transform.position, attackRange, LayerMask.GetMask("Monster"));

        //Check if any monsters are found in the radius
        if (monstersInRadius.Length >= 1)
        {
            monsterIsInRadius = true;

            //Check the distance between the tower and the first monster
            float closestTargetSoFar = Vector2.Distance(this.gameObject.transform.position, monstersInRadius[0].gameObject.transform.position);
            GameObject closestTarget = monstersInRadius[0].gameObject;

            //Loop through for each object found in the radius
            for (int i = 0; i < monstersInRadius.Length; i++)
            {
                //Check the distance of each object found in the radius
                float currentDistance = Vector2.Distance(this.gameObject.transform.position, monstersInRadius[i].gameObject.transform.position);

                //Check the distance of new objects found in the monster array to see if they're closer than the previously closest one found
                if (currentDistance < closestTargetSoFar)
                {
                    //Set the new closest object if one is found
                    closestTarget = monstersInRadius[i].gameObject;
                    closestTargetSoFar = currentDistance;
                }
            }
            //Spawn the projectile to send at the target
            CreateProjectile(closestTarget);
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
            currentTarget = target;
            GameObject projectile = (GameObject)Instantiate(Resources.Load("Towers/Projectiles/TowerProjectile1"), this.transform);
            projectile.transform.position = this.transform.position;
            attackCd = attackSpeed + Time.time;
        }
    }

    public void OnDrawGizmos()
    {
        // Draw a cirlce at the towers position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.75f);
    }
}
