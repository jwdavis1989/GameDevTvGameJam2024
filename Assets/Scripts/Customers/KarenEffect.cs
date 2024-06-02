using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarenEffect : MonoBehaviour
{
    public float range = 15.0f;
    public GameObject karenObject;
    private CustomerController karen;
    // Start is called before the first frame update
    void Start()
    {
        CustomerController karen = karenObject.GetComponent<CustomerController>();
        //InvokeRepeating("Karen", 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Karen()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");
        foreach (GameObject customer in customers)
        {
            CustomerController cusCntrlr = customer.GetComponent<CustomerController>();
            float distanceToCustomer = Vector3.Distance(transform.position, customer.transform.position);
            if (distanceToCustomer < range)
            {
                cusCntrlr.karenSpeed = karen.speed;
                cusCntrlr.karenBoosted = true;
            }
            else
            {
                cusCntrlr.karenBoosted = false;
            }
        }
    }
}
