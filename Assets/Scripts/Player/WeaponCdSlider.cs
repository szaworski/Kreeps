using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCdSlider : MonoBehaviour
{
    [SerializeField] private Vector3 mouseScreenPosition;
    [SerializeField] private Vector3 mouseWorldPosition;
    private int xOffset;
    private int yOffest;
    public new Camera camera;
    public Slider cdSlider;

    GameObject weapon;
    Weapon weaponScript;

    void Awake()
    {
        weapon = GameObject.Find("PlayerWeapon");
        weaponScript = weapon.GetComponent<Weapon>();
        xOffset = 25;
        yOffest = 20;
    }

    void OnEnable()
    {
        cdSlider.value = 0;
        weaponScript.GetSetAttackCd = weaponScript.attackSpeed + Time.time;
        //Follow the mouse cursor with this object (Set the position during OnEnable so we don't see any jittering)
        mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x + xOffset, mouseScreenPosition.y + yOffest, camera.nearClipPlane));
        this.transform.position = mouseWorldPosition;
    }

    void Update()
    {
        //Follow the mouse cursor with this object
        mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x + xOffset, mouseScreenPosition.y + yOffest, camera.nearClipPlane));
        this.transform.position = mouseWorldPosition;

        ChangeSliderValue();
    }

    public void ChangeSliderValue()
    {
        cdSlider.maxValue = weaponScript.attackSpeed;
        cdSlider.value += 1 * Time.deltaTime;

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && GlobalVars.weaponIsSelected && cdSlider.value == cdSlider.maxValue && weaponScript.GetMonsterIsInRadius)
        {
            cdSlider.value = 0;
        }
    }
}
