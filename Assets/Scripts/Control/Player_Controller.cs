using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Commbat;
using UnityEngine;
using RPG.Movemenent;
using RPG.Combat;
using RPG.Core;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace RPG.Control
{
    public class Player_Controller : MonoBehaviour
    {
        private Health _health;
        private Mover _mover;
        private Ray _ray;
        private Fighter _fighter;

        
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]  CursorMapping[] cursorMapping = null;
        [SerializeField]  float maxDistance = 1f;
        [SerializeField]  float sphereCastRadius = 1f;

        private void Start()
        {
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }  
        void Update()
        {
            if (InteractWithComponent()) return;
            if(InteractWithUI()) return;
            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
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
        
        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RayCastAllSorted() ;

            foreach (RaycastHit hit in hits)
            {
                IRaycastable [] raycastables= hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.handleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType()); // Raycast yapılıyor, raycastın yapıldığı objeye göre  CursorType dönerek hangi enum için geçerli ise o cursorun şeklini alıyor.
                        return true;
                    }
                }
            }

            return false;
        }

        RaycastHit[] RayCastAllSorted() /*Uzaklıklara göre cursor değişimini kontrol ettiğimiz kısım.
                                          Arka arkaya bulunan objelerden ilk raycast atılan durumda ikinci objeye raycast atıldığında aynı cursor çıkmamaası durumunun kontorlünün yapıldığı durum. */
        {
            RaycastHit [] hits = Physics.SphereCastAll(ScreenPointToRay(), sphereCastRadius );
            float[] distances = new float[hits.Length]; //hits arrayı uzunluğunda bir array oluşturuyoruz.

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance; // Eleman sayılarını birbirine eşitlerik. 1-1, 2-2 gibi.
            }
            Array.Sort(distances, hits); // Hitslerin sıralamasını yaptık.

            return hits;
        }

      
       
        /* "IntMovement()" World dışında bir noktaya tıklandığı zaman hareket edilemeyeceğinin uyarısını veren bir method.
            Aynı zaman da yürünülebilir kısımlarıda kontrol edip yürümeyi sağlayan method. */
        public bool IntMovement()
        {
            //_ray = ScreenPointToRay(); // Ray direction == _ray(Mouse position.)
           //RaycastHit hit;
           //bool hasHit = Physics.Raycast(_ray, out hit);
            Vector3 target;
            bool hasHit = RayCastNavMesh(out target);
            if (!_mover.CanMoveTo(target)) return false;
            if (hasHit) 
            {
                if ((Input.GetMouseButton(0))) 
                {
                    _mover.StartMoveAction(target , 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        bool RayCastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            _ray = ScreenPointToRay(); // Ray direction == _ray(Mouse position.)
            RaycastHit hit;
            bool hasHit = Physics.Raycast(_ray, out hit);
            if (!hasHit) return false;
            
            NavMeshHit navMeshHit;
            bool navMeshControl = NavMesh.SamplePosition(hit.point, out navMeshHit, maxDistance, NavMesh.AllAreas);
            if (!navMeshControl) return false;
            
            target = navMeshHit.position;
            
            return true;
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
