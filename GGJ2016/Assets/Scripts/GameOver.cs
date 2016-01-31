using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour

{
    public Text day, waterCollected, waterLost, pedestalsPleased, pedestalsAngered;

    void Start()
    {
        day.text = "days in village: " + GameController.controller.Day;
        waterCollected.text = "water collected: " + GameController.controller.GrandTotalWaterCollected;
        waterLost.text = "water lost: " + GameController.controller.GrandTotalWaterLost;
        pedestalsPleased.text = "gods deligted: " + GameController.controller.GrandTotalGodsPleased;
        pedestalsAngered.text = "gods angered: " + GameController.controller.GrandTotalGodsAngered;

        Destroy(GameController.controller.gameObject);
    }
}
