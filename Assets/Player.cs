using System.Collections;
using UnityEngine;

public class Script : MonoBehaviour
{
    Coroutine moveCoroutine;
    Coroutine wallCoroutine;
    bool onWall;
    bool isGrounded;
    Rigidbody2D rb;
    float moveSpeed = 3;
    float jumpPower = 7;
    float refX = 7;
    float refY = 7;
    Vector2 bounce;
    float timeAfterBounce = 0.3f;
    float slideFactor = 1.1f;
    float wallSlideFactor = -2;
    int direction;
    float rotationFactor = 15;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        moveCoroutine = StartCoroutine(Move());
    }
    private void Update()
    {
        bounce = new Vector2(refX * direction, refY);
    }
    IEnumerator Move()
    {
        rb.gravityScale = 1;
        transform.rotation = Quaternion.identity;
        if (wallCoroutine != null)
        {
            StopCoroutine(wallCoroutine);
            wallCoroutine = null;
        }
        while (true)
        {
            if (!onWall || isGrounded && onWall)
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
                    rb.velocity = new Vector2(rb.velocity.x / slideFactor, rb.velocity.y);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                }
            }
            else if (onWall && !isGrounded)
            {
                wallCoroutine = StartCoroutine(Wall());
            }
            yield return null;
        }
    }
    IEnumerator Wall()
    {
        StopCoroutine(moveCoroutine);
        while (true)
        {
            if (rb.velocity.y < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -direction * rotationFactor));
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, wallSlideFactor);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.rotation = Quaternion.identity;
                rb.gravityScale = 1;
                rb.AddForce(bounce, ForceMode2D.Impulse);
                break;
            }
            if (isGrounded)
            {
                break;
            }
            yield return null;
        }
        float elapsed = 0;
        while (elapsed < timeAfterBounce && !isGrounded)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        moveCoroutine = StartCoroutine(Move());
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Right Trigger" || collision.gameObject.tag == "Left Trigger")
        {
            onWall = true;
            if (collision.gameObject.tag == "Right Trigger")
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Right Trigger" || collision.gameObject.tag == "Left Trigger")
        {
            onWall = false;
        }
    }
}