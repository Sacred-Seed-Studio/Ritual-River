using UnityEngine;
using System.Collections;

public enum Personality
{
    Lazy,
    Speedy,
    Consistent,
    Random
}

public enum EnemyState
{
    Chillin,
    Killin
}

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public EnemyMovement movement;
    Vector3 movementVector;

    sbyte direction = -1; // left is -, right is +

    public int leftBound, rightBound;

    public Personality personality;
    public EnemyState state;

    Animator anim;

    float lazySpeed = 0.5f,
          speedySpeed = 2f,
          consistentSpeed = 1f,
          randomSpeed = 1f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        switch (personality)
        {
            case Personality.Lazy: movement.speed = lazySpeed; break;
            case Personality.Speedy: movement.speed = speedySpeed; break;
            case Personality.Consistent: movement.speed = consistentSpeed; break;
            case Personality.Random: movement.speed = randomSpeed; break;
            default: Debug.Log("Unknown personality"); break;
        }
    }

    void Update()
    {
        // If outside of bounds, clamp and flip
        if (transform.position.x <= leftBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
            direction *= -1;
        }
        if (transform.position.x >= rightBound)
        {
            transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
            direction *= -1;
        }

        switch (state)
        {
            case EnemyState.Chillin:
                anim.SetBool("Walk", false);
                movementVector = Vector3.zero;
                break;
            case EnemyState.Killin:
                anim.SetBool("Walk", true);
                SetMovementVector();
                break;
            default:
                Debug.Log("Unknown enemy state! ARG!");
                break;
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(movementVector.x) != 0 || Mathf.Abs(movementVector.y) != 0) movement.Move(movementVector);
    }

    void SetMovementVector()
    {
        switch (personality)
        {
            case Personality.Lazy:
                movementVector = new Vector3(direction, 0, 0);
                break;
            case Personality.Speedy:
                movementVector = new Vector3(direction, 0, 0);
                break;
            case Personality.Consistent:
                movementVector = new Vector3(direction, 0, 0);
                break;
            case Personality.Random:
                if (Random.value > Random.value)
                {
                    if (Random.value > 0.99) { direction *= -1; }
                    if (Random.value > 0.95) { movement.speed = Random.value * 2.5f; }
                    movementVector = new Vector3(direction, 0, 0);
                }
                break;
            default:
                Debug.Log("Unknown personality, freak out!");
                movementVector = Vector3.zero;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;

        GameController.controller.LoseWater();
    }
}
