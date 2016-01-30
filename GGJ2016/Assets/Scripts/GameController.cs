using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController controller;

    public static int STARTING_WATER_LEVEL = 20;
    public static int STARTING_POPULATION = 10;
    public GameObject messageWindow;

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
    private float timeForCollecingWater = 2f;//120f; //120 seconds for collecting the water mini game


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
        CurrentWaterLevel = STARTING_WATER_LEVEL;
        Population = STARTING_POPULATION;

        StartCoroutine(StartGame());
    }

    void Update()
    {
        if (CurrentWaterLevel < 0) gameOver = true;
    }

    public IEnumerator StartGame()
    {

        while (!gameOver)
        {
            yield return StartCoroutine(ShowMessage("Hello"));
            yield return StartCoroutine(StartCollectingWater());

            Debug.Log("Done day " + Day);

            //Start the day cycle for the game
            //Day Loop: show day message and current stats, start game -> start timer, day is over when timer is up, show another message, overnight -> next day
            yield return StartCoroutine(ShowMessage("Good night")); ;
            IncrementDay();
        }
        yield return null;
        SceneManager.LoadScene("GameOver");
    }

    void IncrementDay()
    {
        Day += 1;
        CurrentWaterLevel = -Population;
    }

    public IEnumerator ShowMessage(string message)
    {
        //show a message box on screen
        //nothing in the game happens until the message goes away
        //set the text elements on the message window
        messageWindow.GetComponentInChildren<Text>().text = message;
        messageWindow.SetActive(true);
        shutOffMessage = false;

        while (!shutOffMessage)
        {
            Debug.Log("Waiting to turn message off...");
            //wait for the player to hit the button
            yield return null;
        }

        shutOffMessage = false;
        messageWindow.SetActive(false);
        yield return null;
    }

    public IEnumerator StartCollectingWater()
    {
        float currentTime = 0;
        float delay = 1f;

        while (currentTime < timeForCollecingWater && !doneCollectingWater)
        {
            if (doneCollectingWater) delay = 0f;
            Debug.Log("Collecting water...");
            currentTime += delay;
            yield return new WaitForSeconds(delay);
        }
        //Generate a field to walk, Start the timer, let the player loose in the game field
        //Over when the timer is up
        yield return null;
    }


    public void ShutOffMessage()
    {
        shutOffMessage = true;
    }

}
