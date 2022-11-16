using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavinWrapper : MonoBehaviour
    {
        const string defaultScene = "save";
        [SerializeField] float fadeInTime = 0.2f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmadiate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultScene);
            fader.FadeIn(fadeInTime);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultScene);        
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultScene);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultScene);
        }
    }
    
}
