using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<AudioClip> audioClipList;
    public List<GameObject> aisles;
    public bool SpawnMode = false;
    public bool BuildMode = true;
    private bool BreakMode = true;
    public bool ceoEffect = false;
    private bool lastWave = false;
    public int waveNumber = 0;
    public TextMeshProUGUI managerWriteUpText;
    public TextMeshProUGUI moneyText; 
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI waitModeTimerText;
    public GameObject readyButton;
    public static GameController instance;
    public GameObject buildMenu;
    private TurretShop turretShop;
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
        turretShop = buildMenu.GetComponent<TurretShop>();
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
        if (hoursDigitInMilitaryTime > 12) {
            clockText.text = (hoursDigitInMilitaryTime - 12) + ":00pm";
        }
        else if (hoursDigitInMilitaryTime == 12){
            clockText.text = (hoursDigitInMilitaryTime) + ":00pm";
        }
        else {
            clockText.text = (hoursDigitInMilitaryTime) + ":00am";
        }
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
                //Lock Mouse
                Cursor.lockState = CursorLockMode.Locked;
                
                //Disable Build Menu
                buildMenu.SetActive(false);

                //Set Build Mode to false
                BuildMode = false;
            }
            else {
                //Unlock Mouse
                Cursor.lockState = CursorLockMode.Confined;

                //Enable Build Menu
                buildMenu.SetActive(true);

                //Set Build Mode to true
                BuildMode = true;
            }
        }
    }
    public void EndWave(bool lastWave)
    {
        waveNumber++;
        if (waveNumber == 5 || lastWave)
        {
            GetComponent<AudioSource>().clip = audioClipList[0];
            gameObject.GetComponent<AudioSource>().volume  = 0.2f;
            GetComponent<AudioSource>().Play();
        }
        this.lastWave = lastWave;
        SpawnMode = false;
        InvokeRepeating("CheckCustomersAllDead", 1, 1);

    }
    private void CheckCustomersAllDead()
    {
        // Debug.Log("Customers Remaining: ");
        // Debug.Log(GameObject.FindGameObjectsWithTag("Customer").Length);
        // GameObject[] livingCustomers = GameObject.FindGameObjectsWithTag("Customer");
        // List<GameObject> livingWithMeshCustomers = new List<GameObject>();
        // foreach (GameObject customer in livingCustomers) {
        //     if (customer.GetComponent<MeshRenderer>().enabled) {
        //         livingWithMeshCustomers.Add(customer);
        //     }
        // }

        if(GameObject.FindGameObjectsWithTag("Customer").Length == 0)
        // if (livingWithMeshCustomers.Count <= 0)
        {
            // Debug.Log("No customers found!");
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
            readyButton.SetActive(true);
            turretShop.unlockTowers();
            turretShop.resetTowerButtons();
            turretShop.verifyTowerUnlocks();

            //Enable Turret Shop at end of Wave
            //Unlock Mouse
            Cursor.lockState = CursorLockMode.Confined;

            //Enable Build Menu
            buildMenu.SetActive(true);

            //Set Build Mode to true
            BuildMode = true;
        }
    }

    IEnumerator WaitTimeBetweenWavesTimer(float time)
    {
        yield return new WaitForSeconds(time);
        SetKillCustomersText();
        SpawnMode = true;
        BreakMode = false;
        readyButton.SetActive(false);
        if(waveNumber == 4)
        {
            gameObject.GetComponent<AudioSource>().clip = audioClipList[1];
            gameObject.GetComponent<AudioSource>().volume  = 0.5f;
            gameObject.GetComponent<AudioSource>().Play();
        }
        else if(waveNumber == 9)
        {
            gameObject.GetComponent<AudioSource>().clip = audioClipList[1];
            gameObject.GetComponent<AudioSource>().volume  = 0.5f;
            gameObject.GetComponent<AudioSource>().Play();
        }
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
        readyButton.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
