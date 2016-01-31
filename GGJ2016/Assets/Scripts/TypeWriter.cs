using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TypeWriter : MonoBehaviour
{
    public string textToDisplay;
    public float letterPause;
    //public AudioClip letterSound;
    public Text textBox;

    void OnEnable()
    {
        textToDisplay = textBox.text;
        textBox.text = "";
        Debug.Assert(textBox != null && textToDisplay != null, "Something not initialized");
        StartCoroutine(TypeText());
    }

    void OnDisable()
    {
        textBox.text = "";
    }

    IEnumerator TypeText()
    {
        string[] lines = textToDisplay.Split(Environment.NewLine.ToCharArray());
        foreach (string line in lines)
        {
            foreach (char letter in line.ToCharArray())
            {
                textBox.text += letter;
                yield return new WaitForSeconds(letterPause);
            }
            textBox.text += Environment.NewLine;
        }
    }
}
