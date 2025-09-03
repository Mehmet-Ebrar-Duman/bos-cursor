using UnityEngine;
using FireRescue2D.Managers;

namespace FireRescue2D.Economy
{
    public class Shop : MonoBehaviour
    {
        [Header("Costs")]
        [SerializeField] private int seedCost = 25;
        [SerializeField] private int saplingCost = 75;

        [Header("UI Prompt")]
        [SerializeField] private GameObject promptPanel;

        private bool isPlayerInside = false;

        private void Update()
        {
            if (!isPlayerInside) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Simple buy flow: E cycles purchases Seed -> Sapling
                TryBuySeed();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                TryBuySapling();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = true;
                if (promptPanel != null) promptPanel.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = false;
                if (promptPanel != null) promptPanel.SetActive(false);
            }
        }

        public void TryBuySeed()
        {
            if (GameManager.Instance.SpendScore(seedCost))
            {
                GameManager.Instance.AddSeeds(1);
            }
        }

        public void TryBuySapling()
        {
            if (GameManager.Instance.SpendScore(saplingCost))
            {
                GameManager.Instance.AddSaplings(1);
            }
        }
    }
}

