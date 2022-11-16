using System;
using Combat;
using RPG.Attributes;
using RPG.Combat;
using RPG.Commbat;
using RPG.Core;
using RPG.Movemenent;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour , IAction // ISaveable
    {
        [SerializeField] private float timeBetweenAttacks = 1.31f;
        [SerializeField] private Transform righthandTransform = null;
        [SerializeField] private Transform lefthandTransform = null;
        [SerializeField]  Weapon defaultWeapon = null;

        Transform target;
        private Health _health;
        Weapon currentWeapon = null;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            _health = GetComponent<Health>();
            if (currentWeapon == null)
            { 
                EquippedWeapon(defaultWeapon);
            }
            
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
           
            if(target == null) return;

            _health = target.GetComponent<Health>();

            if (_health.IsDead()) return;
            
            if (target != null)
            {
                if (!GetIsInRange())
                {
                    GetComponent<Mover>().MoveTo(target.position ,1f);
                }
                else 
                {
                    GetComponent<Mover>().Cancel(currentWeapon.GetRange());
                    AttackBehaviour();
                }
            }
            
        }

        public void AttackBehaviour() //Attack animasyonu etkilenşirmek için kullandık.
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // Buradan Hit() methodu çalıştırılacak.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
            
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void Hit() /* Yumruk indiği zaman damage vurarak sağlığı azaltacak.
                    Kullanılan silah Yay ise bunun kontrolünüde burada yapıyoruz sadece " void Shoot() " olarak farklı method içerisinde çağırıyoruz. */
        {
            if(target == null) {return;}
            _health = target.GetComponent<Health>();
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile()) // Kullanılan if koşulu, silahımızın mermili mi yoksa mermisiz mi olup olmadığı kontrolünü sağlıyor.
            {
                currentWeapon.LaunchProjectile(righthandTransform,lefthandTransform , _health , gameObject, damage);
            }
            else
            {
                _health.TakeDamage(gameObject, damage);
            }
            
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange() /* Karakter ile target arasında ki poziyonun uzaklığını true veya false olarak return ettirip
                                     Combat veya Movement methodlarını ona göre çağırdık. */
        {
            return Vector3.Distance(transform.position, target.position) < currentWeapon.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.transform;
            GetComponent<ActionScheduler>().StartAction(this);
        }
        
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }

        public bool CanAttack(GameObject combatTarget)
       {
           if (combatTarget == null)
           {
               return false;
           }

           Health targetToTest = combatTarget.GetComponent<Health>();
           return targetToTest != null && !targetToTest.IsDead();
       }

        public void EquippedWeapon(Weapon weapon) /* Karakterimiz doğduğunda eline sword instantiate ediyoruz,
                                                     Vurmak istediğinde animtor override controller ile sword animasyonuna geçiyoruz. */
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(righthandTransform , lefthandTransform , animator); // Karakter üzerinde bulunan yumruk animasyonunun sword animasyonuna geçtiği kod satırı.
        }

        //public Transform GetEnemyHealth() Transform target değiştirilecek. EnemyHealthDisplay kullanılarak hangi düşman seçilmişsse onun canından düşülecek.
        //                                  Bunun için Transform target değiştirilecek.
        //{  
        //    return target;
        //}

      //public object CaptureState()
      //{
      //    return currentWeapon.name;
      //}

      //public void RestoreState(object state)
      //{
      //    string weaponName = (string) state;
      //    Weapon weapon = Resources.Load<Weapon>(weaponName); 
      //    EquippedWeapon(weapon);
      //}
    }
}