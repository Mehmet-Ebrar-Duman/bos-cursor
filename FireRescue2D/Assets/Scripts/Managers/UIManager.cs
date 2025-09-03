using UnityEngine;
using UnityEngine.UI;

namespace FireRescue2D.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Slider waterSlider;
        [SerializeField] private Text waterText;
        [SerializeField] private Text inventoryText;

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += HandleScoreChanged;
                GameManager.Instance.OnWaterChanged += HandleWaterChanged;
                GameManager.Instance.OnInventoryChanged += HandleInventoryChanged;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
                GameManager.Instance.OnWaterChanged -= HandleWaterChanged;
                GameManager.Instance.OnInventoryChanged -= HandleInventoryChanged;
            }
        }

        private void Start()
        {
            // Initialize UI with current values
            HandleScoreChanged(GameManager.Instance.GetScore());
            HandleWaterChanged(GameManager.Instance.GetCurrentWater(), GameManager.Instance.GetMaxWater());
            HandleInventoryChanged(GameManager.Instance.GetSeedCount(), GameManager.Instance.GetSaplingCount());
        }

        private void HandleScoreChanged(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score}";
            }
        }

        private void HandleWaterChanged(float current, float max)
        {
            if (waterSlider != null)
            {
                waterSlider.maxValue = max;
                waterSlider.value = current;
            }
            if (waterText != null)
            {
                waterText.text = $"Water: {Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
            }
        }

        private void HandleInventoryChanged(int seeds, int saplings)
        {
            if (inventoryText != null)
            {
                inventoryText.text = $"Seeds: {seeds} | Saplings: {saplings}";
            }
        }
    }
}

