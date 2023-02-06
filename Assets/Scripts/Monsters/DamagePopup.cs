using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private bool hasRandXpos;
    private float xPosShift;

    void Update()
    {
        if (!hasRandXpos)
        {
            getRandxPosTarget();
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x + xPosShift, this.transform.position.y + 0.05f, 0), 0.5f * Time.deltaTime);
    }

    public void getRandxPosTarget()
    {
        xPosShift = Random.Range(-0.025f, 0.025f);
        hasRandXpos = true;
    }
}
