using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float speed = 1f;
    public float chargeBoost = 2f;

    Rigidbody2D rb2d;
    [HideInInspector]

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    public void Move(Vector2 input)
    {
        Vector2 p = rb2d.position + input.normalized * speed * Time.deltaTime;
        rb2d.MovePosition(p);
    }

    public void Charge(Vector2 input)
    {
        Vector2 p = rb2d.position + input.normalized * chargeBoost *speed * Time.deltaTime;
        rb2d.MovePosition(p);
    }
}
