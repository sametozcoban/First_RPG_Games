using System;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private void Update()
        {
            transform.forward = Camera.main.transform.forward; //Hazırlanan Canvas içerisinde ki TEXT in  daima kameraya doğru bakmasını sağlayacak.
        }
    }
}