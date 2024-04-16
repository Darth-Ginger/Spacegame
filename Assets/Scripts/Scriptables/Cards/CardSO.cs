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

namespace Spacegame.Gameplay
{
	[CreateAssetMenu(menuName = "Spacegame/Cards/Card", fileName = "Card")]
	public class CardSO : ScriptableObject
	{
		#region Enums
		public enum CostEnum 
		{ 
			O    = 0,
			I    = 1,
			II   = 2,
			III  = 3,
			IV   = 4,
			V    = 5,
			VI   = 6,
			VII  = 7,
			VIII = 8,
			IX   = 9,
			X    = 10
		}
		public enum RankEnum
		{
			Unranked = 0,
			One      = 1,
			Two      = 2,
			Three    = 3,
		}
		#endregion Enums
		
		#region Variables
		[SerializeField]
		private string 	 			_cardName        = "Card";
		[SerializeField][RichText, Multiline(5)]
		private string 	 			_cardDescription = "";
		[SerializeField][Toolbar][Title("Card Cost")]
		private CostEnum 	 		_cardCost    = (CostEnum)0;
		[SerializeField][Toolbar][Title("Card Rank")]
		private RankEnum 	 		_cardRank    = (RankEnum)1;
		[SerializeField]	 
		private TargetType  		_targetType 	 = TargetType.All;
		#endregion Variables

		#region Properties
		// The name of the card
		public string CardName { get => _cardName; set => _cardName = value; } 
		// The description of the card
		public string CardDescription { get => _cardDescription; set => _cardDescription = value; }
		// The cost to play the card
		public CostEnum 	  Cost { get => _cardCost; set => _cardCost = value; }	
		// The rank of the card
		public RankEnum 	  Rank { get => _cardRank; set => _cardRank = value; }
		// The types of targets the card can target
		public TargetType TargetType { get => _targetType; set => _targetType = value; }

		[CustomInspector.Foldout]
		public List<EffectSO> Effects = new();
		#endregion Properties

		public CardSO(string cardName = "", string cardDescription = "A Card", int cardCost = 0, int cardRank = 1, List<EffectSO> effects = null)
		{
			if (cardName == "") cardName = name;
			_cardName = cardName;
			_cardDescription = cardDescription;
			_cardCost = (CostEnum)cardCost;
			_cardRank = (RankEnum)cardRank;

        }

		 
	}
}