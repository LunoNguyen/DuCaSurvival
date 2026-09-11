using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float baseHealth = 30f;
    [SerializeField] private float baseDamage = 5f;
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Rigidbody2D rb;
    private Transform playerTransform;
    private float lastAttackTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Cache player reference once at start
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        // Physics velocity changes belong in FixedUpdate
        MoveTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        if (playerTransform == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * baseSpeed;

        // Flip sprite direction
        if (direction.x > 0.05f)
        {
            transform.localScale = Vector3.one;
        }
        else if (direction.x < -0.05f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Handles continuous contact if the player stays touching the obstacle
        TryDamagePlayer(collision.gameObject);
    }

    void TryDamagePlayer(GameObject target)
    {
        if (!target.CompareTag("Player")) return;

        // Cooldown prevents infinite damage spam while touching
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (target.TryGetComponent<Player>(out var player))
            {
                player.TakeDamage(baseDamage);
                lastAttackTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}