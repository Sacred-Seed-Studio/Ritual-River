﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 1f;

    Vector3 cameraPosition;
    Rigidbody2D rb2d;
    [HideInInspector]
    public Animator anim;

    float topCameraBound = -1.72f;
    float lowCameraBound = -46.6f;

    float topPlayerBound = 3.69f;
    float lowPlayerBound = -47f;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 input)
    {
        anim.SetFloat("x", input.x);
        anim.SetFloat("y", input.y);
        anim.SetBool("Idle", false);

        Vector2 p = rb2d.position + input.normalized * speed * Time.deltaTime;
        cameraPosition.Set(Camera.main.transform.position.x, Mathf.Clamp(p.y, lowCameraBound, topCameraBound), -10f);
        p.y = Mathf.Clamp(p.y, lowPlayerBound, topPlayerBound);
        rb2d.MovePosition(p);
        Camera.main.transform.position = cameraPosition;
    }
}
