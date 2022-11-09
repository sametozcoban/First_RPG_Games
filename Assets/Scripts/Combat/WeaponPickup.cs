using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Combat;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        // Kılıç veya farklı bir savaş aletinde ki collider trigger olduğunda Figter scriptinde ki EquippedWeapon methoduna aldığımız Weaponu göndererek kuşandık.
        [SerializeField]  Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquippedWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}

