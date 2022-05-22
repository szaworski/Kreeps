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
    public static float bonusMoveSpeed;
    public static float bonusMaxHealth;
    public static float bonusHpRegen;
    public static float bonusEvasion;
    public static float bonusArmor;
    public static bool IsHoveringOverHudElement;
    public static bool showStartWaveInstructions;
    public static bool triggerBonusStatsUpdate;

    [SerializeField] private TMP_Text goldAmtUiText;
    [SerializeField] private TMP_Text waveNumUiText;
    [SerializeField] private TMP_Text waveStartUiText;
    [SerializeField] private TMP_Text bonusMoveSpeedUiText;
    [SerializeField] private TMP_Text bonusMaxHealthUiText;
    [SerializeField] private TMP_Text bonusHpRegenUiText;
    [SerializeField] private TMP_Text bonusEvasionUiText;
    [SerializeField] private TMP_Text bonusArmorUiText;

    void Awake()
    {
        //When changing the gold value, add or subtract from the "gold" variable, and then set "goldAmtUiText" to the new value
        gold = 200;
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

        if (triggerBonusStatsUpdate)
        {
            ChangeKreepBonusStats();
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

    public void ChangeKreepBonusStats()
    {
        //Reset values back to zero so they can be recalculated
        bonusMoveSpeed = 0;
        bonusMaxHealth = 0;
        bonusHpRegen = 0;
        bonusArmor = 0;
        bonusEvasion = 0;

        //Apply any Tier 1 bonuses
        bonusMaxHealth += 6 * TileSpawner.numOfRivers;
        bonusMaxHealth += 2 * TileSpawner.numOfMountains;
        bonusArmor += 1 * TileSpawner.numOfMountains;
        bonusMoveSpeed += 0.04f * TileSpawner.numOfForrests;
        bonusHpRegen += 2 * TileSpawner.numOfGraveyards;
        bonusEvasion += 0.02f * TileSpawner.numOfSwamps;

        //Apply any Tier 2 bonuses
        bonusMaxHealth += 10 * TileSpawner.numOfSeashores;
        bonusMaxHealth += 7 * TileSpawner.numOfThickets;
        bonusMaxHealth += 5 * TileSpawner.numOfSettlements;
        bonusMaxHealth += 4 * TileSpawner.numOfDeserts;
        bonusMaxHealth += 3 * TileSpawner.numOfTundras;

        bonusArmor += 1 * TileSpawner.numOfSeashores;
        bonusArmor += 1 * TileSpawner.numOfTundras;
        bonusArmor += 2 * TileSpawner.numOfCaverns;

        bonusMoveSpeed += 0.02f * TileSpawner.numOfTundras;
        bonusMoveSpeed += 0.03f * TileSpawner.numOfDeserts;
        bonusMoveSpeed += 0.05f * TileSpawner.numOfSettlements;

        bonusHpRegen += 1 * TileSpawner.numOfThickets;
        bonusHpRegen += 1 * TileSpawner.numOfTundras;
        bonusHpRegen += 2 * TileSpawner.numOfCaverns;

        bonusEvasion += 0.01f * TileSpawner.numOfCaverns;
        bonusEvasion += 0.01f * TileSpawner.numOfDeserts;
        bonusEvasion += 0.03f * TileSpawner.numOfThickets;

        //Apply any Tier 3 bonuses

        //Set the new text values
        bonusMoveSpeedUiText.SetText(("+") + (bonusMoveSpeed * 100).ToString());
        bonusMaxHealthUiText.SetText(("+") + bonusMaxHealth.ToString());
        bonusHpRegenUiText.SetText(("+") + bonusHpRegen.ToString());
        bonusEvasionUiText.SetText(("+") + (bonusEvasion * 100) + ("%").ToString());
        bonusArmorUiText.SetText(("+") + bonusArmor.ToString());

        triggerBonusStatsUpdate = false;
    }
}
