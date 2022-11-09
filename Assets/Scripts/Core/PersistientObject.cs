using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistientObject : MonoBehaviour
    {
        [SerializeField] GameObject _persistenObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if(hasSpawned) return;

            SpawnPersistent();
            
            hasSpawned = true;
        }

        private void SpawnPersistent()
        {
            GameObject _persistenObject = Instantiate(_persistenObjectPrefab);
            DontDestroyOnLoad(_persistenObject);
        }
    }

}