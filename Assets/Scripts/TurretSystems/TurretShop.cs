using UnityEngine;

public class TurretShop : MonoBehaviour
{
    BuildManager buildManager;
    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("Basic Turret Selected.");
            buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
        }

        if (Input.GetKeyDown("2"))
        {
            Debug.Log("Another Turret Selected.");
            buildManager.SetTurretToBuild(buildManager.anotherTurretPrefab);
        }
    }

    public void PurchaseBasicTurret() {
        Debug.Log("Basic Turret Selected.");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }

    public void PurchaseAnotherTurret() {
        Debug.Log("Another Turret Selected.");
        buildManager.SetTurretToBuild(buildManager.anotherTurretPrefab);
    }
}
