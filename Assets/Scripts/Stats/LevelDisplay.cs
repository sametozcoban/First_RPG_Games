using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats _levelStat;
        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            _levelStat = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _text.text = String.Format("{0:0}", _levelStat.GetLevel());
        }
    }
}