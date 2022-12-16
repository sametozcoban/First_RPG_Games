using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Commbat;
using UnityEngine;
using RPG.Movemenent;
using RPG.Combat;
using RPG.Core;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class Player_Controller : MonoBehaviour
    {
        private Health _health;
        private Mover _mover;
        private Ray _ray;
        private Fighter _fighter;

        enum CursorType
        {
            None,
            Movement,
            Combat,
            UI
        }
        
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]  CursorMapping[] cursorMapping = null;
        

        private void Start()
        {
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }  
        void Update()
        {
            if(InteractWithUI()) return;
            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if(IntCombat()) return;
            if(IntMovement()) return;
            
            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject()) // UI parçasına tıklayıp tıklamadığımızı kontrol ettiğimiz bir durum.
            {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        /* "IntCombat()" Combat özelinde target üzerinde gezinirken saldırılabilir olup olmadığının gösterilmeisini sağlayacak bir method.
            Yürünürken imleç saldırabilir olurken farklı bir imleç kullanılmasını sağlaycak. */
        public bool IntCombat() 
        {
            RaycastHit[] hits = Physics.RaycastAll(ScreenPointToRay());
            
            foreach (RaycastHit hit in hits)
            {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if(target == null) continue;
                    GameObject targetGameObject = target.gameObject;
                    if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                    {
                        continue;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("Attack Player");
                        GetComponent<Fighter>().Attack(target.gameObject);
                    }

                    SetCursor(CursorType.Combat);
                    return true;
            }
            return false;
        }
        
        /* "IntMovement()" World dışında bir noktaya tıklandığı zaman hareket edilemeyeceğinin uyarısını veren bir method.
            Aynı zaman da yürünülebilir kısımlarıda kontrol edip yürümeyi sağlayan method. */
        public bool IntMovement()
        {
            _ray = ScreenPointToRay(); // Ray direction == _ray(Mouse position.)
            RaycastHit hit;
            bool hasHit = Physics.Raycast(_ray, out hit);

            if (hasHit) 
            {
                if ((Input.GetMouseButton(0))) 
                {
                    _mover.StartMoveAction(hit.point , 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }
        
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMapping)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMapping[0];
        }

        private static Ray ScreenPointToRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
