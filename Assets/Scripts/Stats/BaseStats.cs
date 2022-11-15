using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1,99)] int startLevel = 1;
        [SerializeField] CharacterClass _characterClass;
        [SerializeField] Progression _progression = null;

        public float GetHealth()
        {
            return _progression.GetHealth(_characterClass , startLevel);
        }

        public float GetExperience()
        {
            return 10; 
        }
    }
}
