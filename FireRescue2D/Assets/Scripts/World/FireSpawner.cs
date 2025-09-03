using UnityEngine;

namespace FireRescue2D.World
{
    public class FireSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject firePrefab;
        [SerializeField] private Vector2 spawnAreaSize = new Vector2(20f, 12f);
        [SerializeField] private int initialFireCount = 5;

        private void Start()
        {
            for (int i = 0; i < initialFireCount; i++)
            {
                SpawnOne();
            }
        }

        private void SpawnOne()
        {
            if (firePrefab == null) return;
            Vector2 offset = new Vector2(
                Random.Range(-spawnAreaSize.x * 0.5f, spawnAreaSize.x * 0.5f),
                Random.Range(-spawnAreaSize.y * 0.5f, spawnAreaSize.y * 0.5f)
            );
            Vector3 pos = transform.position + (Vector3)offset;
            Instantiate(firePrefab, pos, Quaternion.identity);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 1f));
        }
    }
}

