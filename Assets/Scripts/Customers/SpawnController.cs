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
    public float spawnRate = 3.0f;
    private float spawnDelay = 1.0f;
    public int waveNumber = 0;
    public int[] adultSpawnRate = new[] { 15, 13, 13, 15, 25, 30, 36, 40, 0, 15 };
    public int[] rollerskateKidSpawnRate = new[] { 3, 5, 12, 10, 8, 8, 16, 10, 15, 15 };
    public int[] scooterRate = new[] { 0, 4, 6, 10, 8, 12, 4, 10, 15, 15 };
    public int[] karenSpawnRate = new[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
    public int[] dadSpawnRate = new[] { 0, 0, 0, 2, 5, 12, 4, 10, 20, 20 };
    public int[] momSpawnRate = new[] { 0, 0, 0, 2, 5, 2, 16, 10, 20, 20 };
    public int[] toddlerSpawnRate = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] ceoSpawnRate = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
    public int[][] waveSpawnRates;
    private System.Random rng = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        gameController = gameControllerObject.GetComponent<GameController>();
        //spawn just inside the door
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z-1);
        InvokeRepeating("SpawnCustomer", spawnDelay, spawnRate);

        waveSpawnRates = new int[customerTypes.Length][];
        waveSpawnRates[((int)CustomerType.Adult)] = adultSpawnRate;
        waveSpawnRates[((int)CustomerType.KAREN)] = karenSpawnRate;
        waveSpawnRates[((int)CustomerType.Toddler)] = toddlerSpawnRate;
        waveSpawnRates[((int)CustomerType.CEO)] = ceoSpawnRate;
        waveSpawnRates[((int)CustomerType.Scooter)] = scooterRate;
        waveSpawnRates[((int)CustomerType.RollerskateKid)] = rollerskateKidSpawnRate;
        waveSpawnRates[((int)CustomerType.Mom)] = momSpawnRate;
        waveSpawnRates[((int)CustomerType.Dad)] = dadSpawnRate;
        gameController.aisles[2].GetComponent<AisleController>().setInactive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnWave()
    {
        waveNumber++;

    }
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
            int customerTypeIndex = UnityEngine.Random.Range(0, spawnRatesCopy.Count);//pick random customer type
            int[] indices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7};
            Shuffle(indices);
            foreach (int i in indices)
            {
                if (spawnRatesCopy[i][waveNumber] > 0)
                {
                    GameObject newCustomer = Instantiate(customerTypes[i], spawnLocation, customerTypes[i].transform.rotation);
                    newCustomer.GetComponent<CustomerController>().spawn(gameController);
                    spawned = true;
                    //subtract this spawn from chart
                    waveSpawnRates[(int)newCustomer.GetComponent<CustomerController>().type][waveNumber]--;
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
            //Debug.Log("AdultSpawnRate:"+waveSpawnRates[(int)CustomerType.Adult][waveNumber]);
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
