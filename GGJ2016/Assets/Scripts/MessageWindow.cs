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

    public void ShowMessage(string extra, string day, string population, string collectedWater, string button)
    {
        extraText.text = extra;
        dayText.text = day;
        populationText.text = population;
        collectedWaterText.text = collectedWater;
        buttonText.text = button;
    }

}
