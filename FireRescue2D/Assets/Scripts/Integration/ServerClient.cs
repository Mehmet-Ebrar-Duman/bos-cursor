using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using FireRescue2D.Managers;

namespace FireRescue2D.Integration
{
    public class ServerClient : MonoBehaviour
    {
        [Header("Backend Base URL")]
        [SerializeField] private string baseUrl = "http://localhost:3000";
        [SerializeField] private float minPostIntervalSeconds = 2.0f;

        private bool hasDirtyState;
        private float timeSinceLastPost;

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += _ => MarkDirty();
                GameManager.Instance.OnWaterChanged += (_, __) => MarkDirty();
                GameManager.Instance.OnInventoryChanged += (_, __) => MarkDirty();
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= _ => MarkDirty();
                GameManager.Instance.OnWaterChanged -= (_, __) => MarkDirty();
                GameManager.Instance.OnInventoryChanged -= (_, __) => MarkDirty();
            }
        }

        private void Update()
        {
            timeSinceLastPost += Time.deltaTime;
            if (hasDirtyState && timeSinceLastPost >= minPostIntervalSeconds)
            {
                hasDirtyState = false;
                timeSinceLastPost = 0f;
                PostState();
            }
        }

        private void MarkDirty()
        {
            hasDirtyState = true;
        }

        private void PostState()
        {
            if (GameManager.Instance == null) return;
            var score = GameManager.Instance.GetScore();
            var waterCur = GameManager.Instance.GetCurrentWater();
            var waterMax = GameManager.Instance.GetMaxWater();
            var seeds = GameManager.Instance.GetSeedCount();
            var saplings = GameManager.Instance.GetSaplingCount();

            var json = $"{{\"score\":{score},\"water\":{{\"current\":{waterCur},\"max\":{waterMax}}},\"inventory\":{{\"seeds\":{seeds},\"saplings\":{saplings}}}}}";
            var url = baseUrl.TrimEnd('/') + "/api/state";
            StartCoroutine(PostJson(url, json));
        }

        public void SubmitHighscore(string playerName)
        {
            if (GameManager.Instance == null) return;
            int score = GameManager.Instance.GetScore();
            string safeName = string.IsNullOrEmpty(playerName) ? "Anon" : playerName;
            var json = $"{{\"name\":\"{EscapeJson(safeName)}\",\"score\":{score}}}";
            var url = baseUrl.TrimEnd('/') + "/api/leaderboard";
            StartCoroutine(PostJson(url, json));
        }

        private IEnumerator PostJson(string url, string json)
        {
            using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                byte[] body = Encoding.UTF8.GetBytes(json);
                req.uploadHandler = new UploadHandlerRaw(body);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning($"[ServerClient] POST {url} failed: {req.error}");
                }
            }
        }

        private static string EscapeJson(string input)
        {
            return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}

