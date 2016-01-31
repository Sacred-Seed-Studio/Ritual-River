using UnityEngine;
using System.Collections;

public class Village : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameController.controller.torchesVisible) return;

        Debug.Log("Back to the village!");
        GameController.controller.doneCollectingWater = true;
    }
}
