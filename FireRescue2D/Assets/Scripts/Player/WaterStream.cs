using UnityEngine;

namespace FireRescue2D.Player
{
    public class WaterStream : MonoBehaviour
    {
        [Header("Spray Settings")]
        [SerializeField] private float sprayRadius = 1.75f;
        [SerializeField] private float waterPowerPerSecond = 25f;
        [SerializeField] private LayerMask fireLayerMask;
        [SerializeField] private ParticleSystem sprayParticles;

        private bool isSpraying;

        public void EnableSpray(bool enable)
        {
            isSpraying = enable;
            if (sprayParticles != null)
            {
                var emission = sprayParticles.emission;
                emission.enabled = enable;
            }
        }

        public void ApplyWaterDamage()
        {
            if (!isSpraying) return;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sprayRadius, fireLayerMask);
            if (hits == null || hits.Length == 0) return;

            float waterThisFrame = waterPowerPerSecond * Time.deltaTime;
            for (int i = 0; i < hits.Length; i++)
            {
                var fire = hits[i].GetComponent<World.Fire>();
                if (fire != null)
                {
                    fire.Extinguish(waterThisFrame);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, sprayRadius);
        }
    }
}

