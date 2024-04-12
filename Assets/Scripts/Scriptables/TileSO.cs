using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;
using NaughtyAttributes;
using CustomInspector;
using UnityEngine.Events;

namespace Spacegame.Gameplay.Tiles
{
	[CreateAssetMenu(menuName = "Configs/TileSO", fileName = "TileSO")]
	public class TileSO : ScriptableObject
	{
		public  string 		Name;
		public  TileType 	TileType		= TileType.OpenSpace;
		public  double 		CostModifier 	= 1d;
		public  bool		Pathable		= true;			
		public  bool		Visibility		= true;
		public  GameObject 	GameObject; 
		//@todo Add fields for Tile Properties or effects
		public  UnityEvent TileEffects 	= null;

	}
}