using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsLists : MonoBehaviour
{
    [System.NonSerialized] public List<string> tier1TileCards = new List<string> { "Forrest", "Graveyard", "Mountain", "River", "Swamp" };
    [System.NonSerialized] public List<string> tier2TileCards = new List<string> { "Desert", "Thicket", "Tundra", "Cavern", "Settlement", "Seashore" };
    //[System.NonSerialized] public List<string> tier3TileCards = new List<string> { "Forrest", "Graveyard", "Mountain", "River", "Swamp" };

    //Tier 1 Monsters
    [System.NonSerialized] public List<string> forrestMonsterCards = new List<string> { "Wolf", "Goblin", "Bandit", "Owl" };
    [System.NonSerialized] public List<string> graveyardMonsterCards = new List<string> { "Zombie", "Skeleton", "Ghost", "Bat" };
    [System.NonSerialized] public List<string> riverMonsterCards = new List<string> { "Bluegill", "Turtle", "Duck", "Monk" };
    [System.NonSerialized] public List<string> mountainMonsterCards = new List<string> { "Golem", "Goat", "Ranger", "Bear" };
    [System.NonSerialized] public List<string> swampMonsterCards = new List<string> { "Spider", "Alligator", "Bullfrog", "Ghoul" };

    //Tier 2 Monsters
    [System.NonSerialized] public List<string> desertMonsterCards = new List<string> { "Mummy", "Scorpion", "Rattlesnake", "Camel" };
    [System.NonSerialized] public List<string> thicketMonsterCards = new List<string> { "Orc", "Minotaur", "Panther", "Giant spider" };
    [System.NonSerialized] public List<string> tundraMonsterCards = new List<string> { "Polar bear", "Arctic wolf", "Penguin", "Lich" };
    [System.NonSerialized] public List<string> cavernMonsterCards = new List<string> { "Troll", "Miner", "", "" };
    [System.NonSerialized] public List<string> settlementMonsterCards = new List<string> { "Knight", "Sheep", "Farmer", "" };
    [System.NonSerialized] public List<string> seashoreMonsterCards = new List<string> { "Crab", "", "", "" };
}
