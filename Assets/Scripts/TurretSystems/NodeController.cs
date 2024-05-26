using UnityEngine;
using UnityEngine.EventSystems;

public class NodeController : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 buildPositionOffset = new Vector3(0f, 0.05f, 0f);
    private Color baseColor;
    private Renderer rendererRef;
    private GameObject turretRef;
    private BuildManager buildManager;
    // Start is called before the first frame update
    void Start()
    {
        rendererRef = GetComponent<Renderer>();
        baseColor = rendererRef.material.color;
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if (buildManager.GetTurretToBuild() && !EventSystem.current.IsPointerOverGameObject()) {
            if (turretRef) {
                Debug.Log("Can't Build There! TODO: Display on Screen");
                return;
            }
            //Debug.Log(BuildManager.instance);
            Debug.Log(buildManager.GetTurretToBuild());
            //Build a Turret
            GameObject turretToBuild = buildManager.GetTurretToBuild();
            turretRef = (GameObject)Instantiate(turretToBuild, transform.position + buildPositionOffset, transform.rotation);            
        }
    }

    void OnMouseEnter() {
        if (buildManager.GetTurretToBuild() && !EventSystem.current.IsPointerOverGameObject()) {
            rendererRef.material.color = hoverColor;
        }
    }

    void OnMouseExit() {
        rendererRef.material.color = baseColor;
    }
}
