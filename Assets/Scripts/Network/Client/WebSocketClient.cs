using System;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient Instance;

    private WebSocket ws;
    [HideInInspector] public string playerId = Guid.NewGuid().ToString();

    public Action<string> OnServerMessage;

    async void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);

        ws = new WebSocket("ws://localhost:8080/ws");

        ws.OnOpen += () =>
        {
            Debug.Log("Connected!");
            string join = $"{{\"type\":\"join\",\"playerId\":\"{playerId}\"}}";
            ws.SendText(join);
        };
        ws.OnMessage += bytes => OnServerMessage?.Invoke(
            System.Text.Encoding.UTF8.GetString(bytes));

        ws.OnError += e => Debug.LogError("WS‑Err: " + e);
        ws.OnClose += c => Debug.Log("WS closed: " + c);

        await ws.Connect();
    }

    public async void Send(string msg)
    {
        Debug.Log("send!");
        if (ws.State == WebSocketState.Open) await ws.SendText(msg);
    }

    void Update() { ws?.DispatchMessageQueue(); }

    async void OnApplicationQuit() { await ws.Close(); }
}
