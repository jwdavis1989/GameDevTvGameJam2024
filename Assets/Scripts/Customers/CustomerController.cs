using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CustomerType
{
    KAREN,
    Toddler,
    Adult,
    Scooter,
    RollerskateKid,
    Mom,
    Dad,
    DisgruntledEmployee
}
public class CustomerController : MonoBehaviour 
{
    //stats
    [Header("Attributes")]
    public float health; 
    public float speed;
    public int money;
    public CustomerType type;
    //behavior
    [Header("Behavior")]
    private GameObject moveTarget;
    public GameController gameController;
    public GameObject toddlerPrefab;
    public GameObject bodyParts;
    private int currentAisle = 0;
    private bool goingToNextAisle = false;
    private bool karenBoosted = false;
    // Start is called before the first frame update
    void Start()
    {
        //gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0.0f)
        {
            //TODO drop moneys
            if (type == CustomerType.Mom)
            { // Spawn toddlers on Mom death
                GameObject toddler1 = Instantiate(toddlerPrefab, transform.position, transform.rotation);
                toddler1.transform.position -= new Vector3(0.5f, 0, 0);
                toddler1.GetComponent<CustomerController>().toddlerSpawn(gameController, currentAisle, goingToNextAisle, moveTarget);
                GameObject toddler2 = Instantiate(toddlerPrefab);
                toddler2.GetComponent<CustomerController>().toddlerSpawn(gameController, currentAisle, goingToNextAisle, moveTarget);
                GameObject toddler3 = Instantiate(toddlerPrefab);
                
                toddler3.transform.position += new Vector3(0.5f, 0, 0);
                toddler3.GetComponent<CustomerController>().toddlerSpawn(gameController, currentAisle, goingToNextAisle, moveTarget);

            }
            Destroy(gameObject);
        }
        moveForward();
        
    }
 
    void FixedUpdate()
    {

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
        //transform.forward = newPosition;
        if(bodyParts != null)
        {
            //ROTATION
            bodyParts.transform.LookAt(moveTarget.transform.position);//method4
            //method1
            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //Vector3 rotation = Quaternion.Lerp(bodyParts.transform.rotation, lookRotation, Time.deltaTime * 1).eulerAngles;
            //bodyParts.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            //method2
            //Vector3 posDifference = moveTarget.transform.position - bodyParts.transform.position;
            //Quaternion rotation = Quaternion.LookRotation(posDifference);
            ////Smooth rotate towards target
            //bodyParts.transform.rotation = Quaternion.Slerp(bodyParts.transform.rotation, rotation, Time.deltaTime);
            //method3
            //float rotationSpeed = .1f;
            //float whichWay = Vector3.Cross(bodyParts.transform.forward, moveTarget.transform.position).y; //Return left or right?
            //if (whichWay < 0) { bodyParts.transform.Rotate(0, -rotationSpeed * Time.fixedDeltaTime, 0); } //If -ve we go CCW 
            //if (whichWay > 0) { bodyParts.transform.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0); }  //IF +ve we go CW
        }
    }
    public void spawn(GameController gameController)
    {
        currentAisle = 0;
        goingToNextAisle = false;
        this.gameController = gameController;
        this.SelectNextMoveTarget();
    }
    public void toddlerSpawn(GameController gameController, int currentAisle, bool goingToNextAisle, GameObject moveTarget)
    {
        this.currentAisle = currentAisle;
        this.gameController = gameController;
        this.goingToNextAisle = goingToNextAisle;
        this.moveTarget = moveTarget;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collision!");
        if (other.CompareTag("AisleMarker"))
        {
            if(other.GetComponent<AisleMarker>() != null && other.GetComponent<AisleMarker>().isActive() 
                && other.GetComponent<AisleMarker>().aisle.GetComponent<AisleController>().aisleNumber == currentAisle) {
                SelectNextMoveTarget();
                //Debug.Log("Collision Selecting next move"); 
            }
            
        }else if (other.CompareTag("Manager"))
        {
            gameController.addWriteUp();
            if(type == CustomerType.KAREN)
            {
                gameController.addWriteUp();
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Customer"))
        {
            if (!karenBoosted && other.GetComponent<CustomerController>().type == CustomerType.KAREN)
            {
                karenBoosted = true;
                speed += 10;
            }
        }
    }
    public void setAisle(int aisle)
    {
        currentAisle += aisle;
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
            else
            {
                this.moveTarget = GameObject.FindWithTag("Manager");
            }
            
        }
        else
        {//reached end of aisle get next aisle
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
            else
            {
                this.moveTarget = GameObject.FindWithTag("Manager");
            }
        }
    }
}
