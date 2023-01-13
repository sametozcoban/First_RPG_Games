using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movemenent;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float AggrevatedTime = 5f;
        [SerializeField] private PatrolWay _patrolWay;
        [SerializeField] private float wayPointTolerance = 5f;
        [SerializeField] private float waitPointTime = 2f;
        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;
        [SerializeField] private float shoutDistance = 5f;
        
        private Fighter _fighter;
        private Health _health;
        private GameObject player;
        private Mover _mover;
        
        LazyValue<Vector3> guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceLastWaintPoint = Mathf.Infinity;
        private float timeSinceAggrevatedTime = Mathf.Infinity;
        private int currentWayPointIndex = 0;
        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        // Update is called once per frame
        void Update()
        {
            if(_health.IsDead()) return;
            if (IsAggrevated() && _fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
                
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour(); //Enemy tekrar kendi poziyonuna dönecek.
                //_fighter.Cancel();
            }

            UptadeTimers();
        }

        public void Aggrevated()
        {
            timeSinceAggrevatedTime = 0;
        }

        private void UptadeTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaintPoint += Time.deltaTime;
            timeSinceAggrevatedTime += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (_patrolWay != null)
            {
                if (AtWaypoint())
                {
                    timeSinceLastWaintPoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            
            if(timeSinceLastWaintPoint > waitPointTime)
            { 
                _mover.StartMoveAction(nextPosition , patrolSpeedFraction); //Yeni noktaya gitmemizi sağlar
            }
        }

        private bool AtWaypoint() // Herhangi bir noktada isek bu durumu gerçekleştirip index pointinin oraya gidiyoruz.
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < wayPointTolerance;
        }
        private void CycleWayPoint() //Bir sonra ki gideceği noktanın neresi olduğunu bilecek.
        {
            currentWayPointIndex = _patrolWay.GetNextIndex(currentWayPointIndex);
        }
        
        private Vector3 GetCurrentWayPoint() // Başlangıç noktasını index olarak 0 belirledik bu şekilde 1. noktaya geldiğinde 2. noktaya gideceğini bilecek.
        {
            return _patrolWay.GetPoint(currentWayPointIndex);
        }

       
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            _fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                
                if(ai == null) continue;

                ai.Aggrevated();
            }
        }

        /* ---Player ve target arasında ki mesefanin farkı küçük olduğu duruma göre true veya false dönecek.
           ---Uptade methodunda kullandığımız durum devreye girecek.
           ---Player saldırılabilirse figth.Attack() methodu devreye girerek saldırı yapabilecek. */
        public bool IsAggrevated() 
        {
            float distance =  Vector3.Distance(player.transform.position, transform.position);
            return distance < chaseDistance || timeSinceAggrevatedTime < AggrevatedTime;
        }

        public void OnDrawGizmosSelected() //Seçili olan gameObject üzerine çizilecek olan gizmos methodu. 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position , chaseDistance);
        }
    }
}