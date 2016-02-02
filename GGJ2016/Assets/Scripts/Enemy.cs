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
    float lazyBoost = 0.5f,
      speedyBoost = 3f,
      consistentBoost = 1.5f,
      randomBoost = 1f;

    public float baseSpeed = 1f;

    public float waterToSteal = 2f;

    bool playerInRange;

    void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        Randomize();
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

        WatchForPlayer();

    }

    void FixedUpdate()
    {
        if (!playerInRange)
        {
            movement.Move(movementVector);
        }
        else
        {
            movement.Charge(movementVector);
        }
    }

    public float rayLength = 6f;
    void WatchForPlayer()
    {

        Vector2 dir = Vector2.right;
        if (direction == -1) //left
        {
            dir = Vector2.left;
        }
        else //direction is right
        {
            dir = Vector2.right;
        }
        Debug.DrawRay(transform.position, dir * rayLength, Color.black);
        Physics2D.queriesStartInColliders = false;
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir * rayLength);
        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
        RaycastHit2D backHit = Physics2D.Raycast(transform.position, -dir*rayLength);
        if (backHit.collider != null && backHit.collider.gameObject.tag == "Player")
        {
            direction *= -1;
        }
    }

    public void Randomize()
    {
        personality = (Personality)Random.Range(0, 4);
        switch(Random.Range(0, 8))
        {
            case 0: personality = Personality.Consistent; break;
            case 1: personality = Personality.Lazy; break;
            default:
            case 2: personality = Personality.Speedy; break;
        }
        SetSpeed();
    }

    public void SetSpeed()
    {
        switch (personality)
        {
            case Personality.Lazy: movement.speed = lazySpeed * baseSpeed; movement.chargeBoost = lazyBoost; break;
            case Personality.Speedy: movement.speed = speedySpeed * baseSpeed; movement.chargeBoost = speedyBoost; break;
            case Personality.Consistent: movement.speed = consistentSpeed * baseSpeed; movement.chargeBoost = consistentBoost; break;
            case Personality.Random: movement.speed = randomSpeed * baseSpeed; movement.chargeBoost = randomBoost; break;
            default: Debug.Log("Unknown personality"); break;
        }
    }

    public void SpeedUp()
    {
        baseSpeed *= 1.25f;
    }

    void SetMovementVector()
    {
        movementVector = new Vector3(direction, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;

        GameController.controller.LoseWater(waterToSteal+ (GameController.controller.Day/2f));
        MusicController.controller.PlaySound(MusicType.LoseWater);
    }
}
