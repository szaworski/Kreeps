using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
     void Awake()
    {
        target = transform.parent.GetComponent<Tower>().currentTarget;
        Debug.Log("currentTarget target position: " + target.transform.position);
    }

    void Update()
    {
       transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, 3 * Time.deltaTime);

        //Destory the projectile on collition with the target. Still need to send damage values to the monster (In progress)
        if (transform.position == target.transform.position)
        {
            Destroy(this.gameObject);
        }
    }
}
