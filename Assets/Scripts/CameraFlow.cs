using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    public Transform target;         // Đối tượng cần theo dõi (thường là Player)
    public float smoothSpeed = 0.125f;
    public Vector3 offset;           // Độ lệch (nếu muốn camera không nằm chính giữa player)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
