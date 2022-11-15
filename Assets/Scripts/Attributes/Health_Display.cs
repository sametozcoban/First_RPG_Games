using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class Health_Display : MonoBehaviour
    {
        Health health;
        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            /*String.Format("{0:0}%", health.HealthPercentage()) kullanılma sebebi can azalırken olabildiğince az decimal number olarak gözükmesini sağlamak.
            (Decimal Number = %86.989 gibi gözükmesini istemiyoruz.) */
            _text.text = String.Format("{0:0}%", health.HealthPercentage());
        }
    }
}