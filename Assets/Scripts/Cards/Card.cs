using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public static bool IsHoveringOverUiCard;

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
}
