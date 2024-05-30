using UnityEngine;
using UnityEngine.UI;

public class TurretShop : MonoBehaviour
{
    BuildManager buildManager;
    public GameObject buildButtonPrefab;
    public TurretBluePrint[] turrets;
    public TurretBluePrint toasterTurret;
    public TurretBluePrint consoleTurret;
    public TurretBluePrint lawnMowerTurret;
    public TurretBluePrint cassetteTurret;
    public TurretBluePrint microwaveTurret;
    public TurretBluePrint generatorTurret;
    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
        populateTurretBuyButtons();
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

    public void populateTurretBuyButtons() {
        int i = 0;
        int index = 0;
        foreach (TurretBluePrint turretBluePrint in turrets) {
            //Instantiate Child Button
            GameObject currentButton = Instantiate(buildButtonPrefab, transform.position, Quaternion.identity);
            currentButton.transform.parent = transform;
            currentButton.GetComponent<Button>().onClick.AddListener( () => SelectTower(index++) );
            Debug.Log("Button Created with Index: " + i);
            
            //Set Button Fields
            //Hotkey
            currentButton.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = (i + 1).ToString();

            //Turret Name
            currentButton.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = turretBluePrint.name;

            //Cost
            currentButton.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = "$" + turretBluePrint.cost.ToString();

            i++;
        }
    }

    public void resetTurretBuyButtons() {
        //Remove all button children
        
    }

    // public void SelectToasterTurret() {
    //     Debug.Log("Toaster Turret Selected.");
    //     buildManager.SelectTurretToBuild(toasterTurret);
    // }

    // public void SelectConsoleTurret() {
    //     Debug.Log("Console Turret Selected.");
    //     buildManager.SelectTurretToBuild(consoleTurret);
    // }


    public void SelectTower(int index) {
        // turrets[0]
        // buildManager.SelectTurretToBuild(turrets[0])
        // transform.GetChild()currentButton.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text
        Debug.Log("Attempted Index: turrets[" + index + "]");
        buildManager.SelectTurretToBuild(turrets[index]);
    }
}
