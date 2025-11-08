using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject otherPlayerPrefab;

    Dictionary<string, GameObject> others = new();

    void Start()
    {
        // Nếu không có localPlayer, không cần instantiate hay camera theo

        WebSocketClient.Instance.OnServerMessage += HandleMsg;
    }

    void HandleMsg(string msg)
    {
        Debug.Log("Received: " + msg);
        var data = JsonUtility.FromJson<MoveMsg>(msg);
        if (data.type != "move") return;

        // Tạo hoặc cập nhật OtherPlayer
        if (!others.TryGetValue(data.playerId, out var op))
        {
            op = Instantiate(otherPlayerPrefab, Vector3.zero, Quaternion.identity);
            op.GetComponent<OtherPlayerController>().playerId = data.playerId;
            others[data.playerId] = op;
        }
        op.transform.position = new Vector3(data.x, data.y, 0);
    }

    [System.Serializable]
    class MoveMsg
    {
        public string type;
        public string playerId;
        public float x;
        public float y;
    }
}
