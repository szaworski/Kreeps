using System.Collections.Generic;
using UnityEngine;

public class CardsLists : MonoBehaviour
{
    [System.NonSerialized] public List<string> tier1TileCards = new List<string> { "Forest", "Graveyard", "Mountain", "River", "Swamp" };
    [System.NonSerialized] public List<string> tier2TileCards = new List<string> { "Desert", "Thicket", "Tundra", "Cavern", "Settlement", "Seashore" };
    [System.NonSerialized] public List<string> tier3TileCards = new List<string> { "CanyonCrossing", "CrimsonPlain", "Crypt", "EmeraldCave", "Marsh", "Sewer" };
    [System.NonSerialized] public List<string> tier4TileCards = new List<string> { "CrystalCave", "FrozenPassage", "InfernalWoods", "SacredGrounds", "TaintedCanal", "VolcanicRavine" };
    [System.NonSerialized] public List<string> tier5TileCards = new List<string> { "OceanAbyss", "CelestialPlane", "Underworld", "AncestralForest", "MysticMountain", "CorruptedIsle" };

    //Tier 1 Monsters
    [System.NonSerialized] public List<string> forestMonsterCards = new List<string> { "Wolf", "Goblin", "Skeleton", "Owl" };
    [System.NonSerialized] public List<string> graveyardMonsterCards = new List<string> { "Zombie", "Bandit", "Ghost", "Bat" };
    [System.NonSerialized] public List<string> riverMonsterCards = new List<string> { "Bluegill", "Turtle", "Duck", "Monk" };
    [System.NonSerialized] public List<string> mountainMonsterCards = new List<string> { "Golem", "Goat", "Ranger", "Bear" };
    [System.NonSerialized] public List<string> swampMonsterCards = new List<string> { "Spider", "Alligator", "Bullfrog", "Ghoul" };

    //Tier 2 Monsters
    [System.NonSerialized] public List<string> desertMonsterCards = new List<string> { "Mummy", "Scorpion", "Rattlesnake", "Camel" };
    [System.NonSerialized] public List<string> thicketMonsterCards = new List<string> { "Wild Boar", "Minotaur", "Panther", "Giant Spider" };
    [System.NonSerialized] public List<string> tundraMonsterCards = new List<string> { "Polar Bear", "Arctic Wolf", "Penguin", "Lich" };
    [System.NonSerialized] public List<string> cavernMonsterCards = new List<string> { "Ogre", "Miner", "Wraith", "Skeleton Warrior" };
    [System.NonSerialized] public List<string> settlementMonsterCards = new List<string> { "Knight", "Sheep", "Farmer", "Rat" };
    [System.NonSerialized] public List<string> seashoreMonsterCards = new List<string> { "Crab", "Drifter", "Fisherman", "Sea Serpent" };

    //Tier 3 Monsters
    [System.NonSerialized] public List<string> canyonCrossingMonsterCards = new List<string> { "Hunter", "Cavalry", "Pack Mule", "Falcon" };
    [System.NonSerialized] public List<string> crimsonPlainMonsterCards = new List<string> { "Goblin Rider", "Imp", "Crimson Wolf", "Bison" };
    [System.NonSerialized] public List<string> cryptMonsterCards = new List<string> { "Fiend", "Cultist", "Crawler", "Flying Skull" };
    [System.NonSerialized] public List<string> emeraldCaveMonsterCards = new List<string> { "Gemstone Golem", "Jewel Thief", "Spectral Bear", "Badger" };
    [System.NonSerialized] public List<string> marshMonsterCards = new List<string> { "Water Demon", "Reanimated Wolf", "Poison Toad", "Giant Slug" };
    [System.NonSerialized] public List<string> sewerMonsterCards = new List<string> { "Slime", "Giant Roach", "Vile Wraith", "Dung Devil" };

    //Tier 4 Monsters
    [System.NonSerialized] public List<string> crystalCaveMonsterCards = new List<string> { "Crystalline Ogre", "Crystalline Zombie", "Diamond Gecko", "Geologist" };
    [System.NonSerialized] public List<string> frozenPassageMonsterCards = new List<string> { "Frost Giant", "Arctic Fox", "Frost Ghoul", "Reindeer" };
    [System.NonSerialized] public List<string> infernalWoodsMonsterCards = new List<string> { "Cinder Goblin", "Smoldering Skeleton", "Raven", "Orc Warrior" };
    [System.NonSerialized] public List<string> sacredGroundsMonsterCards = new List<string> { "Priest", "Paladin", "Ghastly Imp", "Death Knight" };
    [System.NonSerialized] public List<string> taintedCanalMonsterCards = new List<string> { "Viper", "Toxic Wolf", "Plague Bear", "Basilisk" };
    [System.NonSerialized] public List<string> volcanicRavineMonsterCards = new List<string> { "Magma Golem", "Lava Demon", "Necromancer", "Phoenix" };

    //Tier 5 Monsters
    [System.NonSerialized] public List<string> ancestralForestMonsterCards = new List<string> { "Crystalline Ogre", "Crystalline Zombie", "Diamond Gecko", "Geologist" };
    [System.NonSerialized] public List<string> celestialPlaneMonsterCards = new List<string> { "Frost Giant", "Arctic Fox", "Frost Ghoul", "Reindeer" };
    [System.NonSerialized] public List<string> corruptedIsleMonsterCards = new List<string> { "Cinder Goblin", "Smoldering Skeleton", "Raven", "Orc Warrior" };
    [System.NonSerialized] public List<string> mysticMountainMonsterCards = new List<string> { "Priest", "Paladin", "Ghastly Imp", "Death Knight" };
    [System.NonSerialized] public List<string> oceanAbyssMonsterCards = new List<string> { "Viper", "Toxic Wolf", "Plague Bear", "Basilisk" };
    [System.NonSerialized] public List<string> underworldMonsterCards = new List<string> { "Magma Golem", "Lava Demon", "Necromancer", "Phoenix" };

    //Weapon cards
    [System.NonSerialized] public List<string> tier1WeaponCards = new List<string> { "Dagger", "ShortSword", "LongSword", "Spear", "HandAxe", "Mace" };
    [System.NonSerialized] public List<string> tier2WeaponCards = new List<string> { "SilverShortSword", "CharredDagger", "FrostWand", "JoltSabre", "TwinDaggers", "CosmicSpear" };
    [System.NonSerialized] public List<string> tier3WeaponCards = new List<string> { "GoldenShortSword", "EmberBattleAxe", "FrostLongSword", "ShockLance", "SacredStaff", "Katana", "AstralGreatSword" };
    [System.NonSerialized] public List<string> tier4WeaponCards = new List<string> { "Dagger", "ShortSword", "LongSword", "Spear", "HandAxe", "Mace", "" };

    //Power up cards
    [System.NonSerialized] public List<string> powerUpCards = new List<string> { "Neutral", "Fire", "Ice", "Thunder", "Holy", "Swift", "Cosmic", "FireSpeedUp", "IceSlowUp", "ThunderArmorDmgUp", "HolyRangeUp", "SwiftPsnDmgUp", "CosmicCritChanceUp" };
}
