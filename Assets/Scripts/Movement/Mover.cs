using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movemenent
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {

        
        [SerializeField] Transform target;
        [SerializeField] private float playerSpeed;
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField]  float maxPathLength = 40f;

        private NavMeshAgent _navMeshAgent;
        private Fighter _fighter;

        private Health _health;
    
        //private ActionScheduler _actionScheduler;
        private Ray _ray;
    
        // Start is called before the first frame update

        private void Awake()
        {
            _health = GetComponent<Health>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _fighter = GetComponent<Fighter>();
            //_actionScheduler = GetComponent<ActionScheduler>();
        }

        void Start()
        {
        
        }
    
        // Update is called once per frame
        void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();
            UpdateAnimation();
        }
    
        public void StartMoveAction(Vector3 hit, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(hit, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) >maxPathLength) return false;

            return true;
        }
            
        public void MoveTo(Vector3 hit, float speedFraction)
        {
            _navMeshAgent.destination = hit;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        }
    
        public void Cancel()
        {
            _navMeshAgent.stoppingDistance = 2f;
        }
    
        private void UpdateAnimation()
        {
            /* Global değerden yerel olan değere dönüştürüyoruz. Nav mesh üzerinden alınan değerler global(x,y,z) olduğundan dolayı animator için kullanışlı değil.
               Bunun önüne geçmek için InverseTransformDirection(Vector 3 ......) kullanarak yerel değere dönüştürerek animatorun anlayacabileceği değerlere dönüştürüyoruz.3-5 birim ileri gibi. */
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); // Convert Local value relative the character. Using InverseTransformDirection(velocity)
    
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        
        /* Bu noktada hesaplanan yol gidilebilir olması için belirlenen "GetPathLength(path) > maxPathLength" koşulunu sağlaması için path uzunluğunu bulmak için kullandığımız method.
      "path.corners" path üzerinde gidilecek olan noktaların Vector3 hali ile tutulmasını sağlıyor.
      "for" döngüsü kullanarak aralarında ki farkı bulup "total değişkenimize ekliyoruz.
      "return total" den gelen float değer "GetPathLength(path) > maxPathLength" koşulunu sağlıyor ise hareket edebiliyoruz sağlamıyor ise false değer dönderiyoruz. */
        public float GetPathLength(NavMeshPath path) 
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1 ; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }
        public void Cancel(float stopDistance)
        {
            _navMeshAgent.stoppingDistance =
                stopDistance; // Target ve player arasında ki mesafe belirtilen değer de ise player navmeshagent duracak.
        }
                
        public object CaptureState() //Sahneyi yakaladığımız save noktası
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) // Save alınan noktaya geri dönmek için kullanılan method.
        {
            SerializableVector3 position = (SerializableVector3)state;
            _navMeshAgent.enabled = false; // NavMesh konumu değiştirirken load yaparken bize karışmanı engellemek için yapılan işlem.
            transform.position = position.ToVector();
            _navMeshAgent.enabled = true;  
        }

    
    }
}
