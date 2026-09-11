using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifeTime = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float crashAnimDelay = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void Shoot(Vector3 targetWorldPosition)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Vector2 spawnPos = transform.position;
        Vector2 targetPos = new Vector2(targetWorldPosition.x, targetWorldPosition.y);

        Vector2 direction = targetPos - spawnPos;

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector2.right;
        }
        else
        {
            direction.Normalize();
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        rb.linearVelocity = direction * bulletSpeed;

        Destroy(gameObject, bulletLifeTime);
    }

    public void SetBulletDmg(float dmg)
    {
        damage = dmg;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        HandleHit(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        HandleHit(collision.gameObject);
    }

    private void HandleHit(GameObject hitTarget)
    {
        if (hasHit) return;

        if (hitTarget.CompareTag("Obstacle"))
        {
            hasHit = true;

            Obstacle obstacle = hitTarget.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.TakeDamage(damage);
            }

            rb.linearVelocity = Vector2.zero;
            if (col != null) col.enabled = false;

            if (animator != null)
            {
                animator.SetBool("isCrashed", true);
            }

            Destroy(gameObject, crashAnimDelay);
        }
    }
}