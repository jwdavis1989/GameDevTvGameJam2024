using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public GameObject toasterTurretPrefab;
    public GameObject consoleTurretPrefab;
    public GameObject buildParticleEffect;
    private TurretBluePrint turretToBuild;

    void Awake() {
        if (instance) {
            return;
        }
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanBuild { get { return turretToBuild != null;} }
    public bool HasMoney { get { return GameController.money >= turretToBuild.cost;} }

    public void SelectTurretToBuild(TurretBluePrint turret) {
        turretToBuild = turret;
    }

    public void BuildTurretOn(NodeController node) {
        if (GameController.money >= turretToBuild.cost) {
            //Handle Turret Price
            GameController.money -= turretToBuild.cost;
            GameController.instance.UpdateMoneyTextDisplay();

            //Build the Turret
            GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity); 
            node.turretRef = turret;

            if (buildParticleEffect) {
                GameObject buildEffect = (GameObject)Instantiate(buildParticleEffect, node.GetBuildPosition(), Quaternion.identity);
                //Destroy the particle effect after x seconds
                Destroy(buildEffect, 3f);
            }
        }
        else {
            Debug.Log("Insufficient Money");
        }
    }
}
