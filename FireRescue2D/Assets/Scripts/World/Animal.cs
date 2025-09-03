using UnityEngine;

namespace FireRescue2D.World
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Animal : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float fleeRadius = 4f;
        [SerializeField] private LayerMask fireLayerMask;

        private Rigidbody2D animalRigidbody;
        private Vector2 wanderDirection;
        private float wanderTimer;

        private void Awake()
        {
            animalRigidbody = GetComponent<Rigidbody2D>();
            PickNewWander();
        }

        private void Update()
        {
            Vector2 fleeDir = GetFleeDirectionFromFire();
            Vector2 desiredDir = fleeDir != Vector2.zero ? fleeDir : WanderDirection();
            animalRigidbody.velocity = desiredDir * moveSpeed;
        }

        private Vector2 WanderDirection()
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
            {
                PickNewWander();
            }
            return wanderDirection;
        }

        private void PickNewWander()
        {
            wanderDirection = Random.insideUnitCircle.normalized;
            wanderTimer = Random.Range(1f, 3f);
        }

        private Vector2 GetFleeDirectionFromFire()
        {
            Collider2D[] fires = Physics2D.OverlapCircleAll(transform.position, fleeRadius, fireLayerMask);
            if (fires == null || fires.Length == 0) return Vector2.zero;

            Vector2 flee = Vector2.zero;
            for (int i = 0; i < fires.Length; i++)
            {
                Vector2 away = (Vector2)(transform.position - fires[i].transform.position);
                float distance = away.magnitude + 0.001f;
                flee += away / (distance * distance);
            }
            return flee.normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var safe = other.GetComponent<SafeZone>();
            if (safe != null)
            {
                safe.OnAnimalRescued();
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, fleeRadius);
        }
    }
}

