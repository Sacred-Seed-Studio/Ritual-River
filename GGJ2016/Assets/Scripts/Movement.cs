using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    Rigidbody2D rb2d;

    Vector2 input;
    Vector3 cameraPosition;

    public float speed = 1f;

    public Vector2 groundBounds = new Vector2(5f, 5f);

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(input.x) != 0 || Mathf.Abs(input.y) != 0) Move();
    }

    void Move()
    {
        Vector2 p = rb2d.position + input.normalized * speed * Time.deltaTime;
        p.x = Mathf.Clamp(p.x, -groundBounds.x, groundBounds.x);
        p.y = Mathf.Clamp(p.y, -groundBounds.y, groundBounds.y);

        rb2d.MovePosition(p);
        cameraPosition.Set(rb2d.position.x, rb2d.position.y, -10f);
        Camera.main.transform.position = cameraPosition;
    }
}
