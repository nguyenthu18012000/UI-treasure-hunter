using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using TreasureHunter.Network;

public class Login : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;

    private ApiClient apiClient;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClick);
        apiClient = ApiClient.Instance;
    }

    void OnLoginClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        var request = new LoginRequest
        {
            username = username,
            password = password
        };

        StartCoroutine(apiClient.Post<LoginResponseDTO>(
            "/auth/login",
            request,
            (res) =>
            {
                Debug.Log("res" + JsonUtility.ToJson(res));

                if (res != null && res.Data != null && res.Data.data != null)
                {
                    string accessToken = res.Data.data.accessToken;
                    PlayerPrefs.SetString("access_token", accessToken);
                    PlayerPrefs.Save();
                    string token = PlayerPrefs.GetString("access_token", "");
                    Debug.Log("AccessToken: " + token);
                    SceneManager.LoadScene("MainScene");
                }
                else
                {
                    Debug.LogError("Login failed: " + res?.Data?.message + " | " + res?.Error);
                }
            }
        ));
    }
}
