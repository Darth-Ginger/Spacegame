#pragma warning disable CS0169 // Field is never used. This file is to see the fields in the inspector only. 
#pragma warning disable CS0414 // The private field is assigned but its value is never used
// ReSharper disable UnusedMember.Local

using UnityEngine;

using Gamelogic.Extensions.Internal;
using System;
using JetBrains.Annotations;

namespace Gamelogic.Extensions.Examples
{
	[Flags]
	public enum MonsterState
	{
		IsHungry = 1,
		IsThirsty = 2,
		IsAngry = 4,
		IsTired = 8
	}

	[Serializable]
	public class MonsterData
	{
		public string name;
		public string nickName;
		public Color color;
	}

	//This class would work exactly the same in the inspector
	//if it was extended from MonoBehaviour instead, except
	//for the InspectorButton
	public class PropertyDrawerExample : GLMonoBehaviour
	{
		[ReadOnly]
		public string readonlyString = "Cannot change in inspector";

		[Space]
		[Comment("Cannot be negative")]
		[NonNegative]
		[SerializeField] private int nonNegativeInt = 0;

		[NonNegative]
		[SerializeField] private float nonNegativeFloat = 0f;

		[Highlight]
		[SerializeField] private int highligtedInt;

		[Space]
		[Comment("Can only be positive")]
		[Positive]
		[SerializeField] private int positiveInt = 1;

		[Positive]
		[SerializeField] private float positiveFloat = 0.1f;

		[Header("Other Fields")]
		[LabelField("nickName")] //Note the nickName is used for the labels
		[SerializeField] private MonsterData[] moreMonsters = 
		{
			new MonsterData{ name = "Vampire", nickName = "Vamp", color = Utils.Blue },
			new MonsterData{ name = "Werewolf", nickName = "Wolf", color = Utils.Red},
		};

		[InspectorFlags]
		[SerializeField] private MonsterState monsterState = MonsterState.IsAngry | MonsterState.IsHungry;

		[SerializeField] private MinMaxFloat range = new MinMaxFloat(0.25f, 0.75f);

		[SerializeField] private OptionalInt anOptionalInt = new OptionalInt
		{
			UseValue = true,
			Value = 3
		};

		[Space]
		[Header("Inspector Buttons")]
		[Dummy, SerializeField, UsedImplicitly]
		private bool dummy; //This hack may be questionable.

		//This will only show as a button if you extend from GLMonoBehaviour.
		[InspectorButton]
		public static void LogHello()
		{
			Debug.Log("Hello");
		}
	}
}
