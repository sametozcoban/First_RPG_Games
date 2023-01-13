using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Combat;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        // Kılıç veya farklı bir savaş aletinde ki collider trigger olduğunda Figter scriptinde ki EquippedWeapon methoduna aldığımız Weaponu göndererek kuşandık.
        [SerializeField]  WeaponConfig weapon = null;
        [SerializeField] private float healthToRestore = 0;
        [SerializeField] private float respawnTime = 3f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PickUp(other.gameObject);
            }
        }

        private void PickUp(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquippedWeapon(weapon);
            }

            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            
            StartCoroutine(HideForSeconds(respawnTime));
        }

        IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow; //Weapon Collider false yapıyoruz.
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow); /* Child olan weaponun aktifliğini duruma göre true ya da false dönderiyoruz.
                                                        Alındıktan sonra geçen süreye göre görünüp görünmeyeceğine karar verdiğimiz nokta. */
            }
        }

        public bool handleRaycast(Player_Controller callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp(callingController.gameObject);
            }
            return true;
        }
        
        public CursorType GetCursorType() //PickUp cursor değişimini weaponpick up içerisinde yaptık.
        {
            return CursorType.PickUp;
        }
    }
}

