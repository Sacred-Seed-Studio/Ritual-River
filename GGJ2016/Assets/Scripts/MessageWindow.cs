using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageWindow : MonoBehaviour
{
    public Text extraText;

    public Text dayText;
    public Text populationText;
    public Text collectedWaterText;

    public Text buttonText;

    public Image symbolImage1, symbolImage2, symbolImage3;

    public GameObject pedestals;

    public void ShowMessage(string extra, string day, string population, string collectedWater, string button)
    {
        extraText.text = extra;
        dayText.text = day;
        populationText.text = population;
        collectedWaterText.text = collectedWater;
        buttonText.text = button;

        symbolImage1.gameObject.SetActive(false);
        symbolImage2.gameObject.SetActive(false);
        symbolImage3.gameObject.SetActive(false);
        pedestals.gameObject.SetActive(false);
    }

    public void ShowMessage(PedestalType[] symbols, string button)
    {
        extraText.text = "";

        symbolImage1.gameObject.SetActive(true);
        symbolImage2.gameObject.SetActive(true);
        symbolImage3.gameObject.SetActive(true);

        symbolImage1.sprite = GameController.controller.GetPedestalSprite(symbols[0]);
        symbolImage2.sprite = GameController.controller.GetPedestalSprite(symbols[1]);
        symbolImage3.sprite = GameController.controller.GetPedestalSprite(symbols[2]);

        symbolImage1.color = GameController.controller.GetPedestalColor(symbols[0]);
        symbolImage2.color = GameController.controller.GetPedestalColor(symbols[1]);
        symbolImage3.color = GameController.controller.GetPedestalColor(symbols[2]);

        extraText.text = "pray to the gods";
        dayText.text = "";
        populationText.text = "";
        collectedWaterText.text = "";
        buttonText.text = button;

        pedestals.gameObject.SetActive(true);
    }
}
