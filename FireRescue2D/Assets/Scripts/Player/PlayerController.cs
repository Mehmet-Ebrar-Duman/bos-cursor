using UnityEngine;
using FireRescue2D.Managers;

namespace FireRescue2D.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float maxSpeed = 8f;

        [Header("References")]
        [SerializeField] private Transform waterNozzlePoint;
        [SerializeField] private WaterStream waterStream;

        private Rigidbody2D playerRigidbody;
        private Camera mainCamera;
        private bool isPlantModeActive = false;

        private void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleAiming();
            HandleSpraying();
            HandlePlantToggle();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;
            Vector2 desiredVelocity = inputDirection * moveSpeed;

            Vector2 newVelocity = Vector2.ClampMagnitude(desiredVelocity, maxSpeed);
            playerRigidbody.velocity = newVelocity;
        }

        private void HandleAiming()
        {
            if (mainCamera == null || waterNozzlePoint == null) return;
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDirection = (mouseWorld - transform.position);
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            waterNozzlePoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void HandleSpraying()
        {
            if (waterStream == null) return;
            bool isMouseDown = Input.GetMouseButton(0);
            if (isMouseDown && GameManager.Instance.TryConsumeWater(Time.deltaTime))
            {
                waterStream.EnableSpray(true);
                waterStream.ApplyWaterDamage();
            }
            else
            {
                waterStream.EnableSpray(false);
            }
        }

        private void HandlePlantToggle()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                isPlantModeActive = !isPlantModeActive;
                Integration.WebGLBridge.ShowToast(isPlantModeActive ? "Plant mode ON" : "Plant mode OFF");
                Managers.GameManager.Instance?.BroadcastState();
            }
        }

        public bool IsPlantModeActive()
        {
            return isPlantModeActive;
        }

        // Exposed for WebGLMessageReceiver
        public void TogglePlantMode()
        {
            isPlantModeActive = !isPlantModeActive;
            Integration.WebGLBridge.ShowToast(isPlantModeActive ? "Plant mode ON" : "Plant mode OFF");
            Managers.GameManager.Instance?.BroadcastState();
        }
    }
}

