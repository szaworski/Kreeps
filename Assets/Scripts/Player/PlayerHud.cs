using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private bool showStartWaveInstructions;
    [SerializeField] private bool triggerBonusStatsUpdate;
    public bool GetSetShowStartWaveInstructions
    {
        get { return showStartWaveInstructions; }
        set { showStartWaveInstructions = value; }
    }
    public bool GetSetTriggerBonusStatsUpdate
    {
        get { return triggerBonusStatsUpdate; }
        set { triggerBonusStatsUpdate = value; }
    }

    public static int gold;
    public static int newGoldValue;

    GameObject tileManager;
    GameObject monsterManager;
    TileSpawner tileSpawnerScript;
    MonsterManager monsterManagerScript;

    void Awake()
    {
        tileManager = GameObject.Find("TileManager");
        tileSpawnerScript = tileManager.GetComponent<TileSpawner>();
        monsterManager = GameObject.Find("MonsterManager");
        monsterManagerScript = monsterManager.GetComponent<MonsterManager>();
        //When changing the gold value, add or subtract from the "gold" variable, and then set "goldAmtUiText" to the new value
        gold = 200;
        newGoldValue = gold;
        goldAmtUiText.SetText(gold.ToString());
        showStartWaveInstructions = true;
        showBonusStats = true;

        //"selectedTowerType" is set to neutral by deafult
        TowerGrid.towerTypeSelected = "Neutral";
    }

    void Update()
    {
        if (newGoldValue > gold || newGoldValue < gold)
        {
            ChangeGoldAmt();
        }

        if (curWaveNum < monsterManagerScript.GetMonsterCount)
        {
            ChangeWaveNum();
        }

        if (triggerBonusStatsUpdate)
        {
            ChangeKreepBonusStats();
        }

        HideShowStartWaveInstructions();
        HideShowBonusStats();
    }

    public void ChangeGoldAmt()
    {
        gold = newGoldValue;
        goldAmtUiText.SetText(gold.ToString());
    }

    public void ChangeWaveNum()
    {
        //Update the wave number to be = to "monsterCount". (This count will match the wave number, so we can use this instead of a new variable)
        curWaveNum = monsterManagerScript.GetMonsterCount;
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
        bonusMaxHealth += 6 * tileSpawnerScript.GetNumOfRivers;
        bonusMaxHealth += 2 * tileSpawnerScript.GetNumOfMountains;
        bonusMaxHealth += 2 * tileSpawnerScript.GetNumOfGraveyards;
        bonusArmor += 1 * tileSpawnerScript.GetNumOfMountains;
        bonusMoveSpeed += 0.04f * tileSpawnerScript.GetNumOfForrests;
        bonusHpRegen += 1 * tileSpawnerScript.GetNumOfGraveyards;
        bonusEvasion += 0.02f * tileSpawnerScript.GetNumOfSwamps;

        //Apply any Tier 2 bonuses
        bonusMaxHealth += 10 * tileSpawnerScript.GetNumOfSeashores;
        bonusMaxHealth += 7 * tileSpawnerScript.GetNumOfThickets;
        bonusMaxHealth += 5 * tileSpawnerScript.GetNumOfSettlements;
        bonusMaxHealth += 4 * tileSpawnerScript.GetNumOfDeserts;
        bonusMaxHealth += 3 * tileSpawnerScript.GetNumOfTundras;

        bonusArmor += 1 * tileSpawnerScript.GetNumOfSeashores;
        bonusArmor += 1 * tileSpawnerScript.GetNumOfTundras;
        bonusArmor += 2 * tileSpawnerScript.GetNumOfCaverns;

        bonusMoveSpeed += 0.02f * tileSpawnerScript.GetNumOfTundras;
        bonusMoveSpeed += 0.03f * tileSpawnerScript.GetNumOfDeserts;
        bonusMoveSpeed += 0.05f * tileSpawnerScript.GetNumOfSettlements;

        bonusHpRegen += 1 * tileSpawnerScript.GetNumOfThickets;
        bonusHpRegen += 1 * tileSpawnerScript.GetNumOfTundras;
        bonusHpRegen += 2 * tileSpawnerScript.GetNumOfCaverns;

        bonusEvasion += 0.01f * tileSpawnerScript.GetNumOfCaverns;
        bonusEvasion += 0.01f * tileSpawnerScript.GetNumOfDeserts;
        bonusEvasion += 0.03f * tileSpawnerScript.GetNumOfThickets;

        //Apply any Tier 3 bonuses

        //Set the new text values
        bonusMoveSpeedUiText.SetText(("+") + (bonusMoveSpeed * 100).ToString());
        bonusMaxHealthUiText.SetText(("+") + bonusMaxHealth.ToString());
        bonusHpRegenUiText.SetText(("+") + bonusHpRegen.ToString());
        bonusEvasionUiText.SetText(("+") + Mathf.Round(bonusEvasion * 100) + ("%").ToString());
        bonusArmorUiText.SetText(("+") + bonusArmor.ToString());

        triggerBonusStatsUpdate = false;
    }
}
