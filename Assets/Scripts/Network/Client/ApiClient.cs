using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using Newtonsoft.Json;

namespace TreasureHunter.Network
{
    public class ApiClient : MonoBehaviour
    {
        [SerializeField] private string baseUrl = "http://localhost:8080";

         // tạo biến singleton để các chỗ khác gọi được: ApiClient.Instance
        public static ApiClient Instance { get; private set; }

        private void Awake()
        {
            // Đảm bảo chỉ có một Instance duy nhất
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ ApiClient sống xuyên suốt game
        }

        public IEnumerator Get<T>(string endpoint, System.Action<ApiResponse<BaseResponse<T>>> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + endpoint))
            {
                yield return request.SendWebRequest();
                var response = ApiHelper.HandleResponse<T>(request);
                callback?.Invoke(response);
            }
        }

        public IEnumerator Post<T>(string endpoint, object body, System.Action<ApiResponse<BaseResponse<T>>> callback)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            Debug.Log("POST " + endpoint + " BODY = " + json);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(baseUrl + endpoint, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();
                var response = ApiHelper.HandleResponse<T>(request);
                callback?.Invoke(response);
            }
        }

        public IEnumerator Put<T>(string endpoint, object body, System.Action<ApiResponse<BaseResponse<T>>> callback)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(baseUrl + endpoint, "PUT"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();
                var response = ApiHelper.HandleResponse<T>(request);
                callback?.Invoke(response);
            }
        }

        public IEnumerator Delete<T>(string endpoint, System.Action<ApiResponse<BaseResponse<T>>> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete(baseUrl + endpoint))
            {
                yield return request.SendWebRequest();
                var response = ApiHelper.HandleResponse<T>(request);
                callback?.Invoke(response);
            }
        }
    }
}