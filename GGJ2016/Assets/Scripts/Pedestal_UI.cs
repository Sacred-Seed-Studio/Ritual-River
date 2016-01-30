using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pedestal_UI : MonoBehaviour
{
    public Sprite pedestalOff, pedestalOn;
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void TurnOn()
    {
        image.sprite = pedestalOn;
    }

    public void TurnOff()
    {
        image.sprite = pedestalOff;
    }
}
