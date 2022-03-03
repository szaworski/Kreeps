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
        switch (cardName)
        {
            case "Forrest":
                TileSpawner.tileCardSelected = "Forrest";
                IsHoveringOverUiCard = false;
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "Graveyard":
                TileSpawner.tileCardSelected = "Graveyard";
                IsHoveringOverUiCard = false;
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "River":
                TileSpawner.tileCardSelected = "River";
                IsHoveringOverUiCard = false;
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "Mountain":
                TileSpawner.tileCardSelected = "Mountain";
                IsHoveringOverUiCard = false;
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "Swamp":
                TileSpawner.tileCardSelected = "Swamp";
                IsHoveringOverUiCard = false;
                TileSpawner.triggerTileCardDestruction = true;
                break;
        }
    }
}
