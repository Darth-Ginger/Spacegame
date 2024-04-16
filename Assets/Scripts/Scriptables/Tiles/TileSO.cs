using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;
using NaughtyAttributes;

using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

namespace Spacegame.Gameplay
{
	[CreateAssetMenu(menuName = "Spacegame/Tiles/TileSO", fileName = "TileSO")]
	public class TileSO : ScriptableObject
	{
		[BoxGroup("Tile Specs")]
		public  string 		Name;
		[BoxGroup("Tile Specs")]
		public  TileType 	TileType		= TileType.OpenSpace;
		[BoxGroup("Tile Specs")]
		public  double 		CostModifier 	= 1d;
		[BoxGroup("Tile Specs")]
		public  bool		Pathable		= true;			
		[BoxGroup("Tile Specs")]
		public  bool		Visibility		= true;
		[BoxGroup("Tile Effects")]
		public  UnityEvent  TileEffects 	= null;
		
		[ReadOnly][SerializedDictionary("Tile Name", "Tile Prefab")]
		public  SerializedDictionary<string,GameObject> Prefabs = new();
		//@todo Add fields for Tile Properties or effects

		public  void  SetGameObject(GameObject gameObject) => SetGameObject(new KeyValuePair<string, GameObject>(gameObject.name, gameObject));
		private void  SetGameObject(KeyValuePair<string, GameObject> gameObject) => Prefabs.TryAdd(gameObject.Key, gameObject.Value);
		
		public GameObject GetGameObject() => Prefabs.FirstOrDefault().Value;
        public GameObject GetGameObject(GameObject gameObject) => Prefabs[gameObject.name];
		public GameObject GetGameObject(string name = "") 
		{
			if (name == "") return GetGameObject();
			return Prefabs[name];
		}
    }
}
