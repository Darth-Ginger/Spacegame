using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Utility;
using Guid = Utility.Guid;
using CustomInspector;
using UnityEditor;




namespace Spacegame.Gameplay
{
    /// <summary>
    /// Base object that makes up all Cards
    /// </summary>
    public class CardClass : MonoBehaviour
    {
        [Space(20, order = 1)]
        [Button(nameof(OnValidate), label = "Refresh", order = 1, size = Size.big)]
        [HideField]
        [SerializeField]private bool _spacer;
        [HorizontalLine]
        

        [SerializeField][Foldout]   private CardSO       _cardSO;
        private Guid       _cardGuid;
        private string     _cardName;
        private string     _cardDescription;
        private int        _cardCost;
        private RankEnum   _cardRank;
        private TargetType _targetType;
        private EffectSO[] _effects;

        public CardSO CardSO            { get => _cardSO;   } // Readonly - Set in constructor/inspector
        public Guid   CardGuid          { get => _cardGuid; } // Readonly - Set by CardSO
        public string CardName          { get => _cardName;        private set => _cardName = value; }
        public string CardDescription   { get => _cardDescription; private set => _cardDescription = value; }
        public int CardCost             { get => _cardCost;        private set => _cardCost = value; }
        public RankEnum CardRank        { get => _cardRank; }
        public TargetType TargetType    { get => _targetType; }
        public HashSet<string> CardEffects { get => _effects.Select(_ => _.Name).ToHashSet(); 
                                             private set => SetCardEffectHashSet();}

        public List<EffectSO> Effects   
            { 
            get => _effects.ToList(); 
            private set { 
                if (value != null) {
                if(value.GetType() == typeof(EffectSO[]))
                    AddEffects(value.ToArray()); 
                else if (value.GetType() == typeof(List<EffectSO>))
                    AddEffects(value);
                } 
            }
            }

        public void OnValidate()
        {
            if (_cardSO == null) return;
            _cardGuid        = _cardSO.Guid;
            _cardName        = _cardSO.CardName;
            _cardDescription = _cardSO.CardDescription;
            _cardCost        = (int)_cardSO.Cost;
            _effects         = _cardSO.CardEffects;
            SetTargetType(_cardSO.TargetType);
            SetRank(_cardSO.Rank);
        }
        public string GetCardEffects () => string.Join(", ", _effects.Select(_ => _.Name));

        public void SetCardEffectHashSet () => CardEffects = _effects.Select(_ => _.Name).ToHashSet();

        public void SetRank (RankEnum rank) => _cardRank = rank;
        public void SetRank (string rank) => _cardRank = (RankEnum)Enum.Parse(typeof(RankEnum), rank);
        public void SetRank (int rank) => _cardRank = (RankEnum)rank;

        public void SetTargetType (TargetType targetType) => _targetType = targetType;
        public void SetTargetType (string targetType) => _targetType = (TargetType)Enum.Parse(typeof(TargetType), targetType);
        public void SetTargetType (int targetType) => _targetType = (TargetType)targetType;

        public  void AddEffect  (EffectSO effectSO)      => _effects = _effects.Append(effectSO).ToArray();
        private void AddEffects (EffectSO[] effects)     => _effects = _effects.Concat(effects).ToArray();
        private void AddEffects (List<EffectSO> effects) => _effects = _effects.Concat(effects).ToArray();
        /// <summary>
        /// Removees the specific effect as determined by Guid
        /// </summary>
        /// <param name="effectSO"></param>
        /// <returns>Returns true if the effect was removed</returns>
        public bool RemoveEffect (EffectSO effectSO)
        {   
            var removed = false;
            foreach (var effect in _effects)
            {
                removed = RemoveEffect(effectSO.Guid);
            }
            return removed;
        }
        /// <summary>
        /// Removes the first effect with the given Guid
        /// </summary>
        /// <param name="effectGuid"></param>
        /// <returns>Returns true if the effect was removed</returns>
        public bool RemoveEffect (Guid effectGuid)
        {
            var effectToRemove = _effects.FirstOrDefault(effect => effect.Guid == effectGuid);

            if (effectToRemove != null)
            {
                _effects = _effects.Where(effect => effect != effectToRemove).ToArray();
                return true;
            }

            return false;
        }
        /// <summary>
        /// Removes all effects with the given name
        /// </summary>
        /// <param name="effectName"></param>
        /// <returns>True if at least one effect was removed</returns>
        public bool RemoveEffect (string effectName)
        {
            var effectToRemove = _effects.FirstOrDefault(effect => effect.Name == effectName);
            if (effectToRemove != null)
            {
                _effects = _effects.Where(effect => effect.CompareToName(effectToRemove) != 0).ToArray();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes all effects with the given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Returns true if at least one effect was removed</returns>
        private bool RemoveEffect(EffectType type)
        {
            var effectToRemove = _effects.FirstOrDefault(effect => effect.Type == type);
            if (effectToRemove != null)
            {
                _effects = _effects.Where(effect => effect.CompareToType(effectToRemove) != 0).ToArray();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes all effects with the given type
        /// </summary>
        /// <param name="effectSO"></param>
        /// <param name="by"></param>
        /// <returns>Returns true if at least one effect was removed</returns>
        public bool RemoveEffects (EffectSO effectSO, string by = "name")
        {
            var removed = false;
            by = by.ToLower();
            foreach (var effect in _effects)
            {
                return by switch
                {
                    "name" => removed = RemoveEffect(effectSO.Name),
                    "guid" => removed = RemoveEffect(effectSO.Guid),
                    "type" => removed = RemoveEffect(effectSO.Type),
                    _ => removed = RemoveEffect(effectSO),
                };
                
            }
            return removed;
        }

    }


    [CustomEditor(typeof(CardClass))]
    public class CardClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CardClass card = (CardClass)target;

            // Create a GUIStyle with rich text formatting
            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label);
            richTextStyle.richText = true;

            // Custom inspector GUI elements for CardClass
            EditorGUILayout.BeginVertical("Box");

            GUILayout.Label("Custom Card Properties", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            /**/EditorGUILayout.LabelField($"Card Name: ", GUILayout.Width(100));
            /**/EditorGUILayout.LabelField($"{card.CardName}");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            /**/EditorGUILayout.LabelField($"Card Description: ", GUILayout.Width(100));
            /**/EditorGUILayout.LabelField($"{card.CardDescription}", richTextStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            /**/EditorGUILayout.LabelField($"Card Cost: ", GUILayout.Width(100));
            /**/EditorGUILayout.LabelField($"{card.CardCost}");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            /**/EditorGUILayout.LabelField($"Card Rank: ", GUILayout.Width(100));
            /**/EditorGUILayout.LabelField($"{card.CardRank}");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            /**/EditorGUILayout.LabelField($"Card Target: ", GUILayout.Width(100));
            /**/EditorGUILayout.LabelField($"{card.TargetType}");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (card.GetCardEffects().Length > 0) {
            /**/EditorGUILayout.LabelField($"Card Effects: ", GUILayout.Width(100));
            /**/EditorGUILayout.LabelField($"{card.GetCardEffects()}");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            if (card.Effects.Count > 0)
            {
                foreach (var effect in card.Effects) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Card Effect: ", GUILayout.Width(100));
                EditorGUILayout.LabelField($"{effect.Type}     {effect.Name}");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                }
            }
            EditorGUILayout.EndVertical();


            DrawDefaultInspector();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }


}