using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurretShop : MonoBehaviour
{
    BuildManager buildManager;
    public GameObject buildButtonPrefab;
    public TurretBluePrint[] turrets;
    public List<TurretBluePrint> currentTurretsList = new List<TurretBluePrint>();
    public TurretBluePrint toasterTurret;
    public TurretBluePrint consoleTurret;
    public TurretBluePrint lawnMowerTurret;
    public TurretBluePrint cassetteTurret;
    public TurretBluePrint microwaveTurret;
    public TurretBluePrint generatorTurret;
    private int verifiedTowers = 0;
    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
        //populateTurretBuyButtons();
        resetTowerButtons();
        verifyTowerUnlocks();
    }

    // Update is called once per frame
    void Update()
    {

        checkTowerHotkeys();
    }

    public void SelectToasterTurret()
    {
        Debug.Log("Toaster Turret Selected.");
        buildManager.SelectTurretToBuild(toasterTurret);
    }

    public void SelectConsoleTurret()
    {
        Debug.Log("Console Turret Selected.");
        buildManager.SelectTurretToBuild(consoleTurret);
    }

    public void SelectLawnmowerTurret()
    {
        Debug.Log("Lawnmower Turret Selected.");
        buildManager.SelectTurretToBuild(lawnMowerTurret);
    }

    public void resetTowerButtons() {
        int i = 0;
        foreach (TurretBluePrint turretBluePrint in turrets) {
            transform.GetChild(i).gameObject.SetActive(false);
            i++;
        }
        currentTurretsList.Clear();
    }

    public void verifyTowerUnlocks() {
        int i = 0;
        verifiedTowers = 0;
        foreach (TurretBluePrint turretBluePrint in turrets) {
            if (turretBluePrint.isUnlocked) {
                //currentTurretsList[verifiedTowers] = turrets[i];
                currentTurretsList.Add(turrets[i]);
                verifiedTowers++;
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = (verifiedTowers).ToString();
            }
            i++;
        }
    }

    public void checkTowerHotkeys() {
        int i = 0;
        foreach (TurretBluePrint turretBluePrint in currentTurretsList) {
            if (Input.GetKeyDown((i + 1).ToString())) {
                Debug.Log("Turret Selected: " + i);
                buildManager.SelectTurretToBuild(currentTurretsList[i]);
            }
            i++;
        }
    }
}
