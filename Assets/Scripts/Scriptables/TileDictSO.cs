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
using ReadOnlyAttribute = NaughtyAttributes.ReadOnlyAttribute;

namespace Spacegame.Gameplay
{
    /// <summary>
    /// ScriptableTileDict
    /// Container for all <see cref="Tile">Tiles</see> 
    /// </summary>
    [CreateAssetMenu(fileName = "TileDict", menuName = "Spacegame/Tiles/ScriptableTileDict")]
    public class ScriptableTileDict : ScriptableObject
    {
        public GameObject BasePrefab;

        [SerializedDictionary("Base Tile", "Tile Prefab")]
        public SerializedDictionary<string, TileSO> TileList;
        
        [ShowNativeProperty]
        private bool PrefabsExists { get { return HasPrefabs(); } set => HasPrefabs(); }
        
        [Button("Refresh Prefabs")]
        public void DRefreshPrefabs() => Prefabs = RefreshPrefabs();
        
        [SerializeField][ReadOnly]
        private GameObject[] _prefabs;
        public List<GameObject> Prefabs
        {
            get { return _prefabs.ToList(); }
            private set { _prefabs = value.ToArray(); RefreshPrefabs(); }
        }

        public GameObject GetPrefab(string name) => Prefabs.Find(_ => _.name == name + "_Tile");
        public List<GameObject> GetPrefabs() => Prefabs;
        public List<string> GetTileNames() => TileList.ToList().Select(_ => _.Key).ToList();

        private GameObject BuildPrefab(string name)
        {

            GameObject prefab = PrefabUtility.InstantiatePrefab(BasePrefab) as GameObject;
            prefab.name = name + "_Tile";
            Tile tile = prefab.AddComponent<Tile>();
            bool specAssigned = tile.SetTileSpecs(TileList[name]);

            if (!specAssigned)
            {
                Debug.LogError("TileSpecs not set for " + name);
                DestroyImmediate(prefab);
                return null;
            }

            var finalPrefab = PrefabUtility.SaveAsPrefabAsset(prefab, $"Assets/Prefab/Tiles/{prefab.name}.prefab");
            // Adds the Prefab to the TileSO
            TileList[name].SetGameObject(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(finalPrefab)));
            

            // Destroys the Instantiated GameObject
            DestroyImmediate(prefab);
            return finalPrefab;
        }

        private List<GameObject> RefreshPrefabs()
        {
            if (TileList == null || TileList.Keys == null || TileList.Values == null) return null;
            var SetPrefabs = new List<GameObject>();
            foreach (var tile in TileList)
            {
                GameObject newPrefab = BuildPrefab(tile.Key);
                if (Prefabs.Contains(newPrefab)) continue;
                SetPrefabs.Add(newPrefab);
            }

            return SetPrefabs;
        }
        private bool HasPrefabs()
        {
            List<bool> _prefabsExist = new();
            // if (!AssetDatabase.IsValidFolder("Assets/Prefab/Tiles")) return false;
            if (TileList == null || TileList.Keys == null || TileList.Values == null || TileList.Count == 0) return false;
            if (Prefabs.Count == 0) return false;

            var prefabTiles = AssetDatabase.FindAssets("", new[] {"Assets/Prefab/Tiles"});
            _prefabsExist.AddRange(from prefab in _prefabs
                                   where prefab != null
                                   where prefabTiles.Contains(
                                            AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefab))
                                            )
                                   select true);
            // Debugging for checking if all prefabs exist
            // Debug.Log("Prefabs Exist: " + _prefabsExist.Count);
            // foreach (var _ in _prefabsExist) Debug.Log(_);
            // Debug.Log("PrefabTiles : " + prefabTiles.Length);
            // foreach (var _ in prefabTiles) Debug.Log(_);
            
            return Prefabs.Count == _prefabsExist.Count && _prefabsExist.All(_ => _ == true);
        }

        public void OnValidate()
        {
            PrefabsExists = HasPrefabs();
        }

    }
}


