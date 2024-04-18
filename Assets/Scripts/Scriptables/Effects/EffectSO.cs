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
using ReadOnlyAttribute = CustomInspector.ReadOnlyAttribute;

namespace Spacegame.Gameplay
{
	// [CreateAssetMenu(menuName = "Spacegame/Effects/", fileName = "Effects")]
	public abstract class EffectSO : SerializableScriptableObject 
	{

        // Attributes to be set in the inspector
		public string 			  Name 	  { get => name; set => _ = name; }
        public EffectTiming 	  Timing  { get; private set; }    
        public EffectType   	  Type 	  { get; private set; } 	 
		public List<EffectModSO>  Mod     { get; private set; } 	 

        // Attributes used for events
        [SerializeField] [ReadOnly] protected GameObject _source;
		[SerializeField] [ReadOnly] protected GameObject _target;

		protected virtual void OnEnable()
		{
			Timing = EffectTiming.Immediate;
			Type   = EffectType.Damage;
			Mod    = new List<EffectModSO>();
		}

		/// <summary>
		/// Applies the effect
		/// </summary>
		public abstract void Apply(GameObject target, GameObject source);
		/// <summary>
		/// Removes the effect
		/// </summary>
		/// <param name="_target"></param>
		/// <param name="_source"></param>
		public abstract void Remove(GameObject _target, GameObject _source);	

		/// <summary>
		/// Compares the <see cref="Name"/> of the <see cref="EffectSO"/>
		/// </summary>
		/// <param name="other"></param>
		/// <returns>Returns an <see cref="int"/> representing the comparison</returns>
		public virtual int CompareToName(EffectSO other)
		{
			return Name.CompareTo(other.Name);
		}
		/// <summary>
		/// Compares the <see cref="EffectType"/> of the <see cref="EffectSO"/>
		/// </summary>
		/// <param name="other"></param>
		/// <returns>Returns an <see cref="int"/> representing the comparison</returns>
		public virtual int CompareToType(EffectSO other)
		{
			return Type.CompareTo(other.Type);
		}
	}
}