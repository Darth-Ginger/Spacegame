using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;
using NaughtyAttributes;
using CustomInspector;
using Spacegame;

namespace Spacegame.Gameplay
{
	public enum NeighborType
	{
		NorthEast,
		North,
		NorthWest,
		SouthWest,
		South,
		SouthEast,
		Indirect
	}

	public enum TileType
	{
		OpenSpace,
		Nebula,
		AsteroidField,
		Base
	}


	/// <summary>
	/// Enumeration for card costs.
	/// </summary>
	public enum CostEnum
	{
		O = 0,
		I = 1,
		II = 2,
		III = 3,
		IV = 4,
		V = 5,
		VI = 6,
		VII = 7,
		VIII = 8,
		IX = 9,
		X = 10
	}

	/// <summary>
	/// Enumeration for card ranks.
	/// </summary>
	public enum RankEnum
	{
		Unranked = 0,
		One = 1,
		Two = 2,
		Three = 3,
	}
	[Flags]
	public enum TargetType
	{
		None = 1 << 0,      // No target - General use -> turn/round/gameplay alter effect
		Self = 1 << 1,      // Self      - General use -> Apply effect to unit that owns the card
		Ally = 1 << 2,      // Ally		 - General use -> Apply effect to unit that is an ally
		Enemy = 1 << 3,     // Enemy 	 - General use -> Apply effect to unit that is an enemy
		Structure = 1 << 4,     // Structure - General use -> Apply effect to structure
		Board = 1 << 5, // Board	 - General use -> Apply effect to a single tile, group of tiles, or all tiles
		All = 1 << 6,   // All		 - General use -> Apply effect to any units, structures, or tiles
	}

	public enum EffectTiming
	{
		Immediate,   // Trigger immediately
		NextTurn,    // Trigger on Next Turn
		FutureTurn,  // Trigger on Future Turn
		NextRound,   // Trigger on Next Round
		FutureRound, // Trigger on Future Round
		OnEnter,     // Trigger when Tile/Structure is entered
		OnExit,      // Trigger when Tile/Structure is exited
		OnUpdate     // Trigger when Tile/Structure is updated
	}

	[Flags]
	public enum EffectType
	{
		Damage = 1 << 1,  // Instant damage
		DamageOverTime = 1 << 2,  // Damage over several turns/rounds
		Heal = 1 << 3,  // Instant heal
		HealOverTime = 1 << 4,  // Heal over several turns/rounds
		Condition = 1 << 5,  // Instant condition (may be persistent)
	}

	public enum RepStatus
	{
		None,
		Friend,
		Enemy,
		Neutral
	}

	public enum SortMethod
	{
		None,
		Ascending,
		Descending,
	}
	public static class EnumManager
	{
		public static T GetRandom<T>() where T : Enum
		{
			T[] TList = (T[])Enum.GetValues(typeof(T));
			return TList.Take<T>(1)
						.ElementAt(UnityRandom.Range(0, TList.Length));
		}

	}
}