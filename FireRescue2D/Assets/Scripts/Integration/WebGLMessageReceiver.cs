using UnityEngine;
using FireRescue2D.Managers;

namespace FireRescue2D.Integration
{
    public class WebGLMessageReceiver : MonoBehaviour
    {
        [Header("Shop Defaults (used if Shop not found)")]
        [SerializeField] private int seedCost = 25;
        [SerializeField] private int saplingCost = 75;

        public void JS_BuySeed(string unused)
        {
            var shop = FindObjectOfType<Economy.Shop>();
            if (shop != null) shop.TryBuySeed();
            else if (GameManager.Instance.SpendScore(seedCost)) GameManager.Instance.AddSeeds(1);
        }

        public void JS_BuySapling(string unused)
        {
            var shop = FindObjectOfType<Economy.Shop>();
            if (shop != null) shop.TryBuySapling();
            else if (GameManager.Instance.SpendScore(saplingCost)) GameManager.Instance.AddSaplings(1);
        }

        public void JS_TogglePlantMode(string unused)
        {
            var pc = FindObjectOfType<Player.PlayerController>();
            if (pc != null) pc.TogglePlantMode();
        }

        public void JS_RefillWater(string amountStr)
        {
            if (!float.TryParse(amountStr, out float amount)) amount = GameManager.Instance.GetMaxWater();
            GameManager.Instance.RefillWater(amount);
        }

        public void JS_AddScore(string pointsStr)
        {
            if (!int.TryParse(pointsStr, out int points)) points = 10;
            GameManager.Instance.AddScore(points);
        }
    }
}

