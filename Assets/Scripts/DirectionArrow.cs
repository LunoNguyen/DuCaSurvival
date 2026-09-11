using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float circleRadius = 2f;
    [SerializeField] private float spriteAngleOffset = -90f; // -90 if sprite points UP, 0 if it points RIGHT

    private Camera mainCam;
    private Vector2 currentDirection = Vector2.right;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (playerTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        UpdateArrow();
    }

    void UpdateArrow()
    {
        // 1. Get current player position directly (prevents 1-frame lag)
        Vector2 playerPos = playerTransform.position;

        // 2. Mouse position in world coordinates
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -mainCam.transform.position.z;
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);

        // 3. Direction vector
        Vector2 directionToMouse = (mouseWorldPos - playerPos).normalized;

        // 4. Update Position
        transform.position = playerPos + (directionToMouse * circleRadius);

        // 5. Update Rotation
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + spriteAngleOffset);
    }
}