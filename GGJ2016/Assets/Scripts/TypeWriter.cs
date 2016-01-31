using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class TypeWriter : MonoBehaviour
{
    public string textToDisplay;
    public float letterPause;
    //public AudioClip letterSound;
    public Text textBox;
    public bool loadLevelWhenDone = false;
    public string levelToLoad = "Main";

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

    void Update()
    {
        if (loadLevelWhenDone)
        {
            if (Input.GetButtonDown("Submit"))
            {
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }

    IEnumerator TypeText()
    {
        bool newLineEntered = false;
        string[] lines = textToDisplay.Split(Environment.NewLine.ToCharArray());
        foreach (string line in lines)
        {
            if (line == "")
            {
                if (!newLineEntered)
                {
                    textBox.text += Environment.NewLine;
                    newLineEntered = true;
                }
                else
                {
                    newLineEntered = false;
                }
                continue;
            }

            foreach (char letter in line.ToCharArray())
            {
                textBox.text += letter;
                yield return new WaitForSeconds(letterPause);
            }
            textBox.text += Environment.NewLine;
        }
        if (loadLevelWhenDone)
        {
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
