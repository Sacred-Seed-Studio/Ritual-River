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
    T8,
    Empty
}


public class GameController : MonoBehaviour
{
    public static GameController controller;

    public static int STARTING_WATER_LEVEL = 20;
    public static int STARTING_POPULATION = 100;
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
    {
        get { return collectedWaterLevel; }
        set
        {
            collectedWaterLevel += value;
            if (CollectedWaterLevel > BucketSize) CollectedWaterLevel = BucketSize;
            else if (CollectedWaterLevel < 0) CollectedWaterLevel = 0;
        }
    }

    private int bucketSize = 10;
    public int BucketSize
    { get { return bucketSize; } set { bucketSize += value; } }

    public string BucketName { get; set; }
    public float Speed { get; set; }
    public float AverageSpeed { get; set; }
    public float GrandTotalWaterCollected { get; set; }
    public float GrandTotalWaterLost { get; set; }
    public float GrandTotalGodsPleased { get; set; }
    public float GrandTotalGodsAngered { get; set; }
    public int Day { get; set; }

    private bool shutOffMessage = false;
    [HideInInspector]
    public bool doneCollectingWater = false;
    private bool gameOver = false;
    private float timeForCollecingWater = 120f; //120 seconds for collecting the water mini game

    public bool openGate = false;
    public bool loseWater;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public Gate gate;

    public Pedestal[] pedestals;
    PedestalType[] pedestalsNeeded;
    public Sprite[] pedestalSymbols;
    public Color[] pedestalColors;

    //[HideInInspector]
    public int correctPedestalsTouched;

    public Text timeText, dayText;

    public GameObject torchWaterPanel;
    Slider waterSlider;
    GameObject torchPanel;

    public bool torchesVisible = true;

    bool waitingForInput = false;

    List<Enemy> monkeys;
    public Transform[] spawnLocations;

    public int monkeyCount = 3;
    List<int> usedSpawnLocations;


    //Mini game parameters
    void Awake()
    {
        if (controller == null)
        {
            controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }

        Day = 1;
        CurrentWaterLevel = 2;
        TotalWaterLevel = STARTING_WATER_LEVEL;
        Population = STARTING_POPULATION;

        waterSlider = torchWaterPanel.GetComponentInChildren<Slider>();
        waterSlider.gameObject.SetActive(false);
        torchPanel = torchWaterPanel.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gate = GameObject.FindGameObjectWithTag("Gate").GetComponent<Gate>();

        monkeys = new List<Enemy>();
        GameObject monkeyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            monkeys.Add((Instantiate(monkeyPrefab, spawnLocations[i].position, Quaternion.identity) as GameObject).GetComponent<Enemy>());
            monkeys[i].gameObject.SetActive(false);
            monkeys[i].gameObject.name = "Monkey" + i;
            //monkeys[i].transform.SetParent(transform);
        }

        SpawnMonkeys();

    }

    void SpawnMonkeys()
    {
        if (usedSpawnLocations == null) usedSpawnLocations = new List<int>();

        for (int i = usedSpawnLocations.Count; i < monkeyCount; i++)
        {
            int spawnLocationIndex = Random.Range(0, spawnLocations.Length);
            while (usedSpawnLocations.Contains(spawnLocationIndex))
            {
                spawnLocationIndex = Random.Range(0, spawnLocations.Length);
            }
            usedSpawnLocations.Add(spawnLocationIndex);
            monkeys[spawnLocationIndex].gameObject.SetActive(true);
        }
    }
    void Start()
    {
        StartCoroutine(StartGame());
    }

    void Update()
    {
        if (TotalWaterLevel < 0) gameOver = true;
        if (correctPedestalsTouched >= 3) openGate = true;
        UpdateUITorchesWaterLevel();

        if (loseWater)
        {
            loseWater = false;
            LoseWater();
        }

        if (waitingForInput && Input.GetButtonDown("Submit"))
        {
            shutOffMessage = true;
        }
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
        Population += 1;
        torchesVisible = true;
        doneCollectingWater = false;
        monkeyCount++;
        if (monkeyCount > 11) monkeyCount = 11;
        gate.ResetGates();

        correctPedestalsTouched = 0;
        openGate = false;
        TouchedWrongPedestal();
        TotalWaterLevel = -Population;
        if (TotalWaterLevel < 0) gameOver = true;
    }

    public IEnumerator ShowMessage(string message, string message2 = "Start Day")
    {
        waitingForInput = true;
        messageWindow.ShowMessage(message, "Day: " + Day.ToString(), "Population: " + Population.ToString(),
            "New Total Water: " + TotalWaterLevel.ToString(), message2);
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
        waitingForInput = false;
        yield return null;
    }

    public IEnumerator ShowMessage(PedestalType[] symbols, string message2 = "Start Day")
    {
        waitingForInput = true;
        messageWindow.ShowMessage(symbols, "Day: " + Day.ToString(), "Population: " + Population.ToString(), "Total Water: " + TotalWaterLevel.ToString(), message2);
        messageWindow.gameObject.SetActive(true);
        shutOffMessage = false;

        while (!shutOffMessage)
        {
            //wait for the player to hit the button
            yield return null;
        }

        shutOffMessage = false;
        messageWindow.gameObject.SetActive(false);
        waitingForInput = false;
        yield return null;
    }

    public IEnumerator StartDay()
    {
        dayText.text = "Day " + Day;
        waterSlider.maxValue = BucketSize;

        SpawnMonkeys();
        SetMonkeysToMode(EnemyState.Chillin);
        RandomizePedestals();
        pedestalsNeeded = GetRandomPedestals();
        yield return StartCoroutine(ShowMessage(pedestalsNeeded));

        yield return null;
    }

    public IEnumerator StartCollectingWater()
    {
        player.allowedToMove = true;
        float currentTime = 0;
        float delay = 1f; // so we can count down 2 minutes

        while (currentTime < timeForCollecingWater && !doneCollectingWater)
        {
            timeText.text = (timeForCollecingWater - currentTime).ToString();
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
        GrandTotalWaterCollected = CurrentWaterLevel;
        Debug.Log("Grand total water collected: " + GrandTotalWaterCollected);
        currentWaterLevel = 0;

        yield return StartCoroutine(ShowMessage("Good night.", "Next Day")); ;

        yield return null;
    }

    public void ShutOffMessage()
    {
        shutOffMessage = true;
    }

    PedestalType[] GetRandomPedestals(int n = 3)
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

        PedestalType[] p = new PedestalType[3];
        p[0] = p1;
        p[1] = p2;
        p[2] = p3;
        return p;
    }

    public bool IsCorrectPedestal(PedestalType p)
    {
        for (int i = 0; i < pedestalsNeeded.Length; i++)
        {
            if (pedestalsNeeded[i] == p) return true;
        }
        return false;
    }

    public void TouchedWrongPedestal()
    {
        //reset all pedestals
        StartCoroutine(TouchedWrong());
    }

    IEnumerator TouchedWrong()
    {
        correctPedestalsTouched = 0;
        yield return new WaitForSeconds(0.5f);
        foreach (Pedestal p in pedestals)
        {
            //p.sr.color = p.inactiveColor;
            p.DisActivate();
            yield return null;
        }
        yield return null;
    }

    public void RandomizePedestals()
    {
        List<PedestalType> typesUsed = new List<PedestalType>();

        foreach (Pedestal p in pedestals)
        {
            while (p.pType == PedestalType.Empty)
            {
                PedestalType pt = (PedestalType)Random.Range(0, 8);
                if (!typesUsed.Contains(pt))
                {
                    typesUsed.Add(pt);
                    p.ChangeType(pt, GetPedestalSprite(pt), GetPedestalColor(pt));
                }
            }
        }
    }

    public void UpdateUITorchesWaterLevel()
    {
        if (torchesVisible) //show the torches that are lit so far
        {
            waterSlider.gameObject.SetActive(false);
            torchPanel.SetActive(true);
            foreach (Pedestal_UI pU in torchPanel.GetComponentsInChildren<Pedestal_UI>())
            {
                pU.TurnOff();
            }
            for (int i = 0; i < correctPedestalsTouched; i++)
            {
                torchWaterPanel.GetComponentsInChildren<Pedestal_UI>()[i].TurnOn();
            }
        }
        else //show the current water level
        {
            torchPanel.SetActive(false);
            waterSlider.gameObject.SetActive(true);

            waterSlider.value = CurrentWaterLevel;
        }
    }

    public void LoseWater(float percentage = 0.25f)
    {
        StartCoroutine(LoseWaterLevel(percentage));
        StartCoroutine(Knockback());
    }

    IEnumerator LoseWaterLevel(float percentage = 0.25f)
    {
        int count = 0;

        while (count < (int)Mathf.Ceil(bucketSize * percentage))
        {
            Debug.Log(currentWaterLevel);
            currentWaterLevel -= 1;
            Debug.Log(currentWaterLevel);
            if (currentWaterLevel < 0)
            {
                currentWaterLevel = 0;
                break;
            }
            count++;
            yield return new WaitForSeconds(0.05f);
        }
        GameController.controller.GrandTotalWaterLost += count;
        yield return null;
    }

    IEnumerator Knockback()
    {
        player.allowedToMove = false;
        StartCoroutine(player.Stunned(2f));
        float direction = -Mathf.Sign(player.movement.anim.GetFloat("y"));
        Vector2 delta = player.movement.rb2d.position + (direction > 0 ? Vector2.up * player.knockback : Vector2.down * player.knockback);
        player.movement.MoveTo(delta);
        //player.movement.rb2d.position = delta;
        float t = 0;

        yield return new WaitForSeconds(0.75f);
        player.allowedToMove = true;
        player.tag = "PlayerStunned";
        while (t < player.stunTime)
        {
            t += 1f;
            yield return new WaitForSeconds(2f);
        }
        player.tag = "Player";

        yield return null;
    }

    public Sprite GetPedestalSprite(PedestalType p)
    {
        switch (p)
        {
            default:
            case PedestalType.T5:
            case PedestalType.T1: return pedestalSymbols[0];
            case PedestalType.T6:
            case PedestalType.T2: return pedestalSymbols[1];
            case PedestalType.T7:
            case PedestalType.T3: return pedestalSymbols[2];
            case PedestalType.T8:
            case PedestalType.T4: return pedestalSymbols[3];
        }
    }

    public Color GetPedestalColor(PedestalType p)
    {
        switch (p)
        {
            default:
            case PedestalType.T1: return pedestalColors[0];
            case PedestalType.T2: return pedestalColors[1];
            case PedestalType.T3: return pedestalColors[2];
            case PedestalType.T4: return pedestalColors[3];
            case PedestalType.T5: return pedestalColors[4];
            case PedestalType.T6: return pedestalColors[5];
            case PedestalType.T7: return pedestalColors[6];
            case PedestalType.T8: return pedestalColors[7];
        }
    }

    public void SetMonkeysToMode(EnemyState state = EnemyState.Chillin)
    {
        //TODO: send monkeys back to sides of path?
        foreach (Enemy e in monkeys)
        {
            SetMonkeyToMode(e, state);
        }
    }

    public void SetMonkeyToMode(Enemy e, EnemyState state = EnemyState.Chillin)
    {
        e.state = state;
    }


}

