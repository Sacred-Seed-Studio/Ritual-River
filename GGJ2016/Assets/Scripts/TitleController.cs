using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleController : MonoBehaviour
{
    [Header("Canvases")]
    public GameObject SplashCanvas;
    public GameObject MenuCanvas;
    public GameObject HelpCanvas;
    [Header("Background")]
    public GameObject menuBackground;
    [Header("Splash Variables")]
    public float delay;
    public float fullLogoTime;
    [Header("SplashImages")]
    public Image splashLogo;
    public Image globalGameJamLogo;

    Color white;
    Color transparent;
    CanvasGroup menuCanvasGroup;

    bool showingSplash;
    bool breakOutEarly;

    void ShowCanvases(bool splash, bool menu, bool help)
    {
        SplashCanvas.SetActive(splash);
        MenuCanvas.SetActive(menu);
        menuBackground.SetActive(menu || help);
        HelpCanvas.SetActive(help);
    }

    void Start()
    {
        Debug.Assert(SplashCanvas != null && MenuCanvas != null && HelpCanvas != null, "Canvases not added");
        Debug.Assert(splashLogo != null && globalGameJamLogo != null, "Logos not added");
        Debug.Assert(menuBackground != null, "Background not added");
        white = Color.white;
        transparent = white;
        transparent.a = 0f;
        ShowCanvases(true, false, false);
        splashLogo.color = transparent;
        globalGameJamLogo.color = transparent;

        menuCanvasGroup = MenuCanvas.GetComponent<CanvasGroup>();
        menuCanvasGroup.alpha = 0.0f;
        StartCoroutine("Splash");
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (!showingSplash) SceneManager.LoadScene("Story");
            else
            {
                breakOutEarly = true;
            }
        }
    }
    public void GetHelp() { ShowCanvases(false, false, true); }

    public void LeaveHelp() { ShowCanvases(false, true, false); }

    IEnumerator Splash()
    {
        showingSplash = true;
        float increment = 0.01f;
        float percentage = 0f;

        #region Sacred Seed Logo
        // Go white
        while (splashLogo.color != white && !breakOutEarly)
        {
            splashLogo.color = Color.Lerp(transparent, white, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }

        // Wait
        yield return new WaitForSeconds(fullLogoTime);

        // Go transparent
        percentage = 0f;
        increment = 0.02f;
        while (splashLogo.color != transparent && !breakOutEarly)
        {
            splashLogo.color = Color.Lerp(white, transparent, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }
        #endregion

        #region Global Game Jam Logo
        increment = 0.01f;
        percentage = 0f;
        // Go white
        while (globalGameJamLogo.color != white && !breakOutEarly)
        {
            globalGameJamLogo.color = Color.Lerp(transparent, white, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }

        // Wait
        yield return new WaitForSeconds(fullLogoTime);

        // Go transparent
        percentage = 0f;
        increment = 0.02f;
        while (globalGameJamLogo.color != transparent && !breakOutEarly)
        {
            globalGameJamLogo.color = Color.Lerp(white, transparent, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }

        showingSplash = false;
        #endregion

        #region Menu
        // Open Menu
        ShowCanvases(false, true, false);

        // Fade menu in
        percentage = 0f;
        increment = 0.01f;
        while (menuCanvasGroup.alpha != 1f)
        {
            menuCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        } 
        #endregion
    }
}
