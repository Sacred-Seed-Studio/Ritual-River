using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public Text day, waterCollected, waterLost, pedestalsPleased, pedestalsAngered;

    void Start()
    {
        day.text = "days in village: " + (GameController.controller.Day - 1);
        waterCollected.text = "water collected: " + GameController.controller.GrandTotalWaterCollected;
        waterLost.text = "water lost: " + GameController.controller.GrandTotalWaterLost;
        pedestalsPleased.text = "gods deligted: " + GameController.controller.GrandTotalGodsPleased;
        pedestalsAngered.text = "gods angered: " + GameController.controller.GrandTotalGodsAngered;

        Destroy(GameController.controller.gameObject);
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit")) SceneManager.LoadScene("Main");
        if (Input.GetButtonDown("Cancel")) SceneManager.LoadScene("Title");
    }
}
