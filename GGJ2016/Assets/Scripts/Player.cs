using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    Vector2 input;
    [HideInInspector]
    public Movement movement;

    void Awake()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(input.x) != 0 || Mathf.Abs(input.y) != 0) movement.Move(input);
    }
}
