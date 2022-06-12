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
    TileSpawner tileSpawnerScript;
    PlayerHud playerHudScript;
    MouseCursor mouseCursorScript;

    void Awake()
    {
        tileManager = GameObject.Find("TileManager");
        playerHud = GameObject.Find("PlayerHud");
        tileSpawnerScript = tileManager.GetComponent<TileSpawner>();
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
            tileSpawnerScript.GetSetTileCardSelected = cardName;
            IsHoveringOverUiCard = false;
            tileSpawnerScript.GetSetTriggerTileCardDestruction = true;
            playerHudScript.GetSetTriggerBonusStatsUpdate = true;
        }
    }

    public void SetSelectedMonsterCard()
    {
        if (!PauseMenuButtons.isPaused)
        {
            tileSpawnerScript.GetSetMonsterCardSelected = cardName;
            IsHoveringOverUiCard = false;
            tileSpawnerScript.GetSetTriggerMonsterCardDestruction = true;
            //Debug.Log("Monster Card selected: " + cardName);
        }
    }

    public void SetSelectedUpgradeCard()
    {
        if (PlayerHud.gold >= upgradeCost && !PauseMenuButtons.isPaused)
        {
            TowerGrid.upgradeCardSelected = cardName;
            TowerGrid.upgradeTypeSelected = upgradeType;
            TowerGrid.upgradeGoldCost = upgradeCost;
            IsHoveringOverUiCard = false;
            TowerGrid.triggerTowerUpgrade = true;
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
                        TileSpawner.bonusNormalDmg += 1;
                        tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Fire"):
                        TileSpawner.bonusFireDmg += 1;
                        tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Ice"):
                        TileSpawner.bonusIceDmg += 1;
                        tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Thunder"):
                        TileSpawner.bonusThunderDmg += 2;
                        tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Holy"):
                        TileSpawner.bonusHolyDmg += 2;
                        tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Swift"):
                        TileSpawner.bonusSwiftDmg += 2;
                        tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                        break;

                    case var _ when cardName.Contains("Cosmic"):
                        TileSpawner.bonusCosmicDmg += 3;
                        tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                        break;
                }
            }

            if (upgradeType == "Weapon")
            {
                mouseCursorScript.GetSetNewWeapon = cardName;

                PlayerHud.newGoldValue = PlayerHud.gold - upgradeCost;
                tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
                IsHoveringOverUiCard = false;
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
            tileSpawnerScript.GetSetTriggerShopCardDestruction = true;
            IsHoveringOverUiCard = false;
        }
    }

    public void SellTower()
    {
        if (!PauseMenuButtons.isPaused)
        {
            TowerGrid.triggerTowerSell = true;
        }
    }

    public void CloseUpgrades()
    {
        if (!PauseMenuButtons.isPaused)
        {
            TowerGrid.triggerUpgradeCardDestruction = true;
        }
    }
}
