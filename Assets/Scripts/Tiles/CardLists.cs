using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsLists : MonoBehaviour
{
    [System.NonSerialized] public List<string> tier1TileCards = new List<string> { "Forest", "Graveyard", "Mountain", "River", "Swamp" };
    [System.NonSerialized] public List<string> tier2TileCards = new List<string> { "Desert", "Thicket", "Tundra", "Cavern", "Settlement", "Seashore" };
    //[System.NonSerialized] public List<string> tier3TileCards = new List<string> { "Forest", "Graveyard", "Mountain", "River", "Swamp", "Swamp" };
    //[System.NonSerialized] public List<string> tier4TileCards = new List<string> { "Forest", "Graveyard", "Mountain", "River", "Swamp", "Swamp" };
    //[System.NonSerialized] public List<string> tier5TileCards = new List<string> { "Forest", "Graveyard", "Mountain", "River", "Swamp", "Swamp" };

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

    //Weapon cards
    [System.NonSerialized] public List<string> tier1WeaponCards = new List<string> { "Dagger", "ShortSword", "LongSword", "Spear", "HandAxe", "Mace" };
    [System.NonSerialized] public List<string> tier2WeaponCards = new List<string> { "SilverShortSword", "CharredDagger", "FrostWand", "JoltSabre", "Katana", "AstralSpear" };
    [System.NonSerialized] public List<string> tier3WeaponCards = new List<string> { "Dagger", "ShortSword", "LongSword", "Spear", "HandAxe", "Mace" };
    [System.NonSerialized] public List<string> tier4WeaponCards = new List<string> { "Dagger", "ShortSword", "LongSword", "Spear", "HandAxe", "Mace" };
    [System.NonSerialized] public List<string> tier5WeaponCards = new List<string> { "Dagger", "ShortSword", "LongSword", "Spear", "HandAxe", "Mace" };

    //Power up cards
    [System.NonSerialized] public List<string> powerUpCards = new List<string> { "Neutral", "Fire", "Ice", "Thunder", "Holy", "Swift", "Cosmic" };
}
