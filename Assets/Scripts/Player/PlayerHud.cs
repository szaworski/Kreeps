using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHud : MonoBehaviour
{
    public static int gold;
    public static int newGoldValue;
    public static int costOfCurrentSelection;
    public static string selectedTowerType;
    public static bool IsHoveringOverHudElement;
    public TMP_Text goldAmtUiText;

    void Awake()
    {
        //When changing the gold value, add or subtract from the "gold" variable, and then set "goldAmtUiText" to the new value
        gold = 300;
        newGoldValue = gold;
        goldAmtUiText.SetText(gold.ToString());
        //"selectedTowerType" is set to neutral by deafult
        selectedTowerType = "neutral";
    }

    void Update()
    {
        //Check if the mouse is over any UI elements to disable other functionality underneath
        if (EventSystem.current.IsPointerOverGameObject())
        {
            IsHoveringOverHudElement = true;
        }

        else
        {
            IsHoveringOverHudElement = false;
        }

        if (newGoldValue > gold || newGoldValue < gold)
        {
            ChangeGoldAmt();
        }
    }

    public void ChangeGoldAmt()
    {
        gold = newGoldValue;
        goldAmtUiText.SetText(gold.ToString());
    }
}
