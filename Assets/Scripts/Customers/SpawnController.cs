using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class SpawnController : MonoBehaviour
{
    private GameController gameController;
    public GameObject gameControllerObject;
    public GameObject[] customerTypes;
    private Vector3 spawnLocation;
    public float spawnRate = 1.5f;
    public float spawnRateIncrease = 0.1f;
    private float spawnDelay = 1.0f;
    public int waveNumber = 0;
    private int[] adultSpawnRate = new[] { 10, 13, 13, 15, 25, 30, 36, 40, 0, 15 };
    private int[] rollerskateKidSpawnRate = new[] { 3, 5, 12, 10, 8, 8, 16, 10, 15, 15 };
    private int[] scooterRate = new[] { 0, 4, 6, 10, 8, 12, 4, 10, 15, 15 };
    private int[] karenSpawnRate = new[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
    private int[] dadSpawnRate = new[] { 0, 0, 0, 2, 5, 12, 4, 10, 20, 20 };
    private int[] momSpawnRate = new[] { 0, 0, 0, 2, 5, 2, 16, 10, 20, 20 };
    private int[] toddlerSpawnRate = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] ceoSpawnRate = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
    private int[] randomSpawnRate = new[] {0,0,3,8,12,12,12,16,20,25 }; private int RANDOM_INDEX = 8;
    private int[][] waveSpawnRates;
    private System.Random rng = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        gameController = gameControllerObject.GetComponent<GameController>();
        //spawn just inside the door
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z-1);
        InvokeRepeating("SpawnCustomer", spawnDelay, spawnRate);

        waveSpawnRates = new int[customerTypes.Length+1][];
        waveSpawnRates[((int)CustomerType.Adult)] = adultSpawnRate;
        waveSpawnRates[((int)CustomerType.KAREN)] = karenSpawnRate;
        waveSpawnRates[((int)CustomerType.Toddler)] = toddlerSpawnRate;
        waveSpawnRates[((int)CustomerType.CEO)] = ceoSpawnRate;
        waveSpawnRates[((int)CustomerType.Scooter)] = scooterRate;
        waveSpawnRates[((int)CustomerType.RollerskateKid)] = rollerskateKidSpawnRate;
        waveSpawnRates[((int)CustomerType.Mom)] = momSpawnRate;
        waveSpawnRates[((int)CustomerType.Dad)] = dadSpawnRate;
        waveSpawnRates[RANDOM_INDEX] = randomSpawnRate;
        foreach(GameObject aisle in gameController.aisles){
            aisle.GetComponent<AisleController>().setInactive(); //asdf
        }
        gameController.aisles[0].GetComponent<AisleController>().setActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void WaveEnd()
    //{
    //    gameController.SpawnMode = false;
    //    waveNumber++;
    //}
    void SpawnCustomer()
    {
        if(!gameController.SpawnMode) {
            return;
        }
        List<int[]> spawnRatesCopy = new List<int[]>(waveSpawnRates);
        int customersChecked = 0;
        bool spawned = false;
        while (!spawned && customersChecked < customerTypes.Length)
        {
            customersChecked++;
            //int customerTypeIndex = UnityEngine.Random.Range(0, spawnRatesCopy.Count);//pick random customer type
            int[] customerTypeIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8};
            Shuffle(customerTypeIndices);
            foreach (int i in customerTypeIndices)
            {
                if (spawnRatesCopy[i][waveNumber] > 0)
                {
                    GameObject newCustomer;
                    if(i == RANDOM_INDEX) //spawn random
                    {
                        int randomCustomerIndex = UnityEngine.Random.Range(1, 6);//all customer types except KAREN and CEO
                        newCustomer = Instantiate(customerTypes[randomCustomerIndex], spawnLocation, customerTypes[randomCustomerIndex].transform.rotation);
                    }
                    else
                    {
                        newCustomer = Instantiate(customerTypes[i], spawnLocation, customerTypes[i].transform.rotation);
                    }
                    newCustomer.GetComponent<CustomerController>().spawn(gameController);
                    spawned = true;
                    //subtract this spawn from chart
                    waveSpawnRates[i][waveNumber]--;
                    //Debug.Log("AdultSpawnRate:" + waveSpawnRates[(int)CustomerType.Adult][waveNumber]); //astest
                    //Debug.Log("RollerSpawnRate:" + waveSpawnRates[(int)CustomerType.RollerskateKid][waveNumber]);
                    break;
                }
            }
            //if (spawnRatesCopy[customerTypeIndex][waveNumber] > 0)
            //{
            //    GameObject newCustomer = Instantiate(customerTypes[customerTypeIndex], spawnLocation, customerTypes[customerTypeIndex].transform.rotation);
            //    newCustomer.GetComponent<CustomerController>().spawn(gameController);
            //    spawned = true;
            //    //subtract this spawn from chart
            //    waveSpawnRates[(int)newCustomer.GetComponent<CustomerController>().type][waveNumber]--;
            //}
        }
        if (!spawned)
        {
            //Debug.Log("waveEnd");
            //Debug.Log("AdultSpawnRate:" + waveSpawnRates[(int)CustomerType.Adult][waveNumber]);
            //Debug.Log("RollerSpawnRate:" + waveSpawnRates[(int)CustomerType.RollerskateKid][waveNumber]);
            //WaveEnd();
            bool lastWave = false;
            if(waveNumber == 9)
                lastWave = true;
            gameController.EndWave(lastWave); 
            waveNumber++;
            CancelInvoke("SpawnCustomer");
            spawnRate -= spawnRateIncrease;
            InvokeRepeating("SpawnCustomer", spawnDelay, spawnRate);
            if (waveNumber < gameController.aisles.Count) {
                gameController.aisles[waveNumber].GetComponent<AisleController>().setActive();
            }
            //Debug.Log("AdultSpawnRate:" + waveSpawnRates[(int)CustomerType.Adult][waveNumber]);
            //Debug.Log("RollerSpawnRate:" + waveSpawnRates[(int)CustomerType.RollerskateKid][waveNumber]);
            //Debug.Log("not spawned... custChecked:" + customersChecked + "waveNumber:"+waveNumber);
        }

        //if (gameController.SpawnMode)
        //{
        //    int index = Random.Range(0, customerTypes.Length);
        //    GameObject newCustomer = Instantiate(customerTypes[index], spawnLocation, customerTypes[index].transform.rotation);
        //    newCustomer.GetComponent<CustomerController>().spawn(gameController);
        //}
    }
    public void Shuffle(int[] list)
    {
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
