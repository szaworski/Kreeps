using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    //Tier 1 Tiles counts
    [HideInInspector] public int numOfForests;
    [HideInInspector] public int numOfGraveyards;
    [HideInInspector] public int numOfRivers;
    [HideInInspector] public int numOfMountains;
    [HideInInspector] public int numOfSwamps;

    //Tier 2 Tiles counts
    [HideInInspector] public int numOfDeserts;
    [HideInInspector] public int numOfThickets;
    [HideInInspector] public int numOfTundras;
    [HideInInspector] public int numOfCaverns;
    [HideInInspector] public int numOfSettlements;
    [HideInInspector] public int numOfSeashores;

    //Vars used for generating card choices
    [HideInInspector] public bool triggerTileCardDestruction;
    [HideInInspector] public bool triggerMonsterCardDestruction;
    [HideInInspector] public bool triggerShopCardDestruction;
    [HideInInspector] public string newTileName;
    [HideInInspector] public string tileName;
    [HideInInspector] public string prependTileName;
    [HideInInspector] public string spawnDirection;
    [HideInInspector] public string tileCardSelected;
    [HideInInspector] public string monsterCardSelected;
    public string currTier;
    public int numOfTimesPlaced;

    //Power up vars
    public static int bonusNormalDmg;
    public static int bonusFireDmg;
    public static int bonusIceDmg;
    public static int bonusThunderDmg;
    public static int bonusHolyDmg;
    public static int bonusSwiftDmg;
    public static int bonusCosmicDmg;
    public static int equipmentLvl;
}
