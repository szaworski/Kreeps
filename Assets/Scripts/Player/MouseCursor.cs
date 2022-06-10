﻿using System.Collections;
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
    Weapon weaponScript;

    void Awake()
    {
        playerWeapon = GameObject.Find("PlayerWeapon");
        weaponScript = playerWeapon.GetComponent<Weapon>();
        //weaponImageIndex = 0;
        newWeapon = "Spear";

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
                    ChangeWeaponStats(2, 1, 0.4f, 0.1f, "Neutral", 0);
                    weaponImageIndex = 0;
                    break;

                case "ShortSword":
                    ChangeWeaponStats(10, 1, 1f, 0.15f, "Neutral", 0);
                    weaponImageIndex = 1;
                    break;

                case "LongSword":
                    ChangeWeaponStats(10, 0, 1f, 0.25f, "Neutral", 0);
                    weaponImageIndex = 2;
                    break;

                case "Spear":
                    ChangeWeaponStats(12, 1, 1f, 0.2f, "Neutral", 0);
                    weaponImageIndex = 3;
                    break;

                case "HandAxe":
                    ChangeWeaponStats(12, 1, 1f, 0.2f, "Neutral", 0);
                    weaponImageIndex = 4;
                    break;

                case "Mace":
                    ChangeWeaponStats(12, 1, 1f, 0.2f, "Neutral", 0);
                    weaponImageIndex = 5;
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
