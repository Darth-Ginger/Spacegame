using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Utility;
using static Spacegame.Gameplay.CardSO;


namespace Spacegame.Gameplay
{
    public class Card : MonoBehaviour
    {
        private CardSO _cardSO;

        public CardSO cardSO => _cardSO;
        public string CardName { get; private set; }
        public string CardDescription { get; private set; }
        public int CardCost { get; private set; }
        public RankEnum CardRank { get; private set; }
        public TargetType TargetType { get; private set; }

        public List<EffectSO> Effects { get; private set; }

        public Card (CardSO cardSO) {

        }
    }
}