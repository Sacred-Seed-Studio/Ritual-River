using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class NightFader : MonoBehaviour
{
    public float increment = 0.01f;
    public float delay = 0.001f;
    float percentage = 0f;
    CanvasGroup cg;
    // Use this for initialization
    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        percentage = 0f;
        cg.alpha = 0f;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (cg.alpha != 1f)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }

        percentage = 0f;

        while (cg.alpha != 0f)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }
    }


}
