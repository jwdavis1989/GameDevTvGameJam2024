using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AisleMarker : MonoBehaviour
{
    //public bool isLeft;
    public int aisleNumber;
    //public bool isActive;
    public GameObject aisle; 
    //private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        //set invisible
        Renderer renderer = this.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.a = 0.0f;
        renderer.material.color = color;

        //GameObject.Find("GameController").GetComponent<GameController>();
        //add self to aisles
        //gameController.aisles.Add(gameObject);
    }
    public bool isActive()
    {
        return aisle != null && aisle.GetComponent<AisleController>().isActive;
    }
    //public bool isRight()
    //{
    //    return !isLeft;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
