using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponHudIcon : MonoBehaviour
{
    [SerializeField] private GameObject weaponStats;
    GameObject playerWeapon;
    Weapon weaponScript;

    [Header("Weapon Stats")]
    public TMP_Text typeText;
    public TMP_Text dmgText;
    public TMP_Text rofText;
    public TMP_Text rngText;

    private float damage;
    private float projectileSpeed;
    private float attackSpeed;
    private float attackRange;

    void Awake()
    {
        playerWeapon = GameObject.Find("PlayerWeapon");
        weaponScript = playerWeapon.GetComponent<Weapon>();
    }

    private void Update()
    {
        if (weaponStats.activeSelf && GlobalVars.triggerWeaponHudStatsUpdate)
        {
            UpdateWeaponStatsText();
            GlobalVars.triggerWeaponHudStatsUpdate = false;
        }
    }

    void OnMouseOver()
    {
        if (GlobalVars.newWeapon != "")
        {
            if (Input.GetMouseButtonDown(0))
            {
                HideShowWeaponStats();
            }
        }
    }

    void HideShowWeaponStats()
    {
        if (!weaponStats.activeSelf)
        {
            UpdateWeaponStatsText();
            GlobalVars.IsHoveringOverUiCard = true;
            weaponStats.SetActive(true);
        }

        else if (weaponStats.activeSelf)
        {
            GlobalVars.IsHoveringOverUiCard = false;
            weaponStats.SetActive(false);
        }
    }

    void UpdateWeaponStatsText()
    {
        damage = weaponScript.startingDamage;
        projectileSpeed = weaponScript.projectileSpeed;
        attackSpeed = weaponScript.startingSpeed;
        attackRange = weaponScript.startingAttackRange;

        if (projectileSpeed == 0)
        {
            typeText.SetText("AOE in radius");
        }
        else
        {
            typeText.SetText("Single target");
        }

        dmgText.SetText(damage.ToString());
        rofText.SetText(attackSpeed.ToString());
        rngText.SetText(attackRange.ToString());
    }
}
