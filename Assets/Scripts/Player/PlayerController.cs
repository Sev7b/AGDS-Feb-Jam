using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer1;

    public float moveSpeed = 5f;
    public float dashForce = 15f; // The force added to the player on dashing
    public float dashCooldown = 2f; // Cooldown duration in seconds
    public float dashDuration = 0.5f; // Duration of the dash in seconds

    public Slider dashSlider;

    #region Private Variables

    private Rigidbody2D rb;
    private CircleCollider2D cc2d;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 mousePosition = Vector2.zero;

    private float lastDashTime = -100f;
    private bool isDashing = false; // Indicates if the player is currently dashing
    private float dashEndTime = -1f; // When the current dash ends

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CircleCollider2D>();

        if (dashSlider != null)
        {
            dashSlider.minValue = 0;
            dashSlider.maxValue = dashCooldown;
            dashSlider.value = dashSlider.maxValue;
        }
    }

    void Update()
    {
        ProcessInputs();

        // Calculate mouse position in world space
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if dash duration has ended
        if (isDashing && Time.time >= dashEndTime)
        {
            cc2d.enabled = true;
            isDashing = false;
        }

        if (dashSlider != null)
            UpdateDashSlider();
    }

    void FixedUpdate()
    {
        if (!isDashing) // Only move based on input if not dashing
        {
            Move();
        }

        RotateTowardsMouse();
    }

    void ProcessInputs()
    {
        if (isPlayer1)
            moveDirection = new Vector2(Input.GetAxisRaw("Player1Horizontal"), Input.GetAxisRaw("Player1Vertical")).normalized;
        else
            moveDirection = new Vector2(Input.GetAxisRaw("Player2Horizontal"), Input.GetAxisRaw("Player2Vertical")).normalized;

        // Dash on 'E' press if cooldown is over
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastDashTime + dashCooldown)
        {
            Dash();
        }
    }

    void Move()
    {
        // Apply the movement to the Rigidbody2D component based on the move direction and speed.
        rb.velocity = moveDirection * moveSpeed;
    }

    void RotateTowardsMouse()
    {
        // Calculate the direction from the player to the mouse
        Vector2 direction = mousePosition - rb.position;

        // Calculate the angle to rotate towards in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation towards the mouse position
        rb.rotation = angle;
    }

    void Dash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        lastDashTime = Time.time;

        cc2d.enabled = false;

        if (moveDirection != Vector2.zero)
            rb.AddForce(moveDirection * dashForce, ForceMode2D.Impulse); // Apply dash force in the direction of movement
        else
            rb.AddForce((mousePosition - rb.position).normalized * (dashForce / 1.5f), ForceMode2D.Impulse); // Fallback dash direction if no input direction
    }

    void UpdateDashSlider()
    {
        // Calculate the cooldown progress
        float timeSinceLastDash = Time.time - lastDashTime;
        float cooldownProgress = Mathf.Clamp(timeSinceLastDash / dashCooldown, 0, dashCooldown);

        // Update the slider's value to reflect the cooldown progress
        dashSlider.value = cooldownProgress;

        // If the dash is ready, ensure the slider is full
        if (cooldownProgress >= dashCooldown)
        {
            dashSlider.value = dashCooldown;
        }
    }
}
