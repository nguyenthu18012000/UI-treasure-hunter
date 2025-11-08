using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;

    private float sendInterval = 0.05f; // 20 lần mỗi giây
    private float sendTimer = 0f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();

        sendTimer += Time.deltaTime;
        if (sendTimer >= sendInterval)
        {
            SendPosition();
            sendTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void SendPosition()
    {
        if (WebSocketClient.Instance == null) return;

        string msg = JsonUtility.ToJson(new MoveMsg
        {
            type = "move",
            playerId = "1111113",
            x = transform.position.x,
            y = transform.position.y
        });
        WebSocketClient.Instance.Send(msg);
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
