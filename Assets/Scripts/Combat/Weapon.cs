using RPG.Core;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons / Make New Weapons", order = 0)]
    public class Weapon : ScriptableObject
    {
        /* --- Scriptable object oluşturduk. Elimizde equippedPrefab için sword aldığımız durum ve animasyon için ise animatorOverrideController kullandık.
           --- Scriptable object oluşturmamızın sebebi ise birden fazla objeyi elimize aldığımızda hangisinin geleceği ve nereye nasıl geleceğini belirtmek için kullanıyoruz.
           --- Player içerisin de  scriptable object entegre edilebilecek Fighter scriptine açtığımız methodunu kullandık. */
        
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] float  attackDamage = 5f;
        [SerializeField] bool isRightHand = true;
        [SerializeField] private Projectile _projectile = null;


        private const string weaponName = "Weapon";
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) /* Player eline göre transform alacacak. Animasyonuda aktif hale getirecek.
                                                                                         OverrideController animasyonunun çalışması durumunun kontrolü. */
        {
            DestroyWeapon(rightHand, leftHand);
            if (equippedPrefab != null)
            {
                var handTransform = GetTransform(rightHand, leftHand);
                 GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }

            if (animatorOverride != null)
            { 
                animator.runtimeAnimatorController = animatorOverride;
            }
            
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target) //Kullanılacak merminin örneklenmesini sağlayan method.
        {
            Projectile projectileInstance = Instantiate(_projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target , attackDamage);
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetDamage()
        {
            return attackDamage;
        }
    }
}