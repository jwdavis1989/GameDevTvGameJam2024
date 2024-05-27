using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public List<GameObject> aisles;
    public bool SpawnMode;
    public bool BuildMode;
    public TextMeshProUGUI managerWriteUpText;
    public TextMeshProUGUI moneyText; 
    public static GameController instance;

    [Header("Player Global Attributes")]
    public int managerWriteUps = 0;
    public int maxManagerWriteUps = 10;
    public static int money;
    public int startingMoney = 5;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addWriteUp()
    {
        managerWriteUps++;
        if(managerWriteUps >= 5)
        {
            managerWriteUpText.text = "Game Over!";
        }
        else
        {
            managerWriteUpText.text = "Write-Ups\n" + managerWriteUps + " / 5";
        }
    }

    public void UpdateMoneyTextDisplay() {
        moneyText.text = "$" + money;
    }
}
