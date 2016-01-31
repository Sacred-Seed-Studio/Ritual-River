using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        gameObject.AddComponent<BoxCollider2D>();
    }

    public void Randomize(Sprite s)
    {
        sr.sprite = s;
        //sr.sortingOrder += 100;
    }
}
