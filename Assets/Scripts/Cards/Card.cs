using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    [SerializeField] private int upgradeCost;
    [SerializeField] private string upgradeType;
    [SerializeField] private int bonusAmt;
    [SerializeField] private float bonusFloatAmt;

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
            if (upgradeType == "BonusDmg")
            {
                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.IsHoveringOverUiCard = false;
                GlobalVars.bonusStats[cardName] += bonusAmt;
                GlobalVars.triggerShopCardDestruction = true;

                if (GlobalVars.bonusStats[cardName + "Lvl"] < 3)
                {
                    GlobalVars.bonusStats[cardName + "Lvl"]++;
                }
            }

            if (upgradeType == "BonusSpeed")
            {
                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.IsHoveringOverUiCard = false;
                GlobalVars.bonusExtraStats[cardName] += bonusFloatAmt;
                GlobalVars.triggerShopCardDestruction = true;

                if (GlobalVars.bonusExtraStats[cardName + "Lvl"] < 4)
                {
                    GlobalVars.bonusExtraStats[cardName + "Lvl"]++;
                }
            }

            if (upgradeType == "BonusRange")
            {
                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.IsHoveringOverUiCard = false;
                GlobalVars.bonusExtraStats[cardName] += bonusFloatAmt;
                GlobalVars.triggerShopCardDestruction = true;

                if (GlobalVars.bonusExtraStats[cardName + "Lvl"] < 4)
                {
                    GlobalVars.bonusExtraStats[cardName + "Lvl"]++;
                }
            }

            if (upgradeType == "BonusCritChance")
            {
                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.IsHoveringOverUiCard = false;
                GlobalVars.bonusExtraStats[cardName] += bonusFloatAmt;
                GlobalVars.triggerShopCardDestruction = true;

                if (GlobalVars.bonusExtraStats[cardName + "Lvl"] < 4)
                {
                    GlobalVars.bonusExtraStats[cardName + "Lvl"]++;
                }
            }

            if (upgradeType == "Weapon")
            {
                GlobalVars.newWeapon = cardName;
                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.triggerShopCardDestruction = true;
                GlobalVars.IsHoveringOverUiCard = false;

                if (GlobalVars.bonusStats["EquipmentLvl"] < 5)
                {
                    GlobalVars.bonusStats["EquipmentLvl"]++;
                }
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
