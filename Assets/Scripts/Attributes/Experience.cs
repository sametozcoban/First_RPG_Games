using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float experiencePoints = 0;

        public void GainExperince(float experience)
        {
            experiencePoints += experience;
        }
    }
}