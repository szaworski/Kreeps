using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public static bool IsHoveringOverUiCard;
    [SerializeField] private string cardName;
    [SerializeField] private int upgradeCost;
    [SerializeField] private string upgradeType;

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
            TileSpawner.tileCardSelected = cardName;
            IsHoveringOverUiCard = false;
            TileSpawner.triggerTileCardDestruction = true;
            PlayerHud.triggerBonusStatsUpdate = true;
        }
    }

    public void SetSelectedMonsterCard()
    {
        if (!PauseMenuButtons.isPaused)
        {
            TileSpawner.monsterCardSelected = cardName;
            IsHoveringOverUiCard = false;
            TileSpawner.triggerMonsterCardDestruction = true;
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
