using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    //Tier 1 Tiles counts
    public static int numOfForests;
    public static int numOfGraveyards;
    public static int numOfRivers;
    public static int numOfMountains;
    public static int numOfSwamps;

    //Tier 2 Tiles counts
    public static int numOfDeserts;
    public static int numOfThickets;
    public static int numOfTundras;
    public static int numOfCaverns;
    public static int numOfSettlements;
    public static int numOfSeashores;

    //Vars used for generating card choices
    public static string tileName;
    public static bool triggerTileCardDestruction;
    public static bool triggerMonsterCardDestruction;
    public static bool triggerShopCardDestruction;
    public static string tileCardSelected;
    public static string monsterCardSelected;
    public static string currTier;
    public static int numOfTimesPlaced;

    //Power up vars
    public static int bonusNormalDmg;
    public static int bonusFireDmg;
    public static int bonusIceDmg;
    public static int bonusThunderDmg;
    public static int bonusHolyDmg;
    public static int bonusSwiftDmg;
    public static int bonusCosmicDmg;
    public static int equipmentLvl;

    //Tower grid vars
    public static int goldCost;
    public static int upgradeGoldCost;
    public static string towerTypeSelected;
    public static string upgradeCardSelected;
    public static string upgradeTypeSelected;
    public static bool upgradeCardsArePresent;
    public static bool triggerUpgradeCardDestruction;
    public static bool triggerTowerUpgrade;
    public static bool triggerTowerSell;
    public static Vector3 upgradePosition;
    public static GameObject oldTowerObj;
    public static GameObject gridObj;
    public static GameObject upgradeCard1Obj;
    public static GameObject upgradeCard2Obj;
    public static GameObject upgradeCard3Obj;
    public static GameObject upgradeCard4Obj;
    public static GameObject upgradeCard5Obj;
    public static GameObject upgradeCard6Obj;

    //Monster Manager vars
    public static int monsterCount;
    public static bool allMonstersAreSpawned;

    //Card Vars
    public static bool IsHoveringOverUiCard;

    //Player HUD vars
    public static int playerHealth;
    public static int newPlayerHealthValue;
    public static int gold;
    public static int newGoldValue;
    public static bool showStartWaveInstructions;
    public static bool triggerBonusStatsUpdate;

    //Player Weapons vars
    public static string currentWeapon;
    public static string newWeapon;
    public static bool weaponIsSelected;
    public static bool useSlashAnim;

    //Pause Menu vars
    public static bool isPaused;

    void Awake()
    {
        currTier = "Tier1";
        numOfTimesPlaced = 0;
        equipmentLvl = 1;

        //Reset location nums
        numOfForests = 0;
        numOfGraveyards = 0;
        numOfRivers = 0;
        numOfMountains = 0;
        numOfSwamps = 0;

        numOfDeserts = 0;
        numOfThickets = 0;
        numOfTundras = 0;
        numOfCaverns = 0;
        numOfSettlements = 0;
        numOfSeashores = 0;

        //Reset Bonus stats
        bonusNormalDmg = 0;
        bonusFireDmg = 0;
        bonusIceDmg = 0;
        bonusThunderDmg = 0;
        bonusHolyDmg = 0;
        bonusSwiftDmg = 0;
        bonusCosmicDmg = 0;

        //Reset monster vars
        monsterCount = 0;
        allMonstersAreSpawned = false;
    }
}
