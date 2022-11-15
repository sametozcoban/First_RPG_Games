using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1,99)] int startLevel = 1;
        [SerializeField] CharacterClass _characterClass;
    }
}
