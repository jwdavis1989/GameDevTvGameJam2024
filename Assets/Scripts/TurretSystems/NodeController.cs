using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 buildPositionOffset = new Vector3(0f, 0.05f, 0f);
    private Color baseColor;
    private Renderer rendererRef;
    private GameObject turretRef;
    // Start is called before the first frame update
    void Start()
    {
        rendererRef = GetComponent<Renderer>();
        baseColor = rendererRef.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if (turretRef) {
            Debug.Log("Can't Build There! TODO: Display on Screen");
            return;
        }
        //Debug.Log(BuildManager.instance);
        Debug.Log(BuildManager.instance.GetTurretToBuild());
        //Build a Turret
        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        turretRef = (GameObject)Instantiate(turretToBuild, transform.position + buildPositionOffset, transform.rotation);
    }

    void OnMouseEnter() {
        rendererRef.material.color = hoverColor;
    }

    void OnMouseExit() {
        rendererRef.material.color = baseColor;
    }
}
