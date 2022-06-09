using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCdSlider : MonoBehaviour
{
    [SerializeField] private Vector3 mouseScreenPosition;
    [SerializeField] private Vector3 mouseWorldPosition;
    [SerializeField] private bool startingSliderValue;

    public new Camera camera;
    public Slider cdSlider;

    GameObject weapon;
    Weapon weaponScript;

    void Awake()
    {
        weapon = GameObject.Find("PlayerWeapon");
        weaponScript = weapon.GetComponent<Weapon>();
    }

    void OnEnable ()
    {
        startingSliderValue = true;
    }

    void Update()
    {
        //Follow the mouse cursor with this object
        mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x + 75, mouseScreenPosition.y - 25, camera.nearClipPlane));
        this.transform.position = mouseWorldPosition;

        ChangeSliderValue();
    }

    public void ChangeSliderValue()
    {
        cdSlider.maxValue = weaponScript.attackSpeed;

        if (startingSliderValue)
        {
            cdSlider.value = weaponScript.attackSpeed;
        }

        else if (!startingSliderValue)
        {
            cdSlider.value += 1 * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && MouseCursor.weaponIsSelected && cdSlider.value == cdSlider.maxValue && weaponScript.GetMonsterIsInRadius)
        {
            cdSlider.value = 0;
            startingSliderValue = false;
        }

        if (GameObject.Find("TileManager").transform.childCount == 0)
        {
            cdSlider.value = weaponScript.attackSpeed;
            startingSliderValue = true;
        }
    }
}
