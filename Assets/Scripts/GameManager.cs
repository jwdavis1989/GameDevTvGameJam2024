using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Global Attributes")]
    public static int money;
    public int startingMoney = 2;

    void Awake() {
        if (instance) {
            return;
        }
            instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        money = startingMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
