using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons / Make New Weapons", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        /* --- Scriptable object oluşturduk. Elimizde equippedPrefab için sword aldığımız durum ve animasyon için ise animatorOverrideController kullandık.
           --- Scriptable object oluşturmamızın sebebi ise birden fazla objeyi elimize aldığımızda hangisinin geleceği ve nereye nasıl geleceğini belirtmek için kullanıyoruz.
           --- Player içerisin de  scriptable object entegre edilebilecek Fighter scriptine açtığımız methodunu kullandık. */
        
        [SerializeField] private Weapon equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float percentageBonus = 0f;
        [SerializeField] float  attackDamage = 5f;
        [SerializeField] bool isRightHand = true;
        [SerializeField] private Projectile _projectile = null;


        private const string weaponName = "Weapon";
        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator) /* Player eline göre transform alacacak. Animasyonuda aktif hale getirecek.
                                                                                         OverrideController animasyonunun çalışması durumunun kontrolü. */
        {
            DestroyWeapon(rightHand, leftHand);
            Weapon weapon = null;
            
            if (equippedPrefab != null)
            {
                var handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            if (animatorOverride != null)
            { 
                animator.runtimeAnimatorController = animatorOverride;
            }
            return weapon;
        }

        private void DestroyWeapon(Transform righthand, Transform lefthand) //Pickup yaptıktan sonra bir önceki silahın elden gitmesini sağladığımız method.
        {
            Transform oldWeapon = righthand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = lefthand.Find(weaponName);
            }
            
            if(oldWeapon == null) return;

            oldWeapon.name = "Destroying"; 
            Destroy(oldWeapon.gameObject);
            /* Klavye özelinde bir tuşa basarak silahlar arası geçiş sağlanabilir. */
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHand)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile() //Kullanılan savaş aleti Yay veya benzeri bir savaş aleti mi ? Kontrolünü sağlıyoruz.
        {
            return _projectile != null; 
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target , GameObject instigator , float calculatedDamage) //Kullanılacak merminin örneklenmesini sağlayan method.
        {
            Projectile projectileInstance = Instantiate(_projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, calculatedDamage , instigator);
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetPercentage()
        {
            return percentageBonus;
        }

        public float GetDamage()
        {
            return attackDamage;
        }
    }
}