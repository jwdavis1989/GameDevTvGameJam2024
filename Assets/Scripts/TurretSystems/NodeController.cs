using UnityEngine;
using UnityEngine.EventSystems;

public class NodeController : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Vector3 buildPositionOffset = new Vector3(0f, 0.05f, 0f);
    private Color baseColor;
    private Renderer rendererRef;
    [Header("Keep Empty!")]
    public GameObject turretRef;
    private BuildManager buildManager;
    // Start is called before the first frame update
    void Start()
    {
        rendererRef = GetComponent<Renderer>();
        baseColor = rendererRef.material.color;
        buildManager = BuildManager.instance;
        // if (turretRef) {
        //     buildManager.BuildTurretOn(this);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if (buildManager.CanBuild && !EventSystem.current.IsPointerOverGameObject()) {
            if (turretRef) {
                Debug.Log("Can't Build There! TODO: Display on Screen");
                return;
            }
            //Build a Turret
            buildManager.BuildTurretOn(this);          
        }
    }

    void OnMouseEnter() {
        if (!EventSystem.current.IsPointerOverGameObject() && buildManager.CanBuild) {
            if (buildManager.HasMoney) {
                rendererRef.material.color = hoverColor;
            }
            else {
                rendererRef.material.color = notEnoughMoneyColor;
            }
        }
    }

    void OnMouseExit() {
        rendererRef.material.color = baseColor;
    }

    public Vector3 GetBuildPosition() {
        return transform.position + buildPositionOffset;
    }
}
