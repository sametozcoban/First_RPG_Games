using System;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _progressionCharacterClass = null;

        public float GetHealth(CharacterClass characterClass, int level) /* Karakter sınıfı için döngü yapıyoruz, uygun sınıf eşleşirse  
                                                                            scriptable obje içerisinde  belirlenen seviye ne ise ona göre sağlık return ediyoruz. */
        {
            foreach (ProgressionCharacterClass character in _progressionCharacterClass)
            {
                if (character._characterClass == characterClass)
                {
                    //return character.health[level - 1];
                }
            }
            return 0;
        }
        
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass _characterClass;
            public ProgressionStat[] stats;
        }
        
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}