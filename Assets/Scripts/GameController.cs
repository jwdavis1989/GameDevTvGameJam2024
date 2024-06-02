using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<GameObject> aisles;
    public bool SpawnMode = false;
    public bool BuildMode = true;
    private bool BreakMode = true;
    public bool ceoEffect = false;
    private bool lastWave = false;
    public TextMeshProUGUI managerWriteUpText;
    public TextMeshProUGUI moneyText; 
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI waitModeTimerText;
    public static GameController instance;
    public GameObject buildMenu;
    private int currentClockTime = 7;
    public float waitTimeBetweenWaves = 30f;
    private float currentWaitTimeRemaining;

    [Header("Player Global Attributes")]
    public int managerWriteUps = 0;
    public int maxManagerWriteUps = 5;
    public static int money;
    public int startingMoney = 500;

    [Header("Tower Base Attributes")]
    public float damage = 50.0f;
    public float range = 5f;
    public float attackSpeed = 1.0f;
    public float generatorBuffMultiplier = 1.25f;
    public float towerSellbackMultiplier = 0.5f;

    [Header("Options Menu Settings")]
    public bool isTurretFireSoundOn = true;
    public bool isBackgroundMusicOn = true;
    public bool isDeathSoundOn = true;
    public bool isMouseCameraPanEnabled = true;
    public float globalVolume = 1f;

    [Header("Cameras")]
    public GameObject buildCamera;
    public GameObject walkCamera;

    void Awake() {
        if (instance) {
            return;
        }
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        managerWriteUps = 0;
        managerWriteUpText.text = "Write-Ups\n" + managerWriteUps + " / " + maxManagerWriteUps;
        money = startingMoney;
        UpdateMoneyTextDisplay();
        UpdateClockTextDisplay(currentClockTime);
        buildMenu.SetActive(BuildMode);
        currentWaitTimeRemaining = waitTimeBetweenWaves;
        StartCoroutine(WaitTimeBetweenWavesTimer(waitTimeBetweenWaves));
        UpdateWaitModeTimerText(currentWaitTimeRemaining);
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (managerWriteUps >= maxManagerWriteUps) {
            GameOver();
        }
        else {
            HandleToggleBuildMenu();

            //Handle Timer Text
            if (BreakMode) {
                currentWaitTimeRemaining -= Time.deltaTime;
                UpdateWaitModeTimerText(currentWaitTimeRemaining);
            }
        }
        //if (!ceoBoosted && other.GetComponent<CustomerController>().type == CustomerType.CEO)
        //{
        //    ceoBoosted = true;
        //}
        
    }
    public void addWriteUp()
    {
        managerWriteUps++;
        if(managerWriteUps >= maxManagerWriteUps)
        {
            managerWriteUpText.text = "Game Over!";
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        else
        {
            managerWriteUpText.text = "Write-Ups\n" + managerWriteUps + " / " + maxManagerWriteUps;
        }
    }

    public void UpdateMoneyTextDisplay() {
        moneyText.text = "$" + money;
    }

    public void UpdateClockTextDisplay(int hoursDigitInMilitaryTime) {
        int hoursMilitaryTime = hoursDigitInMilitaryTime;

        //WORK ON THIS YOU PIECE OF SHIT GOD DAMN HOW COULD YOU FORGET!?

        //if (hour)
        clockText.text = hoursMilitaryTime + ":00";
    }

    public void UpdateWaitModeTimerText(float newTimeRemaining) {
        waitModeTimerText.text = "Break Time!<br>Build Turrets to Live!<br>Break Remaining: " + Mathf.Round(newTimeRemaining);
    }

    public void SetKillCustomersText() {
        waitModeTimerText.text = "Kill them before they report you to your manager!";
    }

    private void GameOver() {
        //Debug.Log("Game Over!");
        //Add Game Over Screen Transition Here
    }

    public void ChangeGameMode(string mode) {
        //Ticket: |[P*] GameController ChangeGameMode() function|
        //if (mode == "")
    }

    public void HandleToggleBuildMenu() {
        //Check hotkeys
        if (Input.GetKeyDown("q") || Input.GetKeyDown("e") || Input.GetKeyDown("f") || Input.GetKeyDown(KeyCode.Tab)) {
            if (BuildMode) {
                //1. Lock Mouse
                Cursor.lockState = CursorLockMode.Locked;

                //2. Enable Main Camera
                //walkCamera.SetActive(true);

                //3. Disable Build Camera
                //buildCamera.SetActive(false);
                
                //4. Disable Build Menu
                buildMenu.SetActive(false);

                //5. Set Build Mode to false
                BuildMode = false;
            }
            else {
                //1. Unlock Mouse
                Cursor.lockState = CursorLockMode.Confined;

                //2. Disable Main Camera
                //walkCamera.SetActive(false);

                //3. Enable Build Camera
                //buildCamera.SetActive(true);

                //4. Enable Build Menu
                buildMenu.SetActive(true);

                //5. Set Build Mode to true
                BuildMode = true;
            }
        }
    }
    public void EndWave(bool lastWave)
    {
        this.lastWave = lastWave;
        SpawnMode = false;
        InvokeRepeating("CheckCustomersAllDead", 1, 1);

    }
    private void CheckCustomersAllDead()
    {
        if(GameObject.FindGameObjectsWithTag("Customer").Length == 0)
        {
            //Debug.Log("Build Mode Started!!");
            CancelInvoke("CheckCustomersAllDead");
            if (this.lastWave)
            {
                //do victory
                SceneManager.LoadScene("Victory", LoadSceneMode.Single);
                return;
            }

                //Start Wait Mode Timer, when timer runs out, set SpawnMode = true
                StartCoroutine(WaitTimeBetweenWavesTimer(waitTimeBetweenWaves));

            //Update Clock Text to next hour
            currentClockTime++;
            UpdateClockTextDisplay(currentClockTime);
            
            currentWaitTimeRemaining = waitTimeBetweenWaves;
            UpdateWaitModeTimerText(currentWaitTimeRemaining);
            BreakMode = true;
        }
    }

    IEnumerator WaitTimeBetweenWavesTimer(float time)
    {
        yield return new WaitForSeconds(time);
        SetKillCustomersText();
        SpawnMode = true;
        BreakMode = false;
    }

    public void SkipToSpawnWave() {
        StopCoroutine(WaitTimeBetweenWavesTimer(currentWaitTimeRemaining));
        //StopAllCoroutines();
        currentWaitTimeRemaining = 1f;
        StartCoroutine(WaitTimeBetweenWavesTimer(currentWaitTimeRemaining));

        //Turn off Build Mode
        buildMenu.SetActive(false);
        BuildMode = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
