using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public GameObject toasterTurretPrefab;
    public GameObject consoleTurretPrefab;
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

    public void SelectTurretToBuild(TurretBluePrint turret) {
        turretToBuild = turret;
    }

    public void BuildTurretOn(NodeController node) {
        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity); 
        node.turretRef = turret;
    }
}
