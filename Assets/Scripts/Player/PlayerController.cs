using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer1;

    public float moveSpeed = 5f;

    #region Private Variables

    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 mousePosition = Vector2.zero;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ProcessInputs();

        // Calculate mouse position in world space
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        Move();
        RotateTowardsMouse();
    }

    void ProcessInputs()
    {
        // Get input from the horizontal and vertical axis (WASD by default).
        if (isPlayer1)
            moveDirection = new Vector2(Input.GetAxisRaw("Player1Horizontal"), Input.GetAxisRaw("Player1Vertical")).normalized;
        else
            moveDirection = new Vector2(Input.GetAxisRaw("Player2Horizontal"), Input.GetAxisRaw("Player2Vertical")).normalized;
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
}
