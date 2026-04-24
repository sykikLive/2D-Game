using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float horizontalInput;
    public float moveSpeed, jumpPower, fallMultiplier = 2.5f, lowJumpMultiplier = 2f;
    bool facingRight = true, isGrounded = false;

    Rigidbody2D rb;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     rb = GetComponent<Rigidbody2D>();   
     animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        if (rb.linearVelocity.y < 0) //Extra gravity so you dont fall in a perfect arc
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump")) //Extra gravity for short hop
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && isGrounded) {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);

        }
    }

    private void FixedUpdate() {

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FlipSprite() {

        if(facingRight && horizontalInput < 0f || !facingRight && horizontalInput > 0f) {
            facingRight = !facingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {

        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }
}
