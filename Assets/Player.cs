using System.Collections;
using UnityEngine;

public class Script : MonoBehaviour
{
    Rigidbody2D rb;
    Coroutine moveCoroutine;
    Coroutine onWallCoroutine;
    bool stoppedMoving = false;
    float moveSpeed = 3;
    float jumpPower = 7;
    bool isGrounded = false;
    bool onWall = false;
    bool jumpedOnWall;
    int direction = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        moveCoroutine = StartCoroutine(Move());
    }
    private void Update()
    {
        if (onWall && !isGrounded)
        {
            StopCoroutine(moveCoroutine);
        }
        else if(onWallCoroutine != null)
        {
            StopCoroutine(onWallCoroutine);
        }
        if (isGrounded && stoppedMoving)
        {
            moveCoroutine = StartCoroutine(Move());
        }
    }
    IEnumerator Move()
    {
        transform.rotation = Quaternion.identity;
        while (true)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
            yield return null;
        }
    }
    IEnumerator WallJump()
    {
        while (onWall)
        {
            rb.gravityScale = 0.2f;
            if (Input.GetKeyDown(KeyCode.UpArrow) && !jumpedOnWall)
            {
                jumpedOnWall = true;
                rb.AddForce(new Vector2(moveSpeed * direction , jumpPower),ForceMode2D.Impulse);
            }
            yield return null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Right Trigger" || other.gameObject.tag == "Left Trigger")&&!isGrounded)
        {
            onWallCoroutine = StartCoroutine(WallJump());
            onWall = true;
            if (other.gameObject.tag == "Right Trigger")
            {
                transform.rotation = Quaternion.Euler(0, 0, 15);
                direction = 1;
            }
            else if (other.gameObject.tag == "Left Trigger")
            {
                transform.rotation = Quaternion.Euler(0, 0, -15);
                direction = -1;
            }
            jumpedOnWall = false;
            stoppedMoving = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Right Trigger" || other.gameObject.tag == "Left Trigger")
        {
            onWall = false;
            rb.gravityScale = 1;
            if(onWallCoroutine!=null)StopCoroutine(onWallCoroutine);
            moveCoroutine = StartCoroutine(Move());
            stoppedMoving = false;
        }
    }
}