using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    Vector3 cameraPosition;

    public float speed = 1f;
    Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 input)
    {
        Vector2 p = rb2d.position + input.normalized * speed * Time.deltaTime;

        rb2d.MovePosition(p);
        cameraPosition.Set(p.x, p.y, -10f);
        Camera.main.transform.position = cameraPosition;
    }
}
