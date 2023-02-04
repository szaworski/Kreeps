using System.Collections.Generic;
using UnityEngine;

public class CardLists : MonoBehaviour
{
    public Dictionary<string, string[]> monsterCards = new Dictionary<string, string[]>()
    {
        {"Forest", forestMonsterCards},
        {"Graveyard", graveyardMonsterCards},
        {"River", riverMonsterCards},
        {"Mountain", mountainMonsterCards},
        {"Swamp", swampMonsterCards},

        {"Desert", desertMonsterCards},
        {"Thicket", thicketMonsterCards},
        {"Tundra", tundraMonsterCards},
        {"Cavern", cavernMonsterCards},
        {"Settlement", settlementMonsterCards},
        {"Seashore", seashoreMonsterCards},

        {"CanyonCrossing", canyonCrossingMonsterCards},
        {"CrimsonPlain", crimsonPlainMonsterCards},
        {"Crypt", cryptMonsterCards},
        {"EmeraldCave", emeraldCaveMonsterCards},
        {"Marsh", marshMonsterCards},
        {"Sewer", sewerMonsterCards},

        {"CrystalCave", crystalCaveMonsterCards},
        {"FrozenPassage", frozenPassageMonsterCards},
        {"InfernalWoods", infernalWoodsMonsterCards},
        {"SacredGrounds", sacredGroundsMonsterCards},
        {"TaintedCanal", taintedCanalMonsterCards},
        {"VolcanicRavine", volcanicRavineMonsterCards},

        {"AncestralForest", ancestralForestMonsterCards},
        {"CelestialPlane", celestialPlaneMonsterCards},
        {"CorruptedIsle", corruptedIsleMonsterCards},
        {"MysticMountain", mysticMountainMonsterCards},
        {"OceanAbyss", oceanAbyssMonsterCards},
        {"Underworld", underworldMonsterCards}
    };

    public Dictionary<int, string[]> weaponCards = new Dictionary<int, string[]>()
    {
        {1, tier1WeaponCards},
        {2, tier2WeaponCards},
        {3, tier3WeaponCards},
        {4, tier4WeaponCards}
    };

    //Location cards
    [System.NonSerialized] public static string[] tier1TileCards = new string[] { "Forest", "Graveyard", "Mountain", "River", "Swamp" };
    [System.NonSerialized] public static string[] tier2TileCards = new string[] { "Desert", "Thicket", "Tundra", "Cavern", "Settlement", "Seashore" };
    [System.NonSerialized] public static string[] tier3TileCards = new string[] { "CanyonCrossing", "CrimsonPlain", "Crypt", "EmeraldCave", "Marsh", "Sewer" };
    [System.NonSerialized] public static string[] tier4TileCards = new string[] { "CrystalCave", "FrozenPassage", "InfernalWoods", "SacredGrounds", "TaintedCanal", "VolcanicRavine" };
    [System.NonSerialized] public static string[] tier5TileCards = new string[] { "OceanAbyss", "CelestialPlane", "Underworld", "AncestralForest", "MysticMountain", "CorruptedIsle" };

    //Tier 1 Monsters
    [System.NonSerialized] public static string[] forestMonsterCards = new string[] { "Wolf", "Goblin", "Skeleton", "Owl" };
    [System.NonSerialized] public static string[] graveyardMonsterCards = new string[] { "Zombie", "Bandit", "Ghost", "Bat" };
    [System.NonSerialized] public static string[] riverMonsterCards = new string[] { "Bluegill", "Turtle", "Duck", "Monk" };
    [System.NonSerialized] public static string[] mountainMonsterCards = new string[] { "Golem", "Goat", "Ranger", "Bear" };
    [System.NonSerialized] public static string[] swampMonsterCards = new string[] { "Spider", "Alligator", "Bullfrog", "Ghoul" };

    //Tier 2 Monsters
    [System.NonSerialized] public static string[] desertMonsterCards = new string[] { "Mummy", "Scorpion", "Rattlesnake", "Camel" };
    [System.NonSerialized] public static string[] thicketMonsterCards = new string[] { "Ogre", "Minotaur", "Panther", "Giant Spider" };
    [System.NonSerialized] public static string[] tundraMonsterCards = new string[] { "Polar Bear", "Arctic Wolf", "Penguin", "Lich" };
    [System.NonSerialized] public static string[] cavernMonsterCards = new string[] { "Rat", "Miner", "Wraith", "Skeleton Warrior" };
    [System.NonSerialized] public static string[] settlementMonsterCards = new string[] { "Knight", "Sheep", "Farmer", "Wild Boar" };
    [System.NonSerialized] public static string[] seashoreMonsterCards = new string[] { "Crab", "Drifter", "Fisherman", "Sea Serpent" };

