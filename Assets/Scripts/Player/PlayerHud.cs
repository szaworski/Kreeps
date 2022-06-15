using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        if (Input.GetKeyDown(KeyCode.Tab) && showBonusStats)
        {
            bonusStats.SetActive(false);
            showBonusStats = false;
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && !showBonusStats)
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
        bonusMaxHealth += 6 * GlobalVars.numOfRivers;
        bonusMaxHealth += 2 * GlobalVars.numOfMountains;
        bonusMaxHealth += 2 * GlobalVars.numOfGraveyards;
        bonusArmor += 1 * GlobalVars.numOfMountains;
        bonusMoveSpeed += 0.04f * GlobalVars.numOfForests;
        bonusHpRegen += 1 * GlobalVars.numOfGraveyards;
        bonusEvasion += 0.02f * GlobalVars.numOfSwamps;

        //Apply any Tier 2 bonuses
        bonusMaxHealth += 10 * GlobalVars.numOfSeashores;
        bonusMaxHealth += 7 * GlobalVars.numOfThickets;
        bonusMaxHealth += 5 * GlobalVars.numOfSettlements;
        bonusMaxHealth += 4 * GlobalVars.numOfDeserts;
        bonusMaxHealth += 3 * GlobalVars.numOfTundras;

        bonusArmor += 1 * GlobalVars.numOfSeashores;
        bonusArmor += 1 * GlobalVars.numOfTundras;
        bonusArmor += 2 * GlobalVars.numOfCaverns;

        bonusMoveSpeed += 0.02f * GlobalVars.numOfTundras;
        bonusMoveSpeed += 0.03f * GlobalVars.numOfDeserts;
        bonusMoveSpeed += 0.05f * GlobalVars.numOfSettlements;

        bonusHpRegen += 1 * GlobalVars.numOfThickets;
        bonusHpRegen += 1 * GlobalVars.numOfTundras;
        bonusHpRegen += 2 * GlobalVars.numOfCaverns;

        bonusEvasion += 0.01f * GlobalVars.numOfCaverns;
        bonusEvasion += 0.01f * GlobalVars.numOfDeserts;
        bonusEvasion += 0.03f * GlobalVars.numOfThickets;

        //Apply any Tier 3 bonuses

        //Set the new text values
        bonusMoveSpeedUiText.SetText(("+") + (bonusMoveSpeed * 100).ToString());
        bonusMaxHealthUiText.SetText(("+") + bonusMaxHealth.ToString());
        bonusHpRegenUiText.SetText(("+") + bonusHpRegen.ToString());
        bonusEvasionUiText.SetText(("+") + Mathf.Round(bonusEvasion * 100) + ("%").ToString());
        bonusArmorUiText.SetText(("+") + bonusArmor.ToString());

        GlobalVars.triggerBonusStatsUpdate = false;
    }
}
