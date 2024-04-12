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

	public static class EnumManager
	{
        public static T GetRandom<T>() where T : Enum
        {
			T[] TList = (T[]) Enum.GetValues(typeof(T));
            return TList.Take<T>(1)
                        .ElementAt(UnityRandom.Range(0, TList.Length));
        }
    }
}