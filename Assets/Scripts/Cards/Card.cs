using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    [SerializeField] private int upgradeCost;
    [SerializeField] private string upgradeType;
    public static bool IsHoveringOverUiCard;

    GameObject tileManager;
    GameObject playerHud;
    PlayerHud playerHudScript;
    MouseCursor mouseCursorScript;

    void Awake()
    {
        tileManager = GameObject.Find("TileManager");
        playerHud = GameObject.Find("PlayerHud");
        playerHudScript = playerHud.GetComponent<PlayerHud>();
        mouseCursorScript = playerHud.GetComponent<MouseCursor>();
    }

    void Update()
    {
        //Check if the mouse is over any UI elements to disable other functionality underneath
        if (EventSystem.current.IsPointerOverGameObject())
        {
            IsHoveringOverUiCard = true;
        }

        else
        {
            IsHoveringOverUiCard = false;
        }
    }

    public void SetSelectedCardType()
    {
        if (!PauseMenuButtons.isPaused)
        {
            GlobalVars.tileCardSelected = cardName;
            IsHoveringOverUiCard = false;
            GlobalVars.triggerTileCardDestruction = true;
            playerHudScript.GetSetTriggerBonusStatsUpdate = true;
        }
    }

    public void SetSelectedMonsterCard()
    {
        if (!PauseMenuButtons.isPaused)
        {
            GlobalVars.monsterCardSelected = cardName;
            IsHoveringOverUiCard = false;
            GlobalVars.triggerMonsterCardDestruction = true;
            //Debug.Log("Monster Card selected: " + cardName);
        }
    }

    public void SetSelectedUpgradeCard()
    {
        if (PlayerHud.gold >= upgradeCost && !PauseMenuButtons.isPaused)
        {
            GlobalVars.upgradeCardSelected = cardName;
            GlobalVars.upgradeTypeSelected = upgradeType;
            GlobalVars.upgradeGoldCost = upgradeCost;
            IsHoveringOverUiCard = false;
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
        if (PlayerHud.gold >= upgradeCost && !PauseMenuButtons.isPaused)
        {
            if (upgradeType == "Bonus")
            {
                PlayerHud.newGoldValue = PlayerHud.gold - upgradeCost;
                IsHoveringOverUiCard = false;

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
                mouseCursorScript.GetSetNewWeapon = cardName;

                PlayerHud.newGoldValue = PlayerHud.gold - upgradeCost;
                GlobalVars.triggerShopCardDestruction = true;
                IsHoveringOverUiCard = false;
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
        if (!PauseMenuButtons.isPaused)
        {
            GlobalVars.triggerShopCardDestruction = true;
            IsHoveringOverUiCard = false;
        }
    }

    public void SellTower()
    {
        if (!PauseMenuButtons.isPaused)
        {
            GlobalVars.triggerTowerSell = true;
        }
    }

    public void CloseUpgrades()
    {
        if (!PauseMenuButtons.isPaused)
        {
            GlobalVars.triggerUpgradeCardDestruction = true;
        }
    }
}
