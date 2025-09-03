using UnityEngine;
using FireRescue2D.Managers;

namespace FireRescue2D.Items
{
    public enum PowerUpType
    {
        WaterRefill,
        ScoreBoost,
        SpeedBoost
    }

    public class PowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUpType powerUpType = PowerUpType.WaterRefill;
        [SerializeField] private float waterAmount = 50f;
        [SerializeField] private int scoreAmount = 10;
        [SerializeField] private float speedBoostAmount = 2f;
        [SerializeField] private float speedBoostDuration = 5f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            switch (powerUpType)
            {
                case PowerUpType.WaterRefill:
                    GameManager.Instance.RefillWater(waterAmount);
                    break;
                case PowerUpType.ScoreBoost:
                    GameManager.Instance.AddScore(scoreAmount);
                    break;
                case PowerUpType.SpeedBoost:
                    var boost = other.GetComponent<Player.PlayerControllerSpeedBoost>();
                    if (boost == null)
                    {
                        boost = other.gameObject.AddComponent<Player.PlayerControllerSpeedBoost>();
                    }
                    boost.ApplyBoost(speedBoostAmount, speedBoostDuration);
                    break;
            }

            Destroy(gameObject);
        }
    }
}

