using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Follow_Camera : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        // Update is called once per frame
        void LateUpdate() // Animasyon ve kameranın uyumsuzluğunu gidermek için LateUptade olarak çalıştırıyoruz. 
        {
            transform.position = _target.position;
        }
    }
}
