using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public CircleCollider2D AoeRadius;
    public float damageValue;
    public float projectileSpeed;
    public float attackRange;
    public string damageType;
    public GameObject target;

    void Awake()
    {
        damageValue = transform.parent.GetComponent<Tower>().damage;
        projectileSpeed = transform.parent.GetComponent<Tower>().projectileSpeed;
        damageType = transform.parent.GetComponent<Tower>().damageType;
        attackRange = transform.parent.GetComponent<Tower>().attackRange;
        target = transform.parent.GetComponent<Tower>().currentTarget;

        if (projectileSpeed == 0)
        {
            AoeRadius.radius = attackRange;
        }
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
                if (damageType == "Thunder" || damageType == "Holy")
                {
                    //Teleport the projectile on top of the target
                    transform.position = target.transform.position;
                }

                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, projectileSpeed * Time.deltaTime);
                }
            }

            else
            {
                //If the target is not null, The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
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
