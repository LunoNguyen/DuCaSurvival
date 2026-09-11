using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private Vector2 direction;

    //The Circle that allow arrow floating around
    [SerializeField] private float circleRadius = 2f;
    private Vector2 circleCenter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        circleCenter = playerTransform.position;
        direction = Vector2.up; // Default direction
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(playerTransform == null)
        {
            Destroy(gameObject); // Destroy the arrow if the player is not found
        }
        UpdateArrowDirection();
        PlayerTracking();
    }

    void PlayerTracking()
    {
        if(playerTransform != null)
        {
            circleCenter = (Vector2)playerTransform.position;
        }
    }

    void UpdateArrowPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Ensure the z-coordinate is zero for 2D
        Vector2 directionToMouse = ((Vector2)mousePos - circleCenter).normalized;
        direction = directionToMouse;
        transform.position = circleCenter + direction * circleRadius;
    }

    void UpdateArrowDirection()
    {
        UpdateArrowPosition();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Adjust for arrow pointing up
    }
}
