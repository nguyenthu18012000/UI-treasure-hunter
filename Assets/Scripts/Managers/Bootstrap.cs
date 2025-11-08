using UnityEngine;
using TreasureHunter.Network;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bootstrap : MonoBehaviour
{
    [Header("Prefabs các hệ thống global")]
    [SerializeField] private ApiClient apiClientPrefab;

    private void Awake()
    {
        // Tạo ApiClient nếu chưa tồn tại
        if (ApiClient.Instance == null && apiClientPrefab != null)
        {
            Instantiate(apiClientPrefab);
        }

        // Giữ Bootstrap tồn tại nếu cần
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ✅ Gọi coroutine để load scene sau 1 frame
        StartCoroutine(LoadLoginSceneNextFrame());
    }

    private IEnumerator LoadLoginSceneNextFrame()
    {
        yield return null; // đợi 1 frame
        SceneManager.LoadScene("LoginScene");
    }
}
