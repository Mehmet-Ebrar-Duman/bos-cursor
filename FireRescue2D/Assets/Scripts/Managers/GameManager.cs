using System;
using UnityEngine;

namespace FireRescue2D.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Score & Economy")]
        [SerializeField] private int currentScore = 0;
        [SerializeField] private int seedCount = 0;
        [SerializeField] private int saplingCount = 0;

        [Header("Water Settings")]
        [SerializeField] private float maxWaterCapacity = 100f;
        [SerializeField] private float currentWaterAmount = 100f;
        [SerializeField] private float waterConsumptionPerSecond = 15f;

        public event Action<int> OnScoreChanged;
        public event Action<float, float> OnWaterChanged;
        public event Action<int, int> OnInventoryChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            NotifyAll();
        }

        public bool TryConsumeWater(float deltaTime)
        {
            float amountToConsume = waterConsumptionPerSecond * deltaTime;
            if (currentWaterAmount <= 0f)
            {
                currentWaterAmount = 0f;
                OnWaterChanged?.Invoke(currentWaterAmount, maxWaterCapacity);
                return false;
            }

            currentWaterAmount = Mathf.Max(0f, currentWaterAmount - amountToConsume);
            OnWaterChanged?.Invoke(currentWaterAmount, maxWaterCapacity);
            return true;
        }

        public void RefillWater(float amount)
        {
            currentWaterAmount = Mathf.Clamp(currentWaterAmount + amount, 0f, maxWaterCapacity);
            OnWaterChanged?.Invoke(currentWaterAmount, maxWaterCapacity);
        }

        public void SetWaterToFull()
        {
            currentWaterAmount = maxWaterCapacity;
            OnWaterChanged?.Invoke(currentWaterAmount, maxWaterCapacity);
        }

        public void AddScore(int points)
        {
            currentScore = Mathf.Max(0, currentScore + points);
            OnScoreChanged?.Invoke(currentScore);
        }

        public bool SpendScore(int points)
        {
            if (currentScore < points) return false;
            currentScore -= points;
            OnScoreChanged?.Invoke(currentScore);
            return true;
        }

        public void AddSeeds(int amount)
        {
            seedCount = Mathf.Max(0, seedCount + amount);
            OnInventoryChanged?.Invoke(seedCount, saplingCount);
        }

        public void AddSaplings(int amount)
        {
            saplingCount = Mathf.Max(0, saplingCount + amount);
            OnInventoryChanged?.Invoke(seedCount, saplingCount);
        }

        public bool TryUseSeed()
        {
            if (seedCount <= 0) return false;
            seedCount--;
            OnInventoryChanged?.Invoke(seedCount, saplingCount);
            return true;
        }

        public bool TryUseSapling()
        {
            if (saplingCount <= 0) return false;
            saplingCount--;
            OnInventoryChanged?.Invoke(seedCount, saplingCount);
            return true;
        }

        public int GetScore() => currentScore;
        public float GetCurrentWater() => currentWaterAmount;
        public float GetMaxWater() => maxWaterCapacity;
        public int GetSeedCount() => seedCount;
        public int GetSaplingCount() => saplingCount;

        private void NotifyAll()
        {
            OnScoreChanged?.Invoke(currentScore);
            OnWaterChanged?.Invoke(currentWaterAmount, maxWaterCapacity);
            OnInventoryChanged?.Invoke(seedCount, saplingCount);
        }
    }
}

