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
	// [CreateAssetMenu(menuName = "Spacegame/Units/ComponentSO", fileName = "ComponentSO")]
	public abstract class ComponentSO : SerializableScriptableObject 
	{
		public abstract string Name { get; }
        


	}
}