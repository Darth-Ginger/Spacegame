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
using UnityEditor;

namespace Spacegame.Gameplay
{
	/// <summary>
	/// Base object that makes up all Cards.
	/// </summary>
	[CreateAssetMenu(menuName = "Spacegame/Cards/Card", fileName = "Card")]
	public class CardSO : SerializableScriptableObject 
	{
		

		[SerializeField]
		private string _cardName = "Card";
		
		[SerializeField][RichText, Multiline(5)]
		private string _cardDescription = "";
		
		[SerializeField][Toolbar][Title("Card Cost")]
		private CostEnum _cardCost = (CostEnum)0;
		
		[SerializeField][Toolbar][Title("Card Rank")]
		private RankEnum _cardRank = (RankEnum)1;

		[SerializeField]	 
		private TargetType _targetType = TargetType.All;

		[CustomInspector.Foldout]
		private EffectSO[] Effects;

		
		/// <summary>
		/// The name of the card.
		/// </summary>
		public string CardName { get => _cardName; set => _cardName = value; } 

		/// <summary>
		/// The description of the card.
		/// </summary>
		public string CardDescription { get => _cardDescription; set => _cardDescription = value; }

		/// <summary>
		/// The cost to play the card.
		/// </summary>
		public CostEnum Cost { get => _cardCost; set => _cardCost = value; }	

		/// <summary>
		/// The rank of the card.
		/// </summary>
		public RankEnum Rank { get => _cardRank; set => _cardRank = value; }

		/// <summary>
		/// The types of targets the card can target.
		/// </summary>
		public TargetType TargetType { get => _targetType; set => _targetType = value; }

		/// <summary>
		/// The effects of the card.
		/// </summary>
		public EffectSO[] CardEffects { get => Effects; set => Effects = value; }

		/// <summary>
		/// Constructor for a card descriptor.
		/// </summary>
		public CardSO(string cardName = "", string cardDescription = "A Card", int cardCost = 0, int cardRank = 1, List<EffectSO> effects = null)
		{
			if (cardName == "") cardName = name;
			_cardName = cardName;
			_cardDescription = cardDescription;
			_cardCost = (CostEnum)cardCost;
			_cardRank = (RankEnum)cardRank;
        }

		private void OnValidate()
		{
			EditorUtility.SetDirty(this);
			
		}

		/// <summary>
		/// Checks if the card has a specific effect.
		/// </summary>
		public bool HasEffect(EffectSO effect) => CardEffects.ToList().Contains(effect);
		
		/// <summary>
		/// Gets the types of effect in the card.
		/// </summary>
		public EffectType GetEffectTypes() => CardEffects.ToList().Select(_ => _.Type).ToList().Aggregate((a, b) => a | b);
	}
}