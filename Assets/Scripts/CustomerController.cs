using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    //stats
    [Header("Attributes")]
    public float health;
    public float speed;
    //behavior
    [Header("Behavior")]
    private GameObject moveTarget;
    public GameController gameController;
    private int currentAisle = 0;
    private bool goingToNextAisle = false;
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
        if(moveTarget == null)
        {
            SelectNextMoveTarget();
        }
        Vector3 direction = moveTarget.transform.position - transform.position;
        Vector3 newPosition = direction.normalized * speed * Time.deltaTime;
        transform.Translate(newPosition);

            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 1).eulerAngles;
            //transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    public void spawn(GameController gameController, bool isOnLeft)
    {
        currentAisle = 0;
        goingToNextAisle = false;
        this.gameController = gameController;
        this.SelectNextMoveTarget();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collision!");
        if (other.CompareTag("AisleMarker"))
        {
            if(other.GetComponent<AisleMarker>() != null && other.GetComponent<AisleMarker>().isActive()) {
                SelectNextMoveTarget();
                Debug.Log("Collision Selecting next move");
            }
            
        }else if (other.CompareTag("Manager"))
        {
            gameController.addWriteUp();
            Destroy(gameObject);
        }
    }
    void SelectNextMoveTarget()
    {
        if (goingToNextAisle)
        {
            goingToNextAisle = false;
            //at next aisle now go to other side
            GameObject currentAisleObject = null;
            currentAisleObject = gameController.aisles.Find((a) =>
            {
                return a.GetComponent<AisleController>().aisleNumber == currentAisle;
            });
            if(currentAisleObject != null)
            {
                this.moveTarget = currentAisleObject.GetComponent<AisleController>().getFarthestSide(transform.position);
            }
            else // TODO LOOP FOR AISLE SKIPPING INACTIVE
            {
                this.moveTarget = GameObject.FindWithTag("Manager");
            }
            
        }
        else
        {//reached end of aisle get next aisle
            //todo add isActive check and loop until last aisle
            currentAisle++;
            goingToNextAisle = true;
            GameObject currentAisleObject = null;
            currentAisleObject = gameController.aisles.Find((a) =>
            {
                return a.GetComponent<AisleController>().aisleNumber == currentAisle;
            });
            //skip over inactve
            while (currentAisleObject != null && !currentAisleObject.GetComponent<AisleController>().isActive) {
                currentAisle++;
                currentAisleObject = null;
                currentAisleObject = gameController.aisles.Find((a) =>
                {
                    return a.GetComponent<AisleController>().aisleNumber == currentAisle && a.GetComponent<AisleController>().isActive;
                });
            }
            if (currentAisleObject != null)
            {
                this.moveTarget = currentAisleObject.GetComponent<AisleController>().getClosestSide(transform.position);
            }
            else // TODO LOOP FOR AISLE SKIPPING INACTIVE
            {
                this.moveTarget = GameObject.FindWithTag("Manager");
            }
        }
    }
}
