﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageValue;
    public float projectileSpeed;
    public string damageType;
    public GameObject target;

    void Awake()
    {
        damageValue = transform.parent.GetComponent<Tower>().damage;
        projectileSpeed = transform.parent.GetComponent<Tower>().projectileSpeed;
        damageType = transform.parent.GetComponent<Tower>().damageType;
        target = transform.parent.GetComponent<Tower>().currentTarget;
        //Debug.Log("currentTarget target position: " + target.transform.position);
    }

    void Update()
    {
        MoveProjectile();
    }

    public void MoveProjectile()
    {
        if (projectileSpeed >= 1)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, projectileSpeed * Time.deltaTime);
            }

            else
            {
                //The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
                destroyProjectile();
            }
        }

        else if (projectileSpeed == 0)
        {
            //The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
            //Need to use invoke to give time for the damage to be dealt
            Invoke("destroyProjectile", 0.1f);
        }
    }

    public void destroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
