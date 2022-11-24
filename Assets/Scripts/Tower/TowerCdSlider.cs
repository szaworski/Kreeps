using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerCdSlider : MonoBehaviour
{
    public Slider cdSlider;
    public GameObject tower;
    Tower towerScript;

    void Awake()
    {
        towerScript = tower.GetComponent<Tower>();
        cdSlider.value = towerScript.attackSpeed;
    }

    void Update()
    {
        ChangeSliderValue();
    }

    public void ChangeSliderValue()
    {
        cdSlider.maxValue = towerScript.attackSpeed;
        cdSlider.value += 1 * Time.deltaTime;

        if (cdSlider.value == cdSlider.maxValue && towerScript.GetMonsterIsInRadius)
        {
            cdSlider.value = 0;
        }
    }
}
