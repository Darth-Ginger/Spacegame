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
	[CreateAssetMenu(menuName = "Spacegame/Units/FactionSO", fileName = "FactionSO")]
	public class FactionSO : ScriptableObject
	{
		
		public string 	 Name 	   { get; private set; }
		public RepStatus RepStatus { get; private set; }
		public int 		 Rep 	   { get; private set; } = 0;

		//@todo Add faction specific items
		// Factors to set faction aggression, defense, etc.
		// ?int ForcePower? from 1-100 determining number of available units
		// ?int Morale? 	from 1-100 determining ability to fight
		// ?int Aggression? from 1-100 determining liklihood to attack
		// ?int Defense? 	from 1-100 determining liklihood to defend
		// etc.
	}
}
