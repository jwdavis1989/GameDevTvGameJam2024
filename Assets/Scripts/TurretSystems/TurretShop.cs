using UnityEngine;

public class TurretShop : MonoBehaviour
{
    BuildManager buildManager;
    public TurretBluePrint toasterTurret;
    public TurretBluePrint consoleTurret;
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
            Debug.Log("Toaster Turret Selected.");
            buildManager.SelectTurretToBuild(toasterTurret);
        }

        if (Input.GetKeyDown("2"))
        {
            Debug.Log("Console Turret Selected.");
            buildManager.SelectTurretToBuild(consoleTurret);
        }
    }

    public void SelectToasterTurret() {
        Debug.Log("Toaster Turret Selected.");
        buildManager.SelectTurretToBuild(toasterTurret);
    }

    public void SelectConsoleTurret() {
        Debug.Log("Console Turret Selected.");
        buildManager.SelectTurretToBuild(consoleTurret);
    }
}
