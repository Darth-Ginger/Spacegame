using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using static Spacegame.Gameplay.Board;
using System.Linq;

namespace Spacegame.Gameplay.Tiles{
/// <summary>
/// ScriptableTileDict
/// Container for all <see cref="Tile"/> prefabs for a given <see cref="TileType"/>
/// </summary>
[CreateAssetMenu(fileName = "TileDict", menuName = "ScriptableTileDict")]
public class ScriptableTileDict : ScriptableObject
{   
    public GameObject BasePrefab;

    [SerializedDictionary("Base Tile", "Tile Prefab")]
    public SerializedDictionary<string, TileSO> TileList;

    private List<GameObject> _prefabs;

    public GameObject       GetPrefab(string name) => BuildPrefab(name);
    public List<GameObject> GetPrefabs() => _prefabs.ToList();
    public List<string>     GetTileNames() => TileList.ToList().Select(_ => _.Key).ToList();

    private GameObject BuildPrefab(string name)
    {
        GameObject prefab = BasePrefab;
        prefab.name = name + "_Tile";
        Tile tile = prefab.AddComponent<Tile>();
        bool specAssigned = tile.SetTileSpecs(TileList[name]);
        
        if (!specAssigned) Debug.LogError("TileSpecs not set for " + name);
        
        return prefab;
    }

    private void OnValidate()
    {
        foreach (var tile in TileList) _prefabs.Add(BuildPrefab(tile.Key));
    }

}
}


