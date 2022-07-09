﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private TMP_Text goldAmtUiText;
    [SerializeField] private TMP_Text waveNumUiText;
    [SerializeField] private TMP_Text waveStartUiText;
    [SerializeField] private TMP_Text bonusMoveSpeedUiText;
    [SerializeField] private TMP_Text bonusMaxHealthUiText;
    [SerializeField] private TMP_Text bonusHpRegenUiText;
    [SerializeField] private TMP_Text bonusEvasionUiText;
    [SerializeField] private TMP_Text bonusArmorUiText;
    [SerializeField] private GameObject bonusStats;
    [SerializeField] private int curWaveNum;
    [SerializeField] private float bonusMoveSpeed;
    [SerializeField] private float bonusMaxHealth;
    [SerializeField] private float bonusHpRegen;
    [SerializeField] private float bonusEvasion;
    [SerializeField] private float bonusArmor;
    [SerializeField] private bool showBonusStats;
    [SerializeField] private GameObject weaponHudObj;
    [SerializeField] private Image weaponHudImage;
    [SerializeField] private Sprite[] weaponHudImagesList;

    public Image GetSetweaponHudImage
    {
        get { return weaponHudImage; }
        set { weaponHudImage = value; }
    }
    public Sprite[] GetWeaponHudImagesList
    {
        get { return weaponHudImagesList; }
    }

    void Awake()
    {
        //Assign the starting player weapon UI sprite
        weaponHudImage = weaponHudObj.GetComponent<Image>();
        weaponHudImage.sprite = weaponHudImagesList[0];

        //When changing the gold value, add or subtract from the "gold" variable, and then set "goldAmtUiText" to the new value
        GlobalVars.gold = 200;
        GlobalVars.newGoldValue = GlobalVars.gold;
        goldAmtUiText.SetText(GlobalVars.gold.ToString());
        GlobalVars.showStartWaveInstructions = true;
        showBonusStats = false;

        //"selectedTowerType" is set to neutral by deafult
        GlobalVars.towerTypeSelected = "Neutral";
    }

    void Update()
    {
        if (GlobalVars.newGoldValue > GlobalVars.gold || GlobalVars.newGoldValue < GlobalVars.gold)
        {
            ChangeGoldAmt();
        }

        if (curWaveNum < GlobalVars.monsterCount)
        {
            ChangeWaveNum();
        }

        if (GlobalVars.triggerBonusStatsUpdate)
        {
            ChangeKreepBonusStats();
        }

        HideShowStartWaveInstructions();
        HideShowBonusStats();
    }

    public void ChangeGoldAmt()
    {
        GlobalVars.gold = GlobalVars.newGoldValue;
        goldAmtUiText.SetText(GlobalVars.gold.ToString());
    }

    public void ChangeWaveNum()
    {
        //Update the wave number to be = to "monsterCount". (This count will match the wave number, so we can use this instead of a new variable)
        curWaveNum = GlobalVars.monsterCount;
        waveNumUiText.SetText(curWaveNum.ToString());
    }

    public void HideShowStartWaveInstructions()
    {
        //This gets set to true in "DestroyMonsterCards()" (See GlobalVars.cs)
        if (GlobalVars.showStartWaveInstructions && !waveStartUiText.enabled)
        {
            waveStartUiText.enabled = true;
        }

        //This gets set to false in "SpawnMonsters()" (See MonsterManager.cs)
        else if (!GlobalVars.showStartWaveInstructions && waveStartUiText.enabled)
        {
            //Hide the start wave text
            waveStartUiText.enabled = false;
        }
    }

    public void HideShowBonusStats()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && showBonusStats && !GlobalVars.isPaused)
        {
            bonusStats.SetActive(false);
            showBonusStats = false;
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && !showBonusStats && !GlobalVars.isPaused)
        {
            bonusStats.SetActive(true);
            showBonusStats = true;
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
        bonusMoveSpeed += 0.04f * GlobalVars.tileCounters["Forest"];

        bonusMaxHealth += 6 * GlobalVars.tileCounters["River"];

        bonusEvasion += 0.02f * GlobalVars.tileCounters["Swamp"];

        bonusMaxHealth += 2 * GlobalVars.tileCounters["Mountain"];
        bonusArmor += 1 * GlobalVars.tileCounters["Mountain"];

        bonusMaxHealth += 2 * GlobalVars.tileCounters["Graveyard"];
        bonusHpRegen += 1 * GlobalVars.tileCounters["Graveyard"];

        //Apply any Tier 2 bonuses
        bonusArmor += 2 * GlobalVars.tileCounters["Cavern"];
        bonusHpRegen += 2 * GlobalVars.tileCounters["Cavern"];

        bonusHpRegen += 2 * GlobalVars.tileCounters["Desert"];
        bonusEvasion += 0.02f * GlobalVars.tileCounters["Desert"];

        bonusMaxHealth += 8 * GlobalVars.tileCounters["Seashore"];
        bonusArmor += 1 * GlobalVars.tileCounters["Seashore"];

        bonusMaxHealth += 5 * GlobalVars.tileCounters["Settlement"];
        bonusMoveSpeed += 0.04f * GlobalVars.tileCounters["Settlement"];

        bonusMaxHealth += 8 * GlobalVars.tileCounters["Thicket"];
        bonusEvasion += 0.02f * GlobalVars.tileCounters["Thicket"];

        bonusHpRegen += 2 * GlobalVars.tileCounters["Tundra"];
        bonusMoveSpeed += 0.02f * GlobalVars.tileCounters["Tundra"];

        //Apply any Tier 3 bonuses

        //Set the new text values
        bonusMoveSpeedUiText.SetText(("+") + (bonusMoveSpeed * 100).ToString());
        bonusMaxHealthUiText.SetText(("+") + bonusMaxHealth.ToString());
        bonusHpRegenUiText.SetText(("+") + bonusHpRegen.ToString());
        bonusEvasionUiText.SetText(("+") + Mathf.Round(bonusEvasion * 100) + ("%").ToString());
        bonusArmorUiText.SetText(("+") + bonusArmor.ToString());

        GlobalVars.bonusKreepStats["BonusMoveSpeed"] = bonusMoveSpeed;
        GlobalVars.bonusKreepStats["BonusMaxHealth"] = bonusMaxHealth;
        GlobalVars.bonusKreepStats["BonusHpRegen"] = bonusHpRegen;
        GlobalVars.bonusKreepStats["BonusArmor"] = bonusArmor;
        GlobalVars.bonusKreepStats["BonusEvasion"] = bonusEvasion;

        GlobalVars.triggerBonusStatsUpdate = false;
    }
}
