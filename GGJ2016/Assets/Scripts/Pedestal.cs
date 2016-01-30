using UnityEngine;
using System.Collections;

public class Pedestal : MonoBehaviour
{
   [HideInInspector] public SpriteRenderer sr;

    public Color activeColor = Color.black;
    public Color inactiveColor = Color.white;
    public Color angryColor = Color.red;
    public PedestalType pType = PedestalType.T1;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Touching pedestal ");
        Activate();
    }

    public void Activate()
    {
        if (GameController.controller.IsCorrectPedestal(pType))
        {
            sr.color = activeColor;
            GameController.controller.correctPedestalsTouched += 1;
        }
        else
        {
            sr.color = angryColor;
            GameController.controller.TouchedWrongPedestal();
        }
    }




    public void DisActivate()
    {
        sr.color = inactiveColor;
    }

    public void Reset()
    {
        DisActivate();
    }
}
