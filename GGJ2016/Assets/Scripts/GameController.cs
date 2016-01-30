using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum PedestalType
{
    T1,
    T2,
    T3,
    T4,
    T5,
    T6,
    T7,
    T8
}


public class GameController : MonoBehaviour
{
    public static GameController controller;

    public static int STARTING_WATER_LEVEL = 20;
    public static int STARTING_POPULATION = 20;
    public static int groundWidthFromCenter = 5;
    public static int groundHeightFromCenter = 15;

    public MessageWindow messageWindow;

    public int Population { get; set; }

    private int currentWaterLevel;
    private int totalWaterLevel;
    private int collectedWaterLevel;

    public int CurrentWaterLevel//total current water in your personal bucket
    { get { return currentWaterLevel; } set { currentWaterLevel += value; } }
    public int TotalWaterLevel  //total water in your town bucket
    { get { return totalWaterLevel; } set { totalWaterLevel += value; } }
    public int CollectedWaterLevel  //current collected water for the day
    { get { return collectedWaterLevel; } set { collectedWaterLevel += value; } }

    public int BucketSize { get; set; }
    public string BucketName { get; set; }
    public float Speed { get; set; }
    public float AverageSpeed { get; set; }
    public float GrandTotalWaterCollected { get; set; }
    public float GrandTotalWaterLost { get; set; }
    public int Day { get; set; }

    private bool shutOffMessage = false;
    [HideInInspector]
    public bool doneCollectingWater = false;
    private bool gameOver = false;
    public float timeForCollecingWater = 2f;//120f; //120 seconds for collecting the water mini game

    public bool openGate = false;

    [HideInInspector]
    public Player player;

    public Pedestal[] pedestals;
    string pedestalsNeeded;

    [HideInInspector]
    public int correctPedestalsTouched;

    //Mini game parameters
    void Awake()
    {
        if (controller == null)
        {
            controller = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }

        Day = 1;
        CurrentWaterLevel = 2;
        TotalWaterLevel = STARTING_WATER_LEVEL;
        Population = STARTING_POPULATION;

        StartCoroutine(StartGame());

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (TotalWaterLevel < 0) gameOver = true;
        if (correctPedestalsTouched >= 3) openGate = true;
    }

    public IEnumerator StartGame()
    {
        while (!gameOver)
        {
            yield return StartCoroutine(StartDay());
            yield return StartCoroutine(StartCollectingWater());
            yield return StartCoroutine(EndDay());
            IncrementDay();
        }
        yield return null;
        SceneManager.LoadScene("GameOver");
    }

    void IncrementDay()
    {
        Day += 1;
        TotalWaterLevel = -Population;
        if (TotalWaterLevel < 0) gameOver = true;
    }

    public IEnumerator ShowMessage(string message, string message2 = "Start Day")
    {
        //show a message box on screen
        //nothing in the game happens until the message goes away
        //set the text elements on the message window
        //messageWindow.GetComponentsInChildren<Text>()[0].text = message;
        //messageWindow.GetComponentsInChildren<Text>()[1].text = message2;

        messageWindow.ShowMessage(message, "Day: " + Day.ToString(), "Population: " + Population.ToString(), "Total Water: " + TotalWaterLevel.ToString(), message2);
        messageWindow.gameObject.SetActive(true);
        shutOffMessage = false;

        while (!shutOffMessage)
        {
            //Debug.Log("Waiting to turn message off...");
            //wait for the player to hit the button
            yield return null;
        }

        shutOffMessage = false;
        messageWindow.gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator StartDay()
    {
        pedestalsNeeded = GetRandomPedestals();
        yield return StartCoroutine(ShowMessage(pedestalsNeeded));

        yield return null;
    }
    public IEnumerator StartCollectingWater()
    {
        player.allowedToMove = true;
        float currentTime = 0;
        float delay = 1f;

        while (currentTime < timeForCollecingWater && !doneCollectingWater)
        {
            if (doneCollectingWater) delay = 0f;
            //Debug.Log("Collecting water...");
            currentTime += delay;
            yield return new WaitForSeconds(delay);
        }
        //Generate a field to walk, Start the timer, let the player loose in the game field
        //Over when the timer is up
        player.allowedToMove = false;
        yield return null;
    }

    public IEnumerator EndDay()
    {
        TotalWaterLevel = CurrentWaterLevel;

        yield return StartCoroutine(ShowMessage("Good night.", "Next Day")); ;

        yield return null;
    }

    public void ShutOffMessage()
    {
        shutOffMessage = true;
    }

    string GetRandomPedestals(int n = 3)
    {
        PedestalType p1 = PedestalType.T1, p2 = PedestalType.T1, p3 = PedestalType.T1;
        List<PedestalType> types = new List<PedestalType>();
        types.Add(PedestalType.T1);
        types.Add(PedestalType.T2);
        types.Add(PedestalType.T3);
        types.Add(PedestalType.T4);
        types.Add(PedestalType.T5);
        types.Add(PedestalType.T6);
        types.Add(PedestalType.T7);
        types.Add(PedestalType.T8);

        while ((p1 == p2) || (p2 == p3) || (p1 == p3))
        {
            p1 = (PedestalType)Random.Range(0, 8);
            p2 = (PedestalType)Random.Range(0, 8);
            p3 = (PedestalType)Random.Range(0, 8);
        }

        return p1.ToString() + " " + p2.ToString() + " " + p3.ToString();
    }

    public bool IsCorrectPedestal(PedestalType p)
    {
        return pedestalsNeeded.Contains(p.ToString());
    }

    public void TouchedWrongPedestal()
    {
        //reset all pedestals
        StartCoroutine(TouchedWrong());
    }

    IEnumerator TouchedWrong()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Pedestal p in pedestals)
        {
            p.sr.color = p.inactiveColor;
            yield return null;
        }
        yield return null;
    }
}
