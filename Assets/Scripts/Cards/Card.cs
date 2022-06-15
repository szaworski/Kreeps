using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    [SerializeField] private int upgradeCost;
    [SerializeField] private string upgradeType;

    GameObject playerHud;
    PlayerHud playerHudScript;
    MouseCursor mouseCursorScript;

    void Awake()
    {
        playerHud = GameObject.Find("PlayerHud");
        playerHudScript = playerHud.GetComponent<PlayerHud>();
        mouseCursorScript = playerHud.GetComponent<MouseCursor>();
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

                switch (cardName)
                {
                    case var _ when cardName.Contains("Neutral"):
                        GlobalVars.bonusNormalDmg += 1;
                        GlobalVars.triggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Fire"):
                        GlobalVars.bonusFireDmg += 1;
                        GlobalVars.triggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Ice"):
                        GlobalVars.bonusIceDmg += 1;
                        GlobalVars.triggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Thunder"):
                        GlobalVars.bonusThunderDmg += 2;
                        GlobalVars.triggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Holy"):
                        GlobalVars.bonusHolyDmg += 2;
                        GlobalVars.triggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Swift"):
                        GlobalVars.bonusSwiftDmg += 2;
                        GlobalVars.triggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Cosmic"):
                        GlobalVars.bonusCosmicDmg += 3;
                        GlobalVars.triggerShopCardDestruction = true;
                        break;
                }
            }

            if (upgradeType == "Weapon")
            {
                GlobalVars.newWeapon = cardName;

                GlobalVars.newGoldValue = GlobalVars.gold - upgradeCost;
                GlobalVars.triggerShopCardDestruction = true;
                GlobalVars.IsHoveringOverUiCard = false;
                GlobalVars.equipmentLvl++;
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
