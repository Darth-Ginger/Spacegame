using NaughtyAttributes;
using UnityEngine;
using Utility;

namespace Spacegame.Gameplay
{
    [CreateAssetMenu(menuName = "Spacegame/Effects/DamageEffect", fileName = "NewDamageEffect")]
    public class DamageEffect : EffectSO
    {
        
        [MinValue(1)]
        public int damage = 1;

        public override void Apply()
        {
            // target.GetComponent<Health>().TakeDamage(damage);
        }
    }
}