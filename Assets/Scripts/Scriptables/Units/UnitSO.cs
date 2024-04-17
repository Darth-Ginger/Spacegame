using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;
using NaughtyAttributes;
using CustomInspector;
using Utility;

namespace Spacegame.Gameplay
{
	[CreateAssetMenu(menuName = "Spacegame/Units/UnitSO", fileName = "UnitSO")]
	public class UnitSO : SerializableScriptableObject 
	{
		public string Name;
		public int 	  MaxHullpoints; // Indicator of physical health
		public int    MaxShield;	 // Supplements Hullpoints
		public int    MaxSpeed;		 // Determined by combination of Engine power, Overall Mass, etc
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