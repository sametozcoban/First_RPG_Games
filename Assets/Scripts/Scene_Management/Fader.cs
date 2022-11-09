using System;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        

        public  IEnumerator FadeOut(float time)
        {
            while (_canvasGroup.alpha < 1) // alpha is not 1
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0) // alpha is not 0
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                
                yield return null;
            }
        }
    }
}