using UnityEngine;
using FireRescue2D.Managers;

namespace FireRescue2D.Economy
{
    public class PlantingSystem : MonoBehaviour
    {
        [SerializeField] private GameObject treePrefab;
        [SerializeField] private LayerMask placeableMask;
        [SerializeField] private float minDistanceBetweenTrees = 1.5f;
        [SerializeField] private float placeRadiusCheck = 0.75f;

        private Player.PlayerController player;
        private Camera mainCamera;

        private void Awake()
        {
            player = FindObjectOfType<Player.PlayerController>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (player == null || treePrefab == null || mainCamera == null) return;
            if (!player.IsPlantModeActive()) return;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 placePos = new Vector2(world.x, world.y);

                if (!IsPlaceable(placePos)) return;

                // Prefer use sapling first, else seed
                if (GameManager.Instance.TryUseSapling() || GameManager.Instance.TryUseSeed())
                {
                    Instantiate(treePrefab, placePos, Quaternion.identity);
                }
            }
        }

        private bool IsPlaceable(Vector2 position)
        {
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(position, placeRadiusCheck, placeableMask);
            if (overlaps != null && overlaps.Length > 0) return false;

            Collider2D[] nearTrees = Physics2D.OverlapCircleAll(position, minDistanceBetweenTrees);
            if (nearTrees != null)
            {
                foreach (var c in nearTrees)
                {
                    if (c.GetComponent<World.Tree>() != null) return false;
                }
            }
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, placeRadiusCheck);
        }
    }
}

