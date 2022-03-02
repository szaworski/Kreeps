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
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "Graveyard":
                TileSpawner.tileCardSelected = "Graveyard";
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "River":
                TileSpawner.tileCardSelected = "River";
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "Mountain":
                TileSpawner.tileCardSelected = "Mountain";
                TileSpawner.triggerTileCardDestruction = true;
                break;

            case "Swamp":
                TileSpawner.tileCardSelected = "Swamp";
                TileSpawner.triggerTileCardDestruction = true;
                break;
        }
    }
}
