using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        [SerializeField] private int portalID = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destinaton;
        [SerializeField] private float fadeOutTime = 0.5f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        private void OnTriggerEnter(Collider other)
        {
            print("Trigger...");
            
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

         IEnumerator Transition()
        {
            if (portalID < 0)
            {
                Debug.LogError("Errorr");
                yield break;
            }

            
            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            SavinWrapper wrapper = FindObjectOfType<SavinWrapper>();
            wrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(portalID);
            
            wrapper.Load();
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            wrapper.Save();
            
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal) // Player spawnOlacak, navmeshagent position kontrolü ile rotation arasında ki çakışmayı önlemek için kullanıldı.
        {
           GameObject player =  GameObject.FindWithTag("Player");
           //player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position); // Teleport durumunda navmesh ile çakışmaması için nerede spawn olması gerektiğini direkt söylüyoruz.
           player.GetComponent<NavMeshAgent>().enabled = false;
           player.transform.position = otherPortal.spawnPoint.position;
           player.transform.rotation = otherPortal.spawnPoint.rotation;
           player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if(portal.destinaton != destinaton) continue; //Portallar eşleşmediğin de devam ediyoruz.

                return portal;
            }

            return null;
        }
    }

}