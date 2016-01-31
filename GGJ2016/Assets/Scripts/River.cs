﻿using UnityEngine;
using System.Collections;

public class River : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        GameController.controller.torchesVisible = false;
        Debug.Log("Touching river ");
        //GameController.controller.CurrentWaterLevel = GameController.controller.BucketSize;
        StartCoroutine(FillWater());
    }

    IEnumerator FillWater()
    {
        int count = 0;
        while (count < GameController.controller.BucketSize)
        {
            GameController.controller.CurrentWaterLevel = 1;
            count++;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
