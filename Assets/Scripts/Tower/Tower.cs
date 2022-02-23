﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower attributes")]
    public int damage;
    public int lvl;
    public float attackSpeed;
    public float attackRange;
    public string type;
    public bool monsterIsInRadius;

    void Update()
    {
        CheckTowerRadius();
    }

    public void CheckTowerRadius()
    {
        Vector2 towerPos = transform.position;
        Collider2D[] monstersInRadius = Physics2D.OverlapCircleAll(towerPos, attackRange, LayerMask.GetMask("Monster"));

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
                //Check the distance of each objects found in the radius
                float currentDistance = Vector2.Distance(this.gameObject.transform.position, monstersInRadius[i].gameObject.transform.position);

                //Check the distance of new objects found in the monster array to see if they're closer than the previously closest one found
                if (currentDistance < closestTargetSoFar)
                {
                    //Set the new closest object if one is found
                    closestTarget = monstersInRadius[i].gameObject;
                    closestTargetSoFar = currentDistance;
                }
            }

            //Spawn and send the projectile at the closest target (In progress)

        }

        else
        {
            monsterIsInRadius = false;
        }
    }

    public void CreateAndShootProjectile()
    {


    }

    public void OnDrawGizmos()
    {
        // Draw a cirlce at the towers position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
