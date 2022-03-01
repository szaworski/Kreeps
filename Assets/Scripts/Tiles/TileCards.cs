using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCards : MonoBehaviour
{
    public string cardType;

    [System.NonSerialized] public List<string> tier1TileCards = new List<string> { "Forrest", "Graveyard", "Test1", "Test2", "Test3" }; 

    void Update()
    {
        
    }
}
