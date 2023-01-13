using RPG.Saving;
using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;
using RPG.UI.DamageText;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour , ISaveable
    {
       
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
           
        }

       LazyValue<float> health  ;

        bool isDead = false;
        public Vector3 position;

        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }
        
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        
        private void Start()
        {
            health.ForceUnit();
        }
        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }
        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        //private void Start()
        //{
        //    GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        //    if (health < 0)
        //    {
        //        health = GetComponent<BaseStats>().GetStat(Stat.Health);   
        //        Debug.Log("Health is : " + health);
        //    }
        //}
        
        public void TakeDamage(GameObject instigator, float damage)
        {
            health.value = Mathf.Max(health.value - damage, 0);
            
            if (health.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperince(instigator);
            }
            else
            { 
                takeDamage.Invoke(damage);
            }
            print(health);
        }

        public void Heal(float healthToRestore)
        {
            health.value = Mathf.Min(health.value + healthToRestore , GetMaxHealthPoints());
            Debug.Log("Benim canım" + health);

        }

        public float GetHealthPoints()
        {
            return health.value;
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
            health.value = Mathf.Max(health.value, regenHealthPoint);
        }

        public float HealthPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return (health.value / GetComponent<BaseStats>().GetStat(Stat.Health));
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
            health.value = (float)state;

            if (health.value == 0) // Save noktasında Sağlık sıfır olan herkes ölü olduğundan bunu da yakalayıp o sahnede ölü olarak kalmasını sağlıyoruz.
            {
                Die();
            }
        }
    }
}