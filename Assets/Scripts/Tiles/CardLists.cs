using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsLists : MonoBehaviour
{
    [System.NonSerialized] public List<string> tier1TileCards = new List<string> { "Forrest", "Graveyard", "Mountain", "River", "Swamp" };
    //[System.NonSerialized] public List<string> tier2TileCards = new List<string> { "Forrest", "Graveyard", "Mountain", "River", "Swamp" };
    //[System.NonSerialized] public List<string> tier3TileCards = new List<string> { "Forrest", "Graveyard", "Mountain", "River", "Swamp" };

    [System.NonSerialized] public List<string> forrestMonsterCards = new List<string> { "Wolf", "Goblin", "Bandit", "Owl" };
    [System.NonSerialized] public List<string> graveyardMonsterCards = new List<string> { "Zombie", "Skeleton", "Ghost", "Bat" };
    [System.NonSerialized] public List<string> riverMonsterCards = new List<string> { "Bluegill", "Turtle", "Duck", "Monk" };
    [System.NonSerialized] public List<string> mountainMonsterCards = new List<string> { "Golem", "Goat", "Ranger", "Bear" };
    [System.NonSerialized] public List<string> swampMonsterCards = new List<string> { "Spider", "Alligator", "Bullfrog", "Ghoul" };
}
