using UnityEngine;
using System.Collections;

public class Pedestal : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sr;

    public Color activeColor = Color.black;
    public Color inactiveColor = Color.white;
    public Color angryColor = Color.red;
    public PedestalType pType = PedestalType.T1;

    Animator anim;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameController.controller.torchesVisible) return;
        Debug.Log("Touching pedestal ");
        Activate();
    }

    public void Activate()
    {
        if (GameController.controller.IsCorrectPedestal(pType))
        {
            //sr.color = activeColor;
            GameController.controller.correctPedestalsTouched += 1;
            anim.SetBool("Off", false);
        }
        else
        {
            sr.color = angryColor;
            GameController.controller.TouchedWrongPedestal();
            anim.SetBool("Off", true);
        }
    }




    public void DisActivate()
    {
        sr.color = inactiveColor;
        anim.SetBool("Off", true);
    }


    public void Reset()
    {
        DisActivate();
    }
}
