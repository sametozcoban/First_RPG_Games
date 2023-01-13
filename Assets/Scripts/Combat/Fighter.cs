using System;
using System.Collections.Generic;
using Combat;
using GameDevTV.Utils;
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
    public class Fighter : MonoBehaviour , IAction , IModifierProvider// ISaveable
    {
        [SerializeField] private float timeBetweenAttacks = 1.31f;
        [SerializeField] private Transform righthandTransform = null;
        [SerializeField] private Transform lefthandTransform = null;
        [FormerlySerializedAs("defaultWeapon")] [SerializeField]  WeaponConfig defaultWeaponConfig = null;

        Transform target;
        private Health _health;
        WeaponConfig currentWeaponConfig ;
        LazyValue<Weapon> currentWeapon;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            
            return AttachWeapon(defaultWeaponConfig);
        }

        private void Start()
        {
            _health = GetComponent<Health>();
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
           
            if(target == null) return;

            _health = target.GetComponent<Health>();

            if (_health.IsDead()) return;
            
            if (target != null)
            {
                if (!GetIsInRange(target.transform))
                {
                    GetComponent<Mover>().MoveTo(target.transform.position ,1f);
                }
                else 
                {
                    GetComponent<Mover>().Cancel(currentWeaponConfig.GetRange());
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

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            if (currentWeaponConfig.HasProjectile()) // Kullanılan if koşulu, silahımızın mermili mi yoksa mermisiz mi olup olmadığı kontrolünü sağlıyor.
            {
                currentWeaponConfig.LaunchProjectile(righthandTransform,lefthandTransform , _health , gameObject, damage);
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

        private bool GetIsInRange(Transform targetTransform) /* Karakter ile target arasında ki poziyonun uzaklığını true veya false olarak return ettirip
                                     Combat veya Movement methodlarını ona göre çağırdık. */
        {
            return Vector3.Distance(transform.position, target.position) < currentWeaponConfig.GetRange();
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
        
        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentage();
            }
        }

        public bool CanAttack(GameObject combatTarget)
       {
           if (combatTarget == null) { return false; }
           if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform)) { return false; }
           Health targetToTest = combatTarget.GetComponent<Health>();
           return targetToTest != null && !targetToTest.IsDead();
       }

        public void EquippedWeapon(WeaponConfig weapon) /* Karakterimiz doğduğunda eline sword instantiate ediyoruz,
                                                     Vurmak istediğinde animtor override controller ile sword animasyonuna geçiyoruz. */
        {
            currentWeaponConfig = weapon;
            currentWeapon.value= AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(righthandTransform , lefthandTransform , animator); // Karakter üzerinde bulunan yumruk animasyonunun sword animasyonuna geçtiği kod satırı.
        }

        public Transform GetEnemyHealth() /*Transform target değiştirilecek. EnemyHealthDisplay kullanılarak hangi düşman seçilmişsse onun canından düşülecek.
                                            Bunun için Transform target değiştirilecek.*/
        {  
            return target;
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
                string weaponName = (string) state;
                WeaponConfig weaponConfig = Resources.Load<WeaponConfig>(weaponName); 
                EquippedWeapon(weaponConfig); 
        }
    }
}