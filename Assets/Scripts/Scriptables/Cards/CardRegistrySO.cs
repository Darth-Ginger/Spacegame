using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utility;

namespace Spacegame.Gameplay
{
    [CreateAssetMenu(menuName = "Spacegame/Cards/CardRegistrySO", fileName = "CardRegistrySO")]
    public class CardRegistrySO : Registry<CardSO>
    {

        /// <summary>
        /// Returns a random <see cref="CardSO"/>
        /// </summary>
        /// <returns>Returns a random <see cref="CardSO"/></returns>
        public CardSO GetRandomCard()            => GetRandom();
        /// <summary>
        /// Returns 1 or more random <see cref="CardSO"/>
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Returns 1 or more random <see cref="CardSO"/></returns>
        public CardSO GetRandomCards(int amount) => GetRandom(amount);


        /// <summary>
        /// Sorts the registry by cost
        /// </summary>
        /// <param name="sortMethod"></param>
        public void SortByCost(SortMethod sortMethod) => Sort<CardSO>(sortMethod, x => x.Cost);
        /// <summary>
        /// Sorts the registry by rank
        /// </summary>
        /// <param name="sortMethod"></param>
        public void SortByRank(SortMethod sortMethod) => Sort<CardSO>(sortMethod, x => x.Rank);
        /// <summary>
        /// Sorts the registry by name
        /// </summary>
        /// <param name="sortMethod"></param>
        public void SortByName(SortMethod sortMethod) => Sort<CardSO>(sortMethod, x => x.CardName);
        /// <summary>
        /// Sorts the registry by effect types
        /// </summary>
        /// <param name="sortMethod"></param>
        public void SortByEffectsType(SortMethod sortMethod) => Sort<CardSO>(sortMethod,
                                                                             x => x.GetEffectTypes());
    }
}