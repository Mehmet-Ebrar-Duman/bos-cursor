using UnityEngine;

namespace FireRescue2D.World
{
    public class Tree : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;

        [Header("Burning")]
        [SerializeField] private GameObject firePrefab;
        [SerializeField] private float igniteThreshold = 25f;
        [SerializeField] private float damagePerSecondWhenOnFire = 10f;

        private Fire attachedFire;

        private void Update()
        {
            if (attachedFire != null)
            {
                currentHealth -= damagePerSecondWhenOnFire * Time.deltaTime;
                if (currentHealth <= 0f)
                {
                    currentHealth = 0f;
                    // Tree destroyed; you can add visuals or drop items here
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (attachedFire != null) return;
            var fire = other.GetComponent<Fire>();
            if (fire != null && currentHealth <= igniteThreshold && firePrefab != null)
            {
                attachedFire = Instantiate(firePrefab, transform.position, Quaternion.identity).GetComponent<Fire>();
            }
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        }
    }
}

