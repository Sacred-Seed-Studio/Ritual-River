using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypeWriter : MonoBehaviour
{
    public string textToDisplay;
    public float letterPause;
    //public AudioClip letterSound;
    public Text textBox;

    void Start()
    {
        textBox.text = "";
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        Debug.Assert(textBox != null && textToDisplay != null, "Something not initialized");
        StartCoroutine(TypeText());
    }

    void OnDisable()
    {
        textBox.text = "";
    }

    IEnumerator TypeText()
    {
        Debug.Log("Top of typetext");
        foreach (char letter in textToDisplay.ToCharArray())
        {
            Debug.Log("Top of typetext loop");
            textBox.text += letter;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
