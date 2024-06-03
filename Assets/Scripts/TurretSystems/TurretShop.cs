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
        unlockTowers();
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

    public void SelectCassettePlayerTurret()
    {
        Debug.Log("Cassette Player Turret Selected.");
        buildManager.SelectTurretToBuild(cassetteTurret);
    }

    public void SelectGeneratorTurret()
    {
        Debug.Log("Generator Turret Selected.");
        buildManager.SelectTurretToBuild(generatorTurret);
    }

    public void SelectMicrowaveTurret()
    {
        Debug.Log("Microwave Turret Selected.");
        buildManager.SelectTurretToBuild(microwaveTurret);
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
                if (verifiedTowers < 6) {
                    verifiedTowers++;
                }
                transform.GetChild(i).gameObject.SetActive(true);
                
                //Set Hotkey
                transform.GetChild(i).transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = (verifiedTowers).ToString();

                //Set Name
                transform.GetChild(i).transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = turrets[i].name;

                //Set Price
                transform.GetChild(i).transform.GetChild(6).GetComponent<TMPro.TextMeshProUGUI>().text = "$" + turrets[i].cost;
            }
            i++;
        }
    }

    public void unlockTowers() {
        if (GameController.instance.waveNumber < 6) {
            for (int i = 0; i < (GameController.instance.waveNumber + 1); i++) {
                turrets[i].isUnlocked = true;
            }
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
