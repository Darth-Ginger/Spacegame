using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gamelogic.Extensions.Examples
{
	/*	This example shows how to use an event tracker.
		The interesting code is in PoisonablePlayer.
	*/
	public class EventTrackerExampleMain : GLMonoBehaviour
	{
		#region Serialized Fields
		[FormerlySerializedAs("animal"), FormerlySerializedAs("player")]
		[Header("Player")]
		[SerializeField] private PoisonableCharacter character = null;

		[Header("Button Lists")]
		[SerializeField] private Color poisonColor = Color.green;
		[SerializeField] private ButtonListUI poisonList = null;

		[Space]
		[SerializeField] private Color antidoteColor = Color.magenta;
		[SerializeField] private ButtonListUI antidotesList = null;
		#endregion

		#region Messages
		public void Start()
		{
			var poisons = 
				Enum.GetValues(typeof(Poison))
					.Cast<Poison>()
					.ToList();

			poisonList.Init(poisons, poison => character.EatPoison(poison), poisonColor);
			antidotesList.Init(poisons, poison => character.EatAntidote(poison), antidoteColor);
		}
		#endregion
	}
}
