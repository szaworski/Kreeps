﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectAnims : MonoBehaviour
{
    public float timeToDelay;
    public bool isBeingDestroyed;

    void Start()
    {
        if (!isBeingDestroyed)
        {
            isBeingDestroyed = true;
            StartCoroutine(DestroyAnimObject(timeToDelay));
        }
    }

    IEnumerator DestroyAnimObject(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Destroy Anim Object");
        Destroy(this.gameObject);
    }
}
