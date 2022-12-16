using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Commbat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool handleRaycast(Player_Controller callingController)   /* "IntCombat()" Combat özelinde target üzerinde gezinirken saldırılabilir olup olmadığının gösterilmeisini sağlayacak bir method.
                                                                            Yürünürken imleç saldırabilir olurken farklı bir imleç kullanılmasını sağlaycak. */
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }
            if (Input.GetMouseButton(0))
            {
                Debug.Log("Attack Player");
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
        
        public CursorType GetCursorType() //Combat durumu için cursor değişimini yaptık.
        {
            return CursorType.Combat;
        }

    }
}