﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    [SerializeField] private int upgradeCost;
    [SerializeField] private string upgradeType;
    [SerializeField] private int bonusAmt;
    [SerializeField] private float bonusFloatAmt;

    [Header("Exponential Value")]
    public TMP_Text exponentTxt;

    private void Awake()
    {
        if (exponentTxt != null)
        {
            exponentTxt.SetText( "(+" + (GlobalVars.coinChoiceCount * 10).ToString() + ")");
        }
    }

    void Update()
    {
        //Check if the mouse is over any UI elements to disable other functionality underneath
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GlobalVars.IsHoveringOverUiCard = true;
        }

        else
        {
            GlobalVars.IsHoveringOverUiCard = false;
        }
    }

    public void SetSelectedCardType()
    {
        if (!GlobalVars.isPaused && !GlobalVars.weaponIsSelected)
        {
            GlobalVars.tileCardSelected = cardName;
            GlobalVars.IsHoveringOverUiCard = false;
            GlobalVars.triggerTileCardDestruction = true;
            GlobalVars.triggerBonusStatsUpdate = true;
        }

        else
        {
            GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("Error");
        }
    }

    public void SetSelectedMonsterCard()
    {
        if (!GlobalVars.isPaused)
        {
            GlobalVars.monsterCardSelected = cardName;
            GlobalVars.IsHoveringOverUiCard = false;
            GlobalVars.triggerMonsterCardDestruction = true;
            //Debug.Log("Monster Card selected: " + cardName);
        }
    }

    public void SetSelectedUpgradeCard()
    {
        if (GlobalVars.gold >= upgradeCost && !GlobalVars.isPaused)
        {
            GlobalVars.upgradeCardSelected = cardName;
            GlobalVars.upgradeTypeSelected = upgradeType;
            GlobalVars.upgradeGoldCost = upgradeCost;
            GlobalVars.IsHoveringOverUiCard = false;
            GlobalVars.triggerTowerUpgrade = true;
            GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("PlaceTower");
            //Debug.Log("Upgrade Card selected: " + cardName);
        }

        else
        {
            GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("Error");
        }
    }

    public void SetSelectedShopCard()
    {
        if (GlobalVars.gold >= upgradeCost && !GlobalVars.isPaused)
        {
            GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
            GlobalVars.IsHoveringOverUiCard = false;
            GlobalVars.triggerShopCardDestruction = true;

            switch (upgradeType)
            {
                case "BonusDmg":
                    GlobalVars.bonusStats[cardName] += bonusAmt;
                    //GlobalVars.bonusStats[cardName + "Lvl"]++;
                    break;

                case "Special":
                    if (GlobalVars.bonusExtraStats[cardName + "Lvl"] < 4)
                    {
                        GlobalVars.bonusExtraStats[cardName] += bonusFloatAmt;
                        GlobalVars.bonusExtraStats[cardName + "Lvl"]++;
                    }
                    break;

                case "Weapon":
                    GlobalVars.newWeapon = cardName;

                    if (GlobalVars.bonusStats["EquipmentLvl"] < 4)
                    {
                        GlobalVars.bonusStats["EquipmentLvl"]++;
                    }
                    break;
            }
        }

        else
        {
            GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("Error");
        }
    }

    public void SelectGoldCard()
    {
        if (!GlobalVars.isPaused)
        {
            GlobalVars.newGoldValue = GlobalVars.gold + 10 + (GlobalVars.coinChoiceCount * 10);
            GlobalVars.triggerShopCardDestruction = true;
            GlobalVars.IsHoveringOverUiCard = false;
            GlobalVars.coinChoiceCount++;
        }
    }

    public void SellTower()
    {
        if (!GlobalVars.isPaused)
        {
            GlobalVars.triggerTowerSell = true;
        }
    }

    public void CloseUpgrades()
    {
        if (!GlobalVars.isPaused)
        {
            GlobalVars.triggerUpgradeCardDestruction = true;
        }
    }
}
