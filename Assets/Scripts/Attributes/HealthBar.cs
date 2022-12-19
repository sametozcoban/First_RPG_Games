using System;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health _healthComponent = null ;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas _canvas = null;
        
        
        void Update()
        {
            if (HealthBarControl()) return;
            foreground.localScale = new Vector3(_healthComponent.GetFraction(),1 ,1 );
        }

        private bool HealthBarControl()
        {
            if (Mathf.Approximately(_healthComponent.GetFraction(), 1) || Mathf.Approximately(_healthComponent.GetFraction() , 0))
            {
                _canvas.enabled = false;
                return true;
            }
            _canvas.enabled = true;
            return false;
        }
    }
}