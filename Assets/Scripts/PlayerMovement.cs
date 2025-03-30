using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxjumpForce = 15f;
    public float chargeRate = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;
    public float bounceForce = 5f;

    private Rigidbody rb;
    private float currentJumpForce = 0f;
    private bool isGrounded = false;
    private bool isCharging = false;
    private float jumpDirection = 0;
    private bool isTouchingWall;
    private bool canJump = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        CheckGround();

        if (isGrounded && canJump )
        {
            ChargeJump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCharging && canJump)
        {
            NormalJump();
        }
        
        if (!isCharging)
        {
            Move();
        }
    }

    void NormalJump()
    {
        if (isGrounded && canJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxjumpForce * 0.5f, rb.velocity.z);
            isGrounded = false;
            canJump = false;
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(moveInput, 0, 0);
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
    }

    void ChargeJump()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                isCharging = true;
                currentJumpForce += chargeRate * Time.deltaTime;
                currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxjumpForce);

                float moveInput = Input.GetAxisRaw("Horizontal");
                if (moveInput != 0)
                {
                    jumpDirection = moveInput;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (isCharging)
                {
                    isCharging = false;
                    rb.velocity = new Vector3(jumpDirection * moveSpeed, currentJumpForce, rb.velocity.z);
                    currentJumpForce = 0f;
                    isGrounded = false;
                    canJump = false;
                }
            }
        }
    }
    
    /*void Jump()
    {
        rb.velocity = new Vector3(jumpDirection * moveSpeed, currentJumpForce, rb.velocity.z);
    }*/

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 bounceDirection = collision.contacts[0].normal + Vector3.up;
            rb.velocity = bounceDirection.normalized * bounceForce;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canJump = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
