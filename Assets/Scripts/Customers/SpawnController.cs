using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private GameController gameController;
    public GameObject gameControllerObject;
    public List<GameObject> customerTypes = new List<GameObject>();
    private Vector3 spawnLocation;
    public int waveNumber = 0;
    public int[] adultSpawnRate = new[] { 15, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    public int[] rollerskateKidSpawnRate = new[] { 5, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    public int[] scooterRate = new[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    public int[] dadSpawnRate = new[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    public int[] momSpawnRate = new[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    public int[] toddlerSpawnRate = new[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    public int[] karenSpawnRate = new[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    public int[] ceoSpawnRate = new[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0 };
    // Start is called before the first frame update
    void Start()
    {
        gameController = gameControllerObject.GetComponent<GameController>();
        //spawn just inside the door
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z-1);
        InvokeRepeating("SpawnCustomer", 3, 3);
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
        if (gameController.SpawnMode)
        {
            int index = Random.Range(0, customerTypes.Count);
            GameObject newCustomer = Instantiate(customerTypes[index], spawnLocation, customerTypes[index].transform.rotation);
            newCustomer.GetComponent<CustomerController>().spawn(gameController);//start on left
        }
    }
}
