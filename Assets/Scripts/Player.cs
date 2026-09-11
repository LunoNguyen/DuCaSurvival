using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float baseAttack = 10f;
    [SerializeField] private float baseAttackSpeed = 0.25f;
    [SerializeField] private float baseSpeed = 5f;

    // Runtime stats
    private float currentHealth;
    private float currentAttack;
    private float currentAttackSpeed;
    private float currentSpeed;

    [Header("Prefabs & References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D playerCollider;
    private Camera mainCam;
    private float attackTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        mainCam = Camera.main;
    }

    void Start()
    {
        currentHealth = baseHealth;
        currentAttack = baseAttack;
        currentAttackSpeed = baseAttackSpeed;
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        HandleMovement();
        UpdateAnimation();
        Fire();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 moveInput = new Vector2(horizontalInput, verticalInput).normalized;
        rb.linearVelocity = moveInput * currentSpeed;

        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            bool isRunning = rb.linearVelocity.sqrMagnitude > 0.01f;
            animator.SetBool("isRunning", isRunning);
        }
    }

    void Fire()
    {
        if (!Input.GetMouseButton(0) || attackTimer > 0f) return;
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Chưa gán Bullet Prefab vào Player!");
            return;
        }

        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        attackTimer = currentAttackSpeed;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -mainCam.transform.position.z;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Collider2D bulletCol = bulletObj.GetComponent<Collider2D>();
        if (bulletCol != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, bulletCol, true);
        }

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetBulletDmg(currentAttack);
            bullet.Shoot(mouseWorldPos);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died.");
        }
    }
}