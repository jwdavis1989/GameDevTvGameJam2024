using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AisleController : MonoBehaviour
{
    public int aisleNumber = 0;
    public bool isActive = true;
    public GameObject westMarker;
    public GameObject eastMarker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject getClosestSide(Vector3 position)
    {
        if(Vector3.Distance(position, westMarker.transform.position)
            > Vector3.Distance(position, eastMarker.transform.position))
        return eastMarker;
        else return westMarker;
    }
    public GameObject getFarthestSide(Vector3 position)
    {
        if (Vector3.Distance(position, westMarker.transform.position)
            > Vector3.Distance(position, eastMarker.transform.position))
            return westMarker;
        else return eastMarker;
    }
}
