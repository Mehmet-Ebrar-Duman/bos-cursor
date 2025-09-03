using UnityEngine;

namespace FireRescue2D.Player
{
    public class PlayerControllerSpeedBoost : MonoBehaviour
    {
        private PlayerController controller;
        private float originalMoveSpeed;
        private float originalMaxSpeed;
        private float remaining;
        private float multiplier = 1f;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
        }

        public void ApplyBoost(float amount, float duration)
        {
            if (controller == null) return;
            if (remaining <= 0f)
            {
                originalMoveSpeed = GetPrivate(controller, "moveSpeed", 5f);
                originalMaxSpeed = GetPrivate(controller, "maxSpeed", 8f);
            }

            multiplier = Mathf.Max(multiplier, amount);
            SetPrivate(controller, "moveSpeed", originalMoveSpeed * multiplier);
            SetPrivate(controller, "maxSpeed", originalMaxSpeed * multiplier);
            remaining = Mathf.Max(remaining, duration);
        }

        private void Update()
        {
            if (remaining > 0f)
            {
                remaining -= Time.deltaTime;
                if (remaining <= 0f)
                {
                    ResetSpeeds();
                    Destroy(this);
                }
            }
        }

        private void ResetSpeeds()
        {
            if (controller == null) return;
            SetPrivate(controller, "moveSpeed", originalMoveSpeed);
            SetPrivate(controller, "maxSpeed", originalMaxSpeed);
        }

        private static float GetPrivate(PlayerController obj, string field, float fallback)
        {
            var fi = typeof(PlayerController).GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fi != null && fi.FieldType == typeof(float))
            {
                return (float)fi.GetValue(obj);
            }
            return fallback;
        }

        private static void SetPrivate(PlayerController obj, string field, float value)
        {
            var fi = typeof(PlayerController).GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fi != null && fi.FieldType == typeof(float))
            {
                fi.SetValue(obj, value);
            }
        }
    }
}

