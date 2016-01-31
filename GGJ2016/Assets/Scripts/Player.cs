using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    [HideInInspector]
    public Vector2 input;
    [HideInInspector]
    public Movement movement;

    [HideInInspector]
    public bool allowedToMove;

    public float knockback = 0.5f;
    public float stunTime = 1f;

    public Color stunnedColor = Color.red;

    SpriteRenderer sr;

    bool flash = false;

    void Awake()
    {
        movement = GetComponent<Movement>();
        sr = GetComponentsInChildren<SpriteRenderer>()[1];
    }

    void Update()
    {
        input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        if (allowedToMove && (Mathf.Abs(input.x) != 0 || Mathf.Abs(input.y) != 0)) movement.Move(input);
        else movement.anim.SetBool("Idle", true);
    }

    public IEnumerator Stunned(float length)
    {
        flash = true;
        StartCoroutine(Flash(Color.white, stunnedColor, 0f, 0.1f));
        yield return new WaitForSeconds(length);
        flash = false;
        yield return null;
    }

    public IEnumerator Flash(Color start, Color target, float percentage, float inc)
    {
        while (flash)
        {
            while (sr.color != target)
            {
                sr.color = Color.Lerp(start, target, percentage);
                percentage += inc;
                yield return null;
            }
            yield return new WaitForSeconds(0.15f);
            Color temp = start;
            start = target;
            target = temp;
        }
        sr.color = Color.white;
        yield return null;
    }
}
