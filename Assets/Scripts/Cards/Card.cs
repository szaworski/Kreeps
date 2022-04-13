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
        TileSpawner.tileCardSelected = cardName;
        IsHoveringOverUiCard = false;
        TileSpawner.triggerTileCardDestruction = true;
    }

    public void SetSelectedMonsterCard()
    {
        TileSpawner.monsterCardSelected = cardName;
        IsHoveringOverUiCard = false;
        TileSpawner.triggerMonsterCardDestruction = true;
        //Debug.Log("Monster Card selected: " + cardName);
    }

    public void SetSelectedUpgradeCard()
    {
        if (PlayerHud.gold >= upgradeCost)
        {
            TowerGrid.upgradeCardSelected = cardName;
            TowerGrid.upgradeTypeSelected = upgradeType;
            TowerGrid.upgradeGoldCost = upgradeCost;
            IsHoveringOverUiCard = false;
            TowerGrid.triggerTowerUpgrade = true;
            //Debug.Log("Upgrade Card selected: " + cardName);
        }
    }

    public void DemolishTower()
    {
        TowerGrid.triggerTowerDemolish = true;
    }

    public void CloseUpgrades()
    {
        TowerGrid.triggerUpgradeCardDestruction = true;
    }


}
