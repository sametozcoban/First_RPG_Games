using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour , ISaveable
    {
        [SerializeField] float health = 20f;
        

        bool isDead = false;
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die();
            }
            print(health);
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