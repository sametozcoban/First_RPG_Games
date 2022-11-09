using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Combat
{
    public class PatrolWay : MonoBehaviour
    {
        const float wayPointGizmos = 0.5f;
        private void OnDrawGizmos() //Çizimlerin yapıldığı method
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);  
                Gizmos.DrawSphere(GetPoint(i), wayPointGizmos );
                Gizmos.DrawLine(GetPoint(i) , GetPoint(j));
            }
        }

        public int GetNextIndex(int i) /*Oluşturulan componentların sonuncusunu oluşturma ve birleştirmeden sonra
                                         harekette sonuncudan tekrar 0. indexe return yapılan method. */
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetPoint(int i) //Çizilecek olan gizmosların kimler olduğunun belirlenmesi.
        {
            return transform.GetChild(i).position;
        }
    }
}