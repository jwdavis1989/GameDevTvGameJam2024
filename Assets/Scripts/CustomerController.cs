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
    private bool goingToNextAisle = true;
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
        if (moveTarget) {
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
        this.SelectNextMoveTarget(isOnLeft);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collision!");
        if (other.CompareTag("AisleMarker"))
        {
            //Debug.Log("collision aisle!");
            bool onLeftSide = other.GetComponent<AisleMarker>().isLeft;
            SelectNextMoveTarget(onLeftSide);
            
        }else if (other.CompareTag("Manager"))
        {
            gameController.addWriteUp();
            Destroy(gameObject);
        }
    }
    void SelectNextMoveTarget(bool onLeftSide)
    {
        if (goingToNextAisle)
        {
            goingToNextAisle = false;
            //at next aisle now go to other side
            GameObject result = null;
            if (onLeftSide)
            {
                result = gameController.aisles.Find((a) =>
                {
                    AisleMarker marker = a.GetComponent<AisleMarker>();
                    return marker.isRight() && marker.aisleNumber == currentAisle;
                });
            }
            else
            {
                result = gameController.aisles.Find((a) =>
                {
                    AisleMarker marker = a.GetComponent<AisleMarker>();
                    return marker.isLeft && marker.aisleNumber == currentAisle;
                });
            }
            if (result != null)
            {
                moveTarget = result;
            }
            else
            {
                moveTarget = GameObject.FindWithTag("Manager");
            }
            
        }
        else
        {//reached end of aisle get next aisle
            //todo add isActive check and loop until last aisle
            currentAisle++;
            goingToNextAisle = true;
            GameObject result = null;
            if (onLeftSide)
            {
                result = gameController.aisles.Find((a) =>
                {
                    AisleMarker marker = a.GetComponent<AisleMarker>();
                    return marker.isLeft && marker.aisleNumber == currentAisle;
                });
            }
            else
            {
                result = gameController.aisles.Find((a) =>
                {
                    AisleMarker marker = a.GetComponent<AisleMarker>();
                    return (!marker.isLeft) && marker.aisleNumber == currentAisle;
                });
            }
            if (result != null)
            {
                moveTarget = result;
            }
            else
            {
                moveTarget = GameObject.FindWithTag("Manager");
            }
        }
    }
}
