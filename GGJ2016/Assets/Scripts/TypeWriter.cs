using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypeWriter : MonoBehaviour
{
    public string textToDisplay;
    public float letterPause;
    //public AudioClip letterSound;
    public Text textBox;

    void OnEnable()
    {
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
        foreach (char letter in textToDisplay.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
