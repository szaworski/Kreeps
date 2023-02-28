using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPopup : MonoBehaviour
{
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + 0.05f, 0), 0.5f * Time.deltaTime);
        Destroy(gameObject, 0.25f);
    }
}
