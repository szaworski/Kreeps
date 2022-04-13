using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHud : MonoBehaviour
{
    public static int gold;
    public static int curWaveNum;
    public static int newGoldValue;
    public static int costOfCurrentSelection;
    public static bool IsHoveringOverHudElement;
    public static bool showStartWaveInstructions;
    public TMP_Text goldAmtUiText;
    public TMP_Text waveNumUiText;
    public TMP_Text waveStartUiText;

    void Awake()
    {
        //When changing the gold value, add or subtract from the "gold" variable, and then set "goldAmtUiText" to the new value
        gold = 350;
        newGoldValue = gold;
        goldAmtUiText.SetText(gold.ToString());
        showStartWaveInstructions = true;

        //"selectedTowerType" is set to neutral by deafult
        TowerGrid.towerTypeSelected = "Neutral";
    }

    void Update()
    {
        if (newGoldValue > gold || newGoldValue < gold)
        {
            ChangeGoldAmt();
        }

        if (curWaveNum < MonsterManager.monsterCount)
        {
            ChangeWaveNum();
        }

        HideShowStartWaveInstructions();
    }

    public void ChangeGoldAmt()
    {
        gold = newGoldValue;
        goldAmtUiText.SetText(gold.ToString());
    }

    public void ChangeWaveNum()
    {
        //Update the wave number to be = to "monsterCount". (This count will match the wave number, so we can use this instead of a new variable)
        curWaveNum = MonsterManager.monsterCount;
        waveNumUiText.SetText(curWaveNum.ToString());
    }

    public void HideShowStartWaveInstructions()
    {
        //This gets set to true in "DestroyMonsterCards()" (See TileSpawner.cs)
        if (showStartWaveInstructions && !waveStartUiText.enabled)
        {
            waveStartUiText.enabled = true;
        }

        //This gets set to false in "SpawnMonsters()" (See MonsterManager.cs)
        else if (!showStartWaveInstructions && waveStartUiText.enabled)
        {
            //Hide the start wave text
            waveStartUiText.enabled = false;
        }
    }
}
