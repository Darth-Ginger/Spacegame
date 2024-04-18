using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;
using NaughtyAttributes;
using CustomInspector;
using TigerForge;
using Utility;

namespace Spacegame.Gameplay
{
	[CreateAssetMenu(menuName = "Spacegame/Effects/", fileName = "EffectModSO")]
	public class EffectModSO : SerializableScriptableObject 
	{
		public enum ModType { Add, Sub, Mult, Div, }

        public EffectTiming Timing     { get; private set; } 
        public EffectType   Type 	   { get; private set; } 
		public int          Modifier   { get; private set; } 
		public ModType      ModAction  { get; private set; } 
		
		private void OnEnable()
		{
			Timing     = EffectTiming.Immediate;
			Type 	   = EffectType.Damage;
			Modifier   = 0;
			ModAction  = ModType.Add;
		}

		/// <summary>
		/// Applies the modifier to the given value
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Returns the modified value</returns>
        public int ApplyMod (int value)
		{
            return ModAction switch
            {
                ModType.Add => value + Modifier,
                ModType.Sub => value - Modifier,
                ModType.Mult => value * Modifier,
                ModType.Div => value / Modifier,
                _ => value,
            };
        }
	}
}