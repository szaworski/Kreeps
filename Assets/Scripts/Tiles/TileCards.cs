using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCards : MonoBehaviour
{
    public string cardType;

    [System.NonSerialized] public List<string> tier1TileCards = new List<string> { "Forrest", "Graveyard", "River", "Mountain", "Swamp" }; 

    void Update()
    {
        
    }
}
