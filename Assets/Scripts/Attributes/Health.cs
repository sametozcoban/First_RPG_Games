using RPG.Saving;
using System;
using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour , ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] float health = -1f;
        
        bool isDead = false;
        public Vector3 position;
        
        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);   
            }
        }
        
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

        public float GetHealthPoints()
        {
            return health;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperince(GameObject instigator) /* Enemy öldüğü durumda çalıştırılarak hasarı veren kişiye experience ekliyoruz. 
                                                                Stat.ExperienceReward öldürülen her düşman için eklenecek olan deneyim puanı. */
        {
            float _stats = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
            Experience _experience = instigator.GetComponent<Experience>();
            if(_experience == null) return;
            
            _experience.GainExperince(_stats);
        }
        private void RegenerateHealth() //Level atladığımızda canın yeninlemesi için kullanılacak olan method.
        {
            float regenHealthPoint = GetComponent<BaseStats>().GetStat(Stat.Health) *(regenerationPercentage / 100);
            health = Mathf.Max(health, regenHealthPoint);
        }

        public float HealthPercentage()
        {
            return 100 * (health / GetComponent<BaseStats>().GetStat(Stat.Health));
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