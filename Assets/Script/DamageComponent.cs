using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _damage;

        public void AplyDamage(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyDamage(_damage,0);
            }
        }
    }
}