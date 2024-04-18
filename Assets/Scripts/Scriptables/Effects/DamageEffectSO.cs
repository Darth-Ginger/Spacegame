using NaughtyAttributes;
using UnityEngine;
using Utility;

namespace Spacegame.Gameplay
{
    [CreateAssetMenu(menuName = "Spacegame/Effects/DamageEffect", fileName = "NewDamageEffect")]
    public class DamageEffect : EffectSO
    {
        
        [MinValue(1)]
        public int damage;

        protected override void OnEnable()
        {
            base.OnEnable();
            damage = 1;
        }

        public override void Apply(GameObject target, GameObject source)
        {
            throw new System.NotImplementedException();
        }

        public override void Remove(GameObject _target, GameObject _source)
        {
            throw new System.NotImplementedException();
        }
    }
}