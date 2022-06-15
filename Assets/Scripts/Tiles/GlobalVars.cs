using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    //Tier 1 Tiles counts
    [HideInInspector] public static int numOfForests;
    [HideInInspector] public static int numOfGraveyards;
    [HideInInspector] public static int numOfRivers;
    [HideInInspector] public static int numOfMountains;
    [HideInInspector] public static int numOfSwamps;

    //Tier 2 Tiles counts
    [HideInInspector] public static int numOfDeserts;
    [HideInInspector] public static int numOfThickets;
    [HideInInspector] public static int numOfTundras;
    [HideInInspector] public static int numOfCaverns;
    [HideInInspector] public static int numOfSettlements;
    [HideInInspector] public static int numOfSeashores;

    //Vars used for generating card choices
    [HideInInspector] public static string tileName;
    [HideInInspector] public static bool triggerTileCardDestruction;
    [HideInInspector] public static bool triggerMonsterCardDestruction;
    [HideInInspector] public static bool triggerShopCardDestruction;
    [HideInInspector] public static string tileCardSelected;
    [HideInInspector] public static string monsterCardSelected;
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

    //Tower grid Global vars
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

    //Monster Manager global vars
    public static int monsterCount;
    public static bool allMonstersAreSpawned;
}
