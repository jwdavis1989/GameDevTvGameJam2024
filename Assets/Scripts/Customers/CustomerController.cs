using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
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
    CEO
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
    public bool karenBoosted = false;
    public float karenSpeed = 12.5f;
    private AudioSource deathSound;
    private GameObject fromTarget;
    private bool goingToCenter = false;
    private const float AISLEDISTANCE = 26.0f;
    private const float CENTERLINE = -0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //gameController = GameObject.Find("GameController").GetComponent<GameController>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0.0f)
        {
            if (deathSound && GameController.instance.isDeathSoundOn)
            {
                deathSound.Play();
            }
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
            if (type == CustomerType.CEO)
            {
                gameController.ceoEffect = false;
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
        if (goingToCenter){
            Vector3 center = new Vector3(CENTERLINE, transform.position.y, transform.position.z);
            if(Vector3.Distance(center, transform.position) < 1.0f)
            {
                goingToCenter = false;
                //direction = moveTarget.transform.position - transform.position;
            }
            else
            {
                direction = center - transform.position;
            }
        }
        Vector3 newPosition;
        if (karenBoosted && karenSpeed > speed)
        {
            newPosition = direction.normalized * karenSpeed * Time.deltaTime;
            newPosition = new Vector3(newPosition.x, 0, newPosition.z);
        }
        else
        {
            newPosition = direction.normalized * speed * Time.deltaTime;
            newPosition = new Vector3(newPosition.x, 0, newPosition.z);
        }
        transform.Translate(newPosition);
        //transform.forward = newPosition;
        if(bodyParts != null)
        {
            //ROTATION
            if (goingToCenter)
            {
                bodyParts.transform.LookAt(new Vector3(CENTERLINE, transform.position.y, transform.position.z));
            }else
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
        this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        this.SelectNextMoveTarget();
        if(type == CustomerType.CEO)
        {
            gameController.ceoEffect = true;
        }
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
            }else if (goingToCenter && other.GetComponent<AisleMarker>() != null && other.GetComponent<AisleMarker>().isActive())
            {
                //SelectNextMoveTarget();
            }


        }
        else if (other.CompareTag("Manager"))
        {
            gameController.addWriteUp();
            if(type == CustomerType.KAREN || type == CustomerType.CEO)
            {
                gameController.addWriteUp();
            }
            if (gameController.ceoEffect)
            {
                gameController.addWriteUp();
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Customer"))
        {
        }
    }
    public void setAisle(int aisle)
    {
        currentAisle = aisle;
    }
    void SelectNextMoveTarget()
    {
        //new code attempt
        if (goingToCenter)
        {
            //reached center continue onward
            this.moveTarget = this.fromTarget;
        }
        else
        //end new code attempt
        if (goingToNextAisle)
        {
            goingToNextAisle = false;
            //this.fromTarget = this.moveTarget; //new 
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
                if (Vector3.Distance(transform.position, moveTarget.transform.position) > AISLEDISTANCE)
                {// longer than an aisle
                    goingToCenter = true;
                }
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
            GameObject holdTarget = this.moveTarget; // ? 
            if (currentAisleObject != null)
            {
                this.moveTarget = currentAisleObject.GetComponent<AisleController>().getClosestSide(transform.position);
                //new code attempt
                if (Vector3.Distance(transform.position, moveTarget.transform.position) > AISLEDISTANCE)
                {// longer than an aisle
                    goingToCenter = true;
                    //holdTarget = this.moveTarget;
                    //this.moveTarget = this.fromTarget;
                    //this.fromTarget = holdTarget;
                }
                //end new code attempt
            }
            else
            {
                this.moveTarget = GameObject.FindWithTag("Manager");
                if (Vector3.Distance(transform.position, moveTarget.transform.position) > AISLEDISTANCE)
                {// longer than an aisle
                    goingToCenter = true;
                }
            }
            
        }
    }
}
