using UnityEngine;
using FireRescue2D.Managers;

namespace FireRescue2D.World
{
    public class SafeZone : MonoBehaviour
    {
        [SerializeField] private int rescueScore = 5;

        public void OnAnimalRescued()
        {
            GameManager.Instance.AddScore(rescueScore);
        }
    }
}

