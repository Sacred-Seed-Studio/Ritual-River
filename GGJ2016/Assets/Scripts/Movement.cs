﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    float speed = 1f;
    public float waterSpeed = 1f, normalSpeed = 1f;

    Vector3 cameraPosition;
    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public Animator anim;

    float topCameraBound = -4.22f;
    float lowCameraBound = -43.73f;

    float topPlayerBound = 3.69f;
    float lowPlayerBound = -47f;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        speed = normalSpeed;
        waterSpeed = speed * 0.65f;
    }

    public void Move(Vector2 input)
    {
        anim.SetFloat("x", input.x);
        anim.SetFloat("y", input.y);
        anim.SetBool("Idle", false);

        Vector2 p = rb2d.position + input.normalized * speed * Time.deltaTime;
        MoveTo(p);
    }

    public void MoveTo(Vector2 p)
    {
        cameraPosition.Set(Camera.main.transform.position.x, Mathf.Clamp(p.y, lowCameraBound, topCameraBound), -10f);
        p.y = Mathf.Clamp(p.y, lowPlayerBound, topPlayerBound);
        rb2d.MovePosition(p);
        Camera.main.transform.position = cameraPosition;
    }

    public void ChangeSpeed(bool carryWater = false)
    {
        if (carryWater) speed = waterSpeed;
        else speed = normalSpeed;
    }

    public void ChangeSpeedHalfWater(float percetageFull)
    {
        speed = Mathf.Lerp(waterSpeed, normalSpeed, percetageFull);
    }
}
