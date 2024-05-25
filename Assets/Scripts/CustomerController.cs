using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    //stats
    [Header("Attributes")]
    public int health;
    public float speed;
    //behavior
    [Header("Behavior")]
    public GameObject moveTarget;
    public GameController gameController;
    private int currentAisle = 1;
    private bool goingToBegin = true;
    // Start is called before the first frame update
    void Start()
    {
        //gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveForward();
    }
    void moveForward()
    {
        Vector3 direction = moveTarget.transform.position - transform.position;
        Vector3 newPosition = direction.normalized * speed * Time.deltaTime;
        transform.Translate(newPosition);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision!");
        if (other.CompareTag("AisleMarker"))
        {
            Debug.Log("collision aisle!");
            bool begin = other.GetComponent<AisleMarker>().isBegin;
            if(begin && goingToBegin) // reach beginning of aisle
            {
                goingToBegin = false;
                currentAisle = other.GetComponent<AisleMarker>().aisleNumber;
                GameObject result = gameController.aisles.Find((a) =>
                {
                    AisleMarker marker = a.GetComponent<AisleMarker>();
                    return marker.isEnd && marker.aisleNumber == currentAisle;
                });
                if (result != null)
                {
                    moveTarget = result;
                }
            }
            else if(!begin && !goingToBegin) //Reached end of aisle
            {
                goingToBegin = true;
                currentAisle++;
                GameObject result = gameController.aisles.Find((a) =>
                {
                    AisleMarker marker = a.GetComponent<AisleMarker>();
                    return marker.isBegin && marker.aisleNumber == currentAisle;
                });
                if (result != null)
                {
                    moveTarget = result;
                }
            }
        }
    }
}
