using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static Dictionary<string, int> tileCounters = new Dictionary<string, int>();
    public static Dictionary<string, int> bonusStats = new Dictionary<string, int>();

    public static string currTier;
    public static string tileName;
    public static string tileCardSelected;
    public static string monsterCardSelected;
    public static string towerTypeSelected;
    public static string upgradeCardSelected;
    public static string upgradeTypeSelected;
    public static string currentWeapon;
    public static string newWeapon;

    public static int goldCost;
    public static int upgradeGoldCost;
    public static int playerHealth;
    public static int newPlayerHealthValue;
    public static int gold;
    public static int newGoldValue;
    public static int monsterCount;

    public static bool triggerTileCardDestruction;
    public static bool triggerMonsterCardDestruction;
    public static bool triggerShopCardDestruction;
    public static bool upgradeCardsArePresent;
    public static bool triggerUpgradeCardDestruction;
    public static bool triggerTowerUpgrade;
    public static bool triggerTowerSell;
    public static bool allMonstersAreSpawned;
    public static bool IsHoveringOverUiCard;
    public static bool showStartWaveInstructions;
    public static bool triggerBonusStatsUpdate;
    public static bool weaponIsSelected;
    public static bool useSlashAnim;
    public static bool isPaused;

    public static GameObject oldTowerObj;
    public static GameObject gridObj;
    public static GameObject upgradeCard1Obj;
    public static GameObject upgradeCard2Obj;
    public static GameObject upgradeCard3Obj;
    public static GameObject upgradeCard4Obj;
    public static GameObject upgradeCard5Obj;
    public static GameObject upgradeCard6Obj;
    public static Vector3 upgradePosition;

    void Awake()
    {
        //Add starting location counts (Reset location counts)
        tileCounters.Clear();
        tileCounters.Add("numOfTimesPlaced", 0);

        tileCounters.Add("Forest", 0);
        tileCounters.Add("Graveyard", 0);
        tileCounters.Add("River", 0);
        tileCounters.Add("Mountain", 0);
        tileCounters.Add("Swamp", 0);

        tileCounters.Add("Desert", 0);
        tileCounters.Add("Thicket", 0);
        tileCounters.Add("Tundra", 0);
        tileCounters.Add("Cavern", 0);
        tileCounters.Add("Settlement", 0);
        tileCounters.Add("Seashore", 0);

        //Add starting bonus stats
        bonusStats.Clear();
        bonusStats.Add("Neutral", 0);
        bonusStats.Add("Fire", 0);
        bonusStats.Add("Ice", 0);
        bonusStats.Add("Thunder", 0);
        bonusStats.Add("Holy", 0);
        bonusStats.Add("Swift", 0);
        bonusStats.Add("Cosmic", 0);
        bonusStats.Add("EquipmentLvl", 1);

        currTier = "Tier1";
        monsterCount = 0;
        allMonstersAreSpawned = false;
    }
}