    //Tier 3 Monsters
    [System.NonSerialized] public static string[] canyonCrossingMonsterCards = new string[] { "Hunter", "Cavalry", "Pack Mule", "Falcon" };
    [System.NonSerialized] public static string[] crimsonPlainMonsterCards = new string[] { "Goblin Rider", "Imp", "Crimson Wolf", "Bison" };
    [System.NonSerialized] public static string[] cryptMonsterCards = new string[] { "Fiend", "Cultist", "Crawler", "Flying Skull" };
    [System.NonSerialized] public static string[] emeraldCaveMonsterCards = new string[] { "Gemstone Golem", "Jewel Thief", "Spectral Bear", "Badger" };
    [System.NonSerialized] public static string[] marshMonsterCards = new string[] { "Water Demon", "Reanimated Wolf", "Poison Toad", "Giant Slug" };
    [System.NonSerialized] public static string[] sewerMonsterCards = new string[] { "Slime", "Giant Roach", "Vile Wraith", "Dung Devil" };

    //Tier 4 Monsters
    [System.NonSerialized] public static string[] crystalCaveMonsterCards = new string[] { "Crystalline Ogre", "Crystalline Zombie", "Diamond Gecko", "Geologist" };
    [System.NonSerialized] public static string[] frozenPassageMonsterCards = new string[] { "Frost Giant", "Arctic Fox", "Frost Ghoul", "Reindeer" };
    [System.NonSerialized] public static string[] infernalWoodsMonsterCards = new string[] { "Cinder Goblin", "Smoldering Skeleton", "Raven", "Orc Warrior" };
    [System.NonSerialized] public static string[] sacredGroundsMonsterCards = new string[] { "Priest", "Paladin", "Ghastly Imp", "Death Knight" };
    [System.NonSerialized] public static string[] taintedCanalMonsterCards = new string[] { "Viper", "Toxic Wolf", "Plague Bear", "Basilisk" };
    [System.NonSerialized] public static string[] volcanicRavineMonsterCards = new string[] { "Magma Golem", "Lava Demon", "Necromancer", "Phoenix" };

    //Tier 5 Monsters
    [System.NonSerialized] public static string[] ancestralForestMonsterCards = new string[] { "Ancestral Spirit", "Great Wolf", "Forest Guardian", "Earth Elemental" };
    [System.NonSerialized] public static string[] celestialPlaneMonsterCards = new string[] { "Celestial Imp", "Celestial Serpent", "Astronaut", "Martian" };
    [System.NonSerialized] public static string[] corruptedIsleMonsterCards = new string[] { "Rot Raven", "Festering Ghoul", "Noxious Ogre", "Blight Fiend" };
    [System.NonSerialized] public static string[] mysticMountainMonsterCards = new string[] { "Arcane Eagle", "Arcane Ghoul", "Rune Bear", "Mystic Mage" };
    [System.NonSerialized] public static string[] oceanAbyssMonsterCards = new string[] { "Colossal Shrimp", "Ghost Ship", "Pirate", "Dolphin" };
    [System.NonSerialized] public static string[] underworldMonsterCards = new string[] { "Chaos Demon", "Skeleton Lord", "Blood Mage", "Hell Hound" };

    //Weapon cards
    [System.NonSerialized] public static string[] tier1WeaponCards = new string[] { "ShortSword", "Dagger", "LongSword", "Spear", "HandAxe", "Mace" };
    [System.NonSerialized] public static string[] tier2WeaponCards = new string[] { "SilverShortSword", "CharredDagger", "FrostWand", "JoltSabre", "TwinDaggers", "CosmicSpear" };
    [System.NonSerialized] public static string[] tier3WeaponCards = new string[] { "GoldenShortSword", "EmberBattleAxe", "FrostLongSword", "ShockLance", "SacredStaff", "Katana", "AstralGreatSword" };
    [System.NonSerialized] public static string[] tier4WeaponCards = new string[] { "TitaniumShortSword", "InfernalLongSword", "BlizzardBroadSword", "LightningGreatAxe", "HeavenlyGreatSword", "ReinforcedMorningStar", "GalacticScepter" };

    //Power up cards
    [System.NonSerialized] public static string[] powerUpCards = new string[] { "Neutral", "Fire", "Ice", "Thunder", "Holy", "Swift", "Cosmic", "FireSpeedUp", "IceSlowUp", "ThunderArmorDmgUp", "HolyRangeUp", "SwiftPsnDmgUp", "CosmicCritChanceUp", "FireBurnUp", "IceRangeUp", "ThunderSpeedUp", "HolyBlessingDmgUp", "SwiftSpeedUp", "CosmicRangeUp" }; //"StonePwr1", "StonePwr2"
}