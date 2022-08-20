using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string text;
    public bool isTowerGrid;

    private void OnMouseOver()
    {
        if (!GlobalVars.IsHoveringOverUiCard && !isTowerGrid)
        {
            TooltipManager.tooltipInstance.SetAndShowTooltip(text);
        }

        else if (isTowerGrid)
        {
            if (!GlobalVars.IsHoveringOverUiCard && GlobalVars.IsHoveringOverTower && GlobalVars.selectedTowerHasUpgrades)
            {
                TooltipManager.tooltipInstance.SetAndShowTooltip(text);
            }

            else
            {
                TooltipManager.tooltipInstance.HideTooltip();
            }
        }
    }

    private void OnMouseExit()
    {
        TooltipManager.tooltipInstance.HideTooltip();
    }
}
