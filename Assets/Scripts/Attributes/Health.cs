using RPG.Saving;
using System;
using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour , ISaveable
    {
        [SerializeField] float health = 20f;
        
        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
            
        }

        bool isDead = false;
        public Vector3 position;

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die();
                AwardExperince(instigator);

            }
            print(health);
        }

        private void AwardExperince(GameObject instigator) /* Enemy öldüğü durumda çalıştırılarak hasarı veren kişiye experience ekliyoruz. 
                                                                Şuan için sabit return 10 dönüyor daha sonra düşmana göre değişicek. */
        {
            float _stats = GetComponent<BaseStats>().GetExperience();
            Experience _experience = instigator.GetComponent<Experience>();
            if(_experience == null) return;
            
            _experience.GainExperince(_stats);
        }

        public float HealthPercentage()
        {
            return 100 * (health / GetComponent<BaseStats>().GetHealth());
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;

            if (health == 0) // Save noktasında Sağlık sıfır olan herkes ölü olduğundan bunu da yakalayıp o sahnede ölü olarak kalmasını sağlıyoruz.
            {
                Die();
            }
        }
    }
}