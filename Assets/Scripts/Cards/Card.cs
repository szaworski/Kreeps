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

    void Awake()
    {
        tileManager = GameObject.Find("TileManager");
        playerHud = GameObject.Find("PlayerHud");
        tileSpawnerScript = tileManager.GetComponent<TileSpawner>();
        playerHudScript = playerHud.GetComponent<PlayerHud>();
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
