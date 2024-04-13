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
using NaughtyAttributes;

namespace Spacegame.Gameplay.Tiles{
/// <summary>
/// ScriptableTileDict
/// Container for all <see cref="Tile">Tiles</see> 
/// </summary>
[CreateAssetMenu(fileName = "TileDict", menuName = "ScriptableTileDict")]
public class ScriptableTileDict : ScriptableObject
{   
    public GameObject BasePrefab;

    [SerializedDictionary("Base Tile", "Tile Prefab")]
    public SerializedDictionary<string, TileSO> TileList;

    [Button("Refresh Prefabs")]
    public void DRefreshPrefabs() => RefreshPrefabs();

    private List<GameObject> _prefabs;

    public GameObject       GetPrefab(string name)  => _prefabs.Find(_ => _.name == name + "_Tile");
    public List<GameObject> GetPrefabs()            => _prefabs;
    public List<string>     GetTileNames()          => TileList.ToList().Select(_ => _.Key).ToList();

    private GameObject BuildPrefab(string name)
    {
        
        GameObject prefab = PrefabUtility.InstantiatePrefab(BasePrefab) as GameObject;
        prefab.name = name + "_Tile";
        Tile tile = prefab.AddComponent<Tile>();
        bool specAssigned = tile.SetTileSpecs(TileList[name]);
        
        if (!specAssigned) {
            Debug.LogError("TileSpecs not set for " + name);
            DestroyImmediate(prefab);
            return null;
        }
      
        var finalPrefab = PrefabUtility.SaveAsPrefabAsset(prefab, $"Assets/Prefab/Tiles/{prefab.name}.prefab");
            // Adds the Prefab to the TileSO
            if (TileList[name].GetGameObject() != null && TileList[name].GetGameObject() != finalPrefab)
            {
                // Adds the Prefab to the TileSO
                TileList[name].SetGameObject(finalPrefab);
            }
            
            // Destroys the Instantiated GameObject
            DestroyImmediate(prefab);
        return finalPrefab;
    }

    private void RefreshPrefabs()
    {
        if (TileList == null || TileList.Keys == null || TileList.Values == null) return;
        _prefabs ??= new List<GameObject>();
        foreach (var tile in TileList) 
        {
            GameObject newPrefab = BuildPrefab(tile.Key);
            if (_prefabs.Contains(newPrefab)) continue;
            _prefabs.Add(newPrefab);
        }
    }

}
}


