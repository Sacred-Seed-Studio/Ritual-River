using UnityEngine;
using System.Collections;

public class River : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Touching river ");
        GameController.controller.CurrentWaterLevel = GameController.controller.BucketSize;
        GameController.controller.torchesVisible = false;
    }
}
