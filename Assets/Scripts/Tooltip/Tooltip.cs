using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string text;

    private void OnMouseOver()
    {
        if (!GlobalVars.IsHoveringOverUiCard)
        {
            TooltipManager.tooltipInstance.SetAndShowTooltip(text);
        }
    }

    private void OnMouseExit()
    {
        TooltipManager.tooltipInstance.HideTooltip();
    }
}
