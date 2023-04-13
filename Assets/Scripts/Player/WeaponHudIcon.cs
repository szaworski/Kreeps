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
            ShowWeaponStats();
        }
    }

    void OnMouseExit()
    {
        HideWeaponStats();
    }

    void HideWeaponStats()
    {
        if (weaponStats.activeSelf)
        {
            GlobalVars.IsHoveringOverUiCard = false;
            weaponStats.SetActive(false);
        }
    }

    void ShowWeaponStats()
    {
        if (!weaponStats.activeSelf)
        {
            UpdateWeaponStatsText();
            GlobalVars.IsHoveringOverUiCard = true;
            weaponStats.SetActive(true);
        }
    }

    void UpdateWeaponStatsText()
    {
        damage = weaponScript.damage;
        projectileSpeed = weaponScript.projectileSpeed;
        attackSpeed = weaponScript.attackSpeed;
        attackRange = weaponScript.attackRange;
        float[] bonusStats = weaponScript.bonusStats;

        if (projectileSpeed == 0)
        {
            typeText.SetText("AOE in radius");
        }
        else
        {
            typeText.SetText("Single target");
        }

        if (bonusStats[0] > 0)
        {
            dmgText.SetText(Mathf.Round(damage - bonusStats[0]).ToString() + " + " + Mathf.Round(bonusStats[0]).ToString());
        }
        else
        {
            dmgText.SetText(Mathf.Round(damage).ToString());
        }

        if ((attackSpeed % 1) == 0)
        {
            if (bonusStats[1] > 0)
            {
                rofText.SetText(string.Format("{0:C0}", (attackSpeed + bonusStats[1]).ToString() + "s - " + string.Format("{0:C0}", bonusStats[1].ToString() + "s")));
            }
            else
            {
                rofText.SetText(attackSpeed.ToString() + "s");
            }
        }
        else
        {
            if (bonusStats[1] > 0)
            {
                rofText.SetText((attackSpeed + bonusStats[1]).ToString("F1") + "s - " + bonusStats[1].ToString("F1") + "s");
            }
            else
            {
                rofText.SetText(attackSpeed.ToString() + "s");
            }
        }

        if (bonusStats[2] > 0)
        {
            rngText.SetText((attackRange - bonusStats[2]).ToString("F1") + " + " + bonusStats[2]);
        }
        else
        {
            rngText.SetText(attackRange.ToString("F1"));
        }
        /*
        dmgText.SetText(Mathf.Round(damage).ToString());
        rngText.SetText(attackRange.ToString("F1"));

        if ((attackSpeed % 1) == 0)
        {
            rofText.SetText(string.Format("{0:C0}", attackSpeed.ToString()) + "s");
        }
        else
        {
            rofText.SetText(attackSpeed.ToString("F1") + "s");
        }
        */
    }
}
