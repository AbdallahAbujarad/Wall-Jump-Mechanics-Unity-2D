using UnityEngine;

public class Script : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed = 3;
    float jumpPower = 7;
    bool isGrounded = false;
    bool triggeringWall = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    void Update()
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isGrounded && triggeringWall)
        {
            float direction = transform.rotation.eulerAngles.z > 0 ? -1 : 1;
            rb.velocity = new Vector2(direction * moveSpeed, jumpPower);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Left Trigger" && !isGrounded)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -15));
        }
        else if (collision.gameObject.tag == "Right Trigger" && !isGrounded)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 15));
        }
        rb.velocity = new Vector2(rb.velocity.x , 0);
        triggeringWall = true;
        rb.gravityScale = 0.2f;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        transform.rotation = Quaternion.identity;
        rb.gravityScale = 1;
        triggeringWall = false;
    }
}