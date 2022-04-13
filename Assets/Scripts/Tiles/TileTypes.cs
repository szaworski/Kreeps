using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypes : CardsLists
{
    //These arrays need to be set to NonSerizlized or else the contents won't update when edited
    [System.NonSerialized] public string[] forrestTiles =  new string [] { "ForrestTile1", "ForrestTile2", "ForrestTile3", "ForrestTile4", "ForrestTile5", "ForrestTile6" };
    [System.NonSerialized] public string[] graveyardTiles = new string[] { "GraveyardTile1", "GraveyardTile2", "GraveyardTile3", "GraveyardTile4", "GraveyardTile5", "GraveyardTile6" };
    [System.NonSerialized] public string[] riverTiles = new string[] { "RiverTile1", "RiverTile2", "RiverTile3", "RiverTile4", "RiverTile5", "RiverTile6" };
    [System.NonSerialized] public string[] mountainTiles = new string[] { "MountainTile1", "MountainTile2", "MountainTile3", "MountainTile4", "MountainTile5", "MountainTile6" };
    [System.NonSerialized] public string[] swampTiles = new string[] { "SwampTile1", "SwampTile2", "SwampTile3", "SwampTile4", "SwampTile5", "SwampTile6" };
}
