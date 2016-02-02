using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoSortingLayers : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (tag == "Player") sr.sortingOrder = Mathf.Abs((int)Mathf.Ceil(transform.position.y * 10)) +10;
        else sr.sortingOrder = Mathf.Abs((int)Mathf.Ceil(transform.position.y*10));
    }
}
