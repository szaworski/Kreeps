using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private int weaponImageIndex;
    [SerializeField] private string currentWeapon;
    [SerializeField] private string newWeapon;
    [SerializeField] private Texture2D cursorImage;
    [SerializeField] private Texture2D[] weaponImages;
    public static bool weaponIsSelected;
    public string GetSetCurrentWeapon
    {
        get { return currentWeapon; }
        set { currentWeapon = value; }
    }
    public string GetSetNewWeapon
    {
        get { return newWeapon; }
        set { newWeapon = value; }
    }

    GameObject playerWeapon;
    GameObject playerHud;
    Weapon weaponScript;
    PlayerHud playerHudScript;

    void Awake()
    {
        playerWeapon = GameObject.Find("PlayerWeapon");
        playerHud = GameObject.Find("PlayerHud");
        weaponScript = playerWeapon.GetComponent<Weapon>();
        playerHudScript = playerHud.GetComponent<PlayerHud>();
        newWeapon = "";

        weaponIsSelected = false;
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        SetNewWeapon();
        SwapMouseCursor();
    }

    public void SwapMouseCursor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || PauseMenuButtons.isPaused)
        {
            weaponIsSelected = false;
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != "" && !PauseMenuButtons.isPaused)
        {
            weaponIsSelected = true;
            Cursor.SetCursor(weaponImages[weaponImageIndex], Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void SetNewWeapon()
    {
        if (currentWeapon != newWeapon)
        {
            currentWeapon = newWeapon;

            switch (currentWeapon)
            {
                case "Dagger":
                    ChangeWeaponStats(4, 1, 0.5f, 0.1f, "Neutral", 0);
                    weaponImageIndex = 0;
                    playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[1];
                    break;

                case "ShortSword":
                    ChangeWeaponStats(10, 1, 1f, 0.15f, "Neutral", 0);
                    weaponImageIndex = 1;
                    playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[2];
                    break;

                case "LongSword":
                    ChangeWeaponStats(10, 0, 1f, 0.25f, "Neutral", 0);
                    weaponImageIndex = 2;
                    playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[3];
                    break;

                case "Spear":
                    ChangeWeaponStats(12, 1, 1f, 0.2f, "Neutral", 0);
                    weaponImageIndex = 3;
                    playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[4];
                    break;

                case "HandAxe":
                    ChangeWeaponStats(12, 1, 1f, 0.2f, "Neutral", 0);
                    weaponImageIndex = 4;
                    playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[5];
                    break;

                case "Mace":
                    ChangeWeaponStats(12, 1, 1f, 0.2f, "Neutral", 0);
                    weaponImageIndex = 5;
                    playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[6];
                    break;
            }
        }
    }


    public void ChangeWeaponStats(float damage, float projectileSpeed, float attackSpeed, float attackRange, string damageType, float slowAmt)
    {
        weaponScript.damage = damage;
        weaponScript.projectileSpeed = projectileSpeed;
        weaponScript.attackSpeed = attackSpeed;
        weaponScript.attackRange = attackRange;
        weaponScript.damageType = damageType;
        weaponScript.slowAmt = slowAmt;
    }
}
