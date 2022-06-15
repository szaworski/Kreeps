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
    [SerializeField] private bool showStartWaveInstructions;
    [SerializeField] private bool triggerBonusStatsUpdate;
    [SerializeField] private GameObject weaponHudObj;
    [SerializeField] private Image weaponHudImage;
    [SerializeField] private Sprite[] weaponHudImagesList;
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
    public Image GetSetweaponHudImage
    {
        get { return weaponHudImage; }
        set { weaponHudImage = value; }
    }
    public Sprite[] GetWeaponHudImagesList
    {
        get { return weaponHudImagesList; }
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

        //Assign the starting player weapon UI sprite
        weaponHudImage = weaponHudObj.GetComponent<Image>();
        weaponHudImage.sprite = weaponHudImagesList[0];

        //When changing the gold value, add or subtract from the "gold" variable, and then set "goldAmtUiText" to the new value
        gold = 200;
        newGoldValue = gold;
        goldAmtUiText.SetText(gold.ToString());
        showStartWaveInstructions = true;
        showBonusStats = false;

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
        bonusMaxHealth += 6 * tileSpawnerScript.numOfRivers;
        bonusMaxHealth += 2 * tileSpawnerScript.numOfMountains;
        bonusMaxHealth += 2 * tileSpawnerScript.numOfGraveyards;
        bonusArmor += 1 * tileSpawnerScript.numOfMountains;
        bonusMoveSpeed += 0.04f * tileSpawnerScript.numOfForests;
        bonusHpRegen += 1 * tileSpawnerScript.numOfGraveyards;
        bonusEvasion += 0.02f * tileSpawnerScript.numOfSwamps;

        //Apply any Tier 2 bonuses
        bonusMaxHealth += 10 * tileSpawnerScript.numOfSeashores;
        bonusMaxHealth += 7 * tileSpawnerScript.numOfThickets;
        bonusMaxHealth += 5 * tileSpawnerScript.numOfSettlements;
        bonusMaxHealth += 4 * tileSpawnerScript.numOfDeserts;
        bonusMaxHealth += 3 * tileSpawnerScript.numOfTundras;

        bonusArmor += 1 * tileSpawnerScript.numOfSeashores;
        bonusArmor += 1 * tileSpawnerScript.numOfTundras;
        bonusArmor += 2 * tileSpawnerScript.numOfCaverns;

        bonusMoveSpeed += 0.02f * tileSpawnerScript.numOfTundras;
        bonusMoveSpeed += 0.03f * tileSpawnerScript.numOfDeserts;
        bonusMoveSpeed += 0.05f * tileSpawnerScript.numOfSettlements;

        bonusHpRegen += 1 * tileSpawnerScript.numOfThickets;
        bonusHpRegen += 1 * tileSpawnerScript.numOfTundras;
        bonusHpRegen += 2 * tileSpawnerScript.numOfCaverns;

        bonusEvasion += 0.01f * tileSpawnerScript.numOfCaverns;
        bonusEvasion += 0.01f * tileSpawnerScript.numOfDeserts;
        bonusEvasion += 0.03f * tileSpawnerScript.numOfThickets;

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
