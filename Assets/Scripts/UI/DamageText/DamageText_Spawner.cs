using System;
using UnityEngine;
using RPG.Attributes;

namespace RPG.UI.DamageText
{
    public class DamageText_Spawner : MonoBehaviour
    /* Damage_Text prefabını sürekli Character içerisinde bulundurmaktansa Character içerisinde boş obje oluşturuldu.
     Oluşturulan bu objeye daha önceden hazırlanmış olan "Damage_Text" prefabı entegre edildi. 
     Hasar alma vb. durumlar meydana geldiğinde Damage_Text prefabı Instantiate olmasını sağlayacak olan script..*/
    {
        [SerializeField] Damage_Text _damageText = null;
        
        public void Spawn(float damageAmount) 
        {
            Damage_Text instance = Instantiate(_damageText, transform);
            instance.SetValue(damageAmount);
        }
    }
}