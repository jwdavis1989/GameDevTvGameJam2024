using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public List<GameObject> aisles;
    public bool SpawnMode;
    public bool BuildMode;
    public bool ceoEffect = false;
    public TextMeshProUGUI managerWriteUpText;
    public TextMeshProUGUI moneyText; 
    public TextMeshProUGUI clockText;
    public static GameController instance;
    public GameObject buildMenu;

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
        UpdateClockTextDisplay(7, "am");
        buildMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (managerWriteUps >= maxManagerWriteUps) {
            GameOver();
        }
        else {
            HandleToggleBuildMenu();
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
        }
        else
        {
            managerWriteUpText.text = "Write-Ups\n" + managerWriteUps + " / " + maxManagerWriteUps;
        }
    }

    public void UpdateMoneyTextDisplay() {
        moneyText.text = "$" + money;
    }

    public void UpdateClockTextDisplay(int hoursDigit, string amOrPm) {
        clockText.text = hoursDigit + ":00" + amOrPm;
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
    public void EndWave()
    {
        SpawnMode = false;
        InvokeRepeating("CheckCustomersAllDead", 1, 1);

    }
    private void CheckCustomersAllDead()
    {
        if(GameObject.FindGameObjectsWithTag("Customer").Length == 0)
        {
            //Debug.Log("Build Mode Started!!");
            CancelInvoke("CheckCustomersAllDead");
            BuildMode = true;
        }
    }
}
