using UnityEngine;

public class Player : MonoBehaviour
{
    //Initial base status of the player
    [SerializeField] private int  baseHealth = 100;
    [SerializeField] private float baseAttack= 10f;
    [SerializeField] private float baseDefense = 5f;
    [SerializeField] private float baseSpeed = 5f;

    //Initial current status of the player
    [SerializeField] private int currentHealth;
    [SerializeField] private int currentAttack;
    [SerializeField] private int currentDefense;
    [SerializeField] private float currentSpeed;

    //IDK 
    private Rigidbody2D rb;
    private Animator animator;

    //Awake is called when the script instance is being loaded
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        UpdateAnimation();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if(horizontalInput != 0 && verticalInput != 0)
        {
            currentSpeed = Mathf.Sqrt(baseSpeed * baseSpeed / 2);
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, verticalInput * currentSpeed);

        // Move the player in the direction of the input
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f || Mathf.Abs(rb.linearVelocity.y) > 0.1f;
        if(isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
}
