using System.Collections;
using UnityEngine;

public class Script : MonoBehaviour
{
    Rigidbody2D rb;
    Coroutine moveCoroutine;
    float moveSpeed = 3;
    float jumpPower = 7;
    bool isGrounded = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        moveCoroutine = StartCoroutine(Move());
    }
    IEnumerator Move()
    {
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
                rb.velocity = new Vector2(rb.velocity.x,jumpPower);
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
}