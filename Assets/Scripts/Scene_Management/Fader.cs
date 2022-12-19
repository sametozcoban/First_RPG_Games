using System;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
         CanvasGroup _canvasGroup;
         Coroutine currentActiveFade = null;
         

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmadiate()
        {
            _canvasGroup.alpha = 1;
        }
        

        public  IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }
        
        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        private IEnumerator Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade= StartCoroutine(FadeRoutine(target,time));
            yield return currentActiveFade;
        }
        
        private IEnumerator FadeRoutine(float target,float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha , target))
            {
                _canvasGroup.alpha += Mathf.MoveTowards(target, time,Time.deltaTime / time);
                yield return null;
            }
        }
    }
}