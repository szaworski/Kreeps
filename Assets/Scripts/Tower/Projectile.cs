using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageValue;
    public GameObject target;

    void Awake()
    {
        damageValue = transform.parent.GetComponent<Tower>().damage;
        target = transform.parent.GetComponent<Tower>().currentTarget;
        //Debug.Log("currentTarget target position: " + target.transform.position);
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, 3 * Time.deltaTime);
            //The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
        }
    }
}
