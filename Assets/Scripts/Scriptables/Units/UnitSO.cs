using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;
using NaughtyAttributes;
using CustomInspector;

namespace Spacegame.Gameplay
{
	[CreateAssetMenu(menuName = "Spacegame/Units/UnitSO", fileName = "UnitSO")]
	public class UnitSO : ScriptableObject
	{
		public string Name;
		public int 	  MaxHull;
		public int    MaxShield;
		public int    MaxSpeed;
        public int Hardpoints => HighEnergyHP + LowEnergyHP + MissileHP + OtherHP;
        [BoxGroup("HardPoints")]
        public int    HighEnergyHP;
		[BoxGroup("HardPoints")]
		public int 	  LowEnergyHP;
		[BoxGroup("HardPoints")]
		public int    MissileHP;
		[BoxGroup("HardPoints")]
		public int    OtherHP;

		public int    MaxCargoCapacity;
		public int    MaxCargoMass;


	}
}