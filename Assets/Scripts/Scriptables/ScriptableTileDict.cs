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
using static Board;
using System.Linq;

/// <summary>
/// ScriptableTileDict
/// Container for all <see cref="Tile"/> prefabs for a given <see cref="TileType"/>
/// </summary>
[CreateAssetMenu(fileName = "TileDict", menuName = "ScriptableTileDict")]
public class ScriptableTileDict : ScriptableObject
{   
    [SerializedDictionary("Base Tile", "Tile Prefab")]
    public SerializedDictionary<string, GameObject> TileList;
        
    public GameObject GetPrefab(string name) => TileList[name];
    public List<GameObject> GetPrefabs() => TileList.ToList().Select(_ => _.Value).ToList();
    public List<string> GetNames() => TileList.ToList().Select(_ => _.Key).ToList();
}


