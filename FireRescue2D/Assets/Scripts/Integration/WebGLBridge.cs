using System.Runtime.InteropServices;
using UnityEngine;

namespace FireRescue2D.Integration
{
    public static class WebGLBridge
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void WebGL_SendState(int score, float waterCurrent, float waterMax, int seeds, int saplings);
        [DllImport("__Internal")] private static extern void WebGL_ShowToast(string message);
#endif

        public static void SendState(int score, float waterCurrent, float waterMax, int seeds, int saplings)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            try { WebGL_SendState(score, waterCurrent, waterMax, seeds, saplings); } catch { }
#else
            Debug.Log($"[WEBGL_SIM] State => score:{score} water:{waterCurrent}/{waterMax} seeds:{seeds} saplings:{saplings}");
#endif
        }

        public static void ShowToast(string message)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            try { WebGL_ShowToast(message); } catch { }
#else
            Debug.Log($"[WEBGL_SIM] Toast: {message}");
#endif
        }
    }
}

