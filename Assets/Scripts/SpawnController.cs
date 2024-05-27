using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private GameController gameController;
    public GameObject gameControllerObject;
    public List<GameObject> customerTypes = new List<GameObject>();
    private Vector3 spawnLocation;
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
    void SpawnCustomer()
    {
        if (gameController.SpawnMode)
        {
            int index = Random.Range(0, customerTypes.Count);
            GameObject newCustomer = Instantiate(customerTypes[index], spawnLocation, customerTypes[index].transform.rotation);
            newCustomer.GetComponent<CustomerController>().spawn(gameController, true);//start on left
        }
    }
}
