using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    [SerializeField] private int upgradeCost;
    [SerializeField] private string upgradeType;

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
        if (!GlobalVars.isPaused)
        {
            GlobalVars.tileCardSelected = cardName;
            GlobalVars.IsHoveringOverUiCard = false;
            GlobalVars.triggerTileCardDestruction = true;
            GlobalVars.triggerBonusStatsUpdate = true;
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
            if (upgradeType == "Bonus")
            {
                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.IsHoveringOverUiCard = false;
                GlobalVars.bonusStats[cardName]++;
                GlobalVars.triggerShopCardDestruction = true;
            }

            if (upgradeType == "Weapon")
            {
                GlobalVars.newWeapon = cardName;
                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.triggerShopCardDestruction = true;
                GlobalVars.IsHoveringOverUiCard = false;
                GlobalVars.bonusStats["EquipmentLvl"]++;
            }
        }

        else
        {
            GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("Error");
        }
    }

    public void SelectExitCard()
    {
        if (!GlobalVars.isPaused)
        {
            GlobalVars.triggerShopCardDestruction = true;
            GlobalVars.IsHoveringOverUiCard = false;
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
