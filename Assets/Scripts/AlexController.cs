using UnityEngine;

public class AlexController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector2 movement;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("horizontal", movement.x);
        animator.SetFloat("vertical", movement.y);

        float speed = movement.sqrMagnitude;
        if (speed < 0.01f) speed = 0f;
        animator.SetFloat("speed", speed);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
