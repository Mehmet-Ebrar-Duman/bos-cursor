using UnityEngine;
using FireRescue2D.Managers;

namespace FireRescue2D.World
{
    [RequireComponent(typeof(Collider2D))]
    public class Fire : MonoBehaviour
    {
        [Header("Fire Health")]
        [SerializeField] private float maxIntensity = 100f;
        [SerializeField] private float currentIntensity = 100f;

        [Header("Spread Settings")]
        [SerializeField] private float spreadIntervalSeconds = 3f;
        [SerializeField] private float spreadRadius = 3f;
        [SerializeField] private int maxNeighbors = 2;
        [SerializeField] private LayerMask treeLayerMask;
        [SerializeField] private GameObject firePrefab;

        [Header("Scoring")]
        [SerializeField] private int extinguishScore = 10;

        private float spreadTimer;
        private bool isExtinguished = false;

        private void OnEnable()
        {
            currentIntensity = Mathf.Clamp(currentIntensity, 0f, maxIntensity);
            spreadTimer = spreadIntervalSeconds;
        }

        private void Update()
        {
            if (isExtinguished) return;

            spreadTimer -= Time.deltaTime;
            if (spreadTimer <= 0f)
            {
                TrySpread();
                spreadTimer = spreadIntervalSeconds;
            }
        }

        public void Extinguish(float waterPower)
        {
            if (isExtinguished) return;
            currentIntensity -= waterPower;
            if (currentIntensity <= 0f)
            {
                currentIntensity = 0f;
                ExtinguishCompletely();
            }
        }

        private void ExtinguishCompletely()
        {
            if (isExtinguished) return;
            isExtinguished = true;
            GameManager.Instance.AddScore(extinguishScore);
            Destroy(gameObject);
        }

        private void TrySpread()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, spreadRadius, treeLayerMask);
            if (hits == null || hits.Length == 0 || firePrefab == null) return;

            int spawned = 0;
            for (int i = 0; i < hits.Length && spawned < maxNeighbors; i++)
            {
                Vector3 pos = hits[i].transform.position + (Vector3)(Random.insideUnitCircle * 0.5f);
                Instantiate(firePrefab, pos, Quaternion.identity);
                spawned++;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spreadRadius);
        }
    }
}

