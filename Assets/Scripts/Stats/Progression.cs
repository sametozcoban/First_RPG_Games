using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _progressionCharacterClass = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level) /* Karakter sınıfı için döngü yapıyoruz, uygun sınıf eşleşirse  
                                                                            scriptable obje içerisinde  belirlenen seviye ne ise ona göre sağlık return ediyoruz. */
        {
            BuildLookUp();

            float[] levels =  lookUpTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
            /*foreach (ProgressionCharacterClass character in _progressionCharacterClass) /* Bu arama yöntemi daha sonradan eklenecek özelliklerden dolayı bilgisayarı yoracağından dolayı kaldırıldı.
                                                                                            BuilLookUp methodunda dictionary oluşturuldu. Yöntemlerden hangisini kullanmak isterseniz.
            {
                if (character._characterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in character.stats )
                {
                    if(progressionStat.stat != stat) continue;
                    
                    if(progressionStat.levels.Length < level) continue;
                    
                    return progressionStat.levels[level - 1];
                }
            }
            return 0; - */ 
            }
       

        private void BuildLookUp()
        {
            if(lookUpTable != null) return;

            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass character in _progressionCharacterClass)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in character.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookUpTable[character._characterClass] = statLookupTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            float[] levels =  lookUpTable[characterClass][stat];
            return levels.Length;
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