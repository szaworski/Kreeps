using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public static bool IsHoveringOverUiCard;
    public string cardName;

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
        TileSpawner.triggerTileCardDestruction = true;
    }
}
