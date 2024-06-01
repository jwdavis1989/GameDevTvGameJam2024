using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTurretControl : MonoBehaviour
{
    [Header("Attributes - 1.0 = 100%")]
    public float rangeMultiplier = 1f;
    public float tickDelayInSeconds = 3f;
    private float radius = 5f;

    [Header("Unity Setup Fields")]
    public string targetTag = "Turret";
    public string aoeTargetTag = "AoETurret";
    public ParticleSystem generatorBuffParticles;
    private float fireCountdown = 0f;

    // Start is called before the first frame update
    void Start() {
        //Initializes Turret Attributes based on global baselines
        initializeAttributes();
        generatorBuffParticles.Play();
        //ApplyBuff();
    }

    // Update is called once per frame
    void Update() {
        if (fireCountdown <= 0f) {
            ApplyBuff();
            fireCountdown = tickDelayInSeconds;
        }

        fireCountdown -= Time.deltaTime;
    }

    void initializeAttributes() {
        radius = GameController.instance.range * rangeMultiplier;
    }

    void ApplyBuff() {
        Collider[] affectedTowers = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider affectedTower in affectedTowers)
        {
            if (affectedTower.CompareTag(targetTag)) {
                //Apply buff to direct fire towers
                TurretController affectedController = affectedTower.GetComponent<TurretController>();
                affectedController.generatorBuffParticles.Play();
                affectedController.IsgeneratorBuffed = true;
            }
            else if (affectedTower.CompareTag(aoeTargetTag)) {
                //Apply buff to AoE towers
                MicrowaveTurretController affectedController = affectedTower.GetComponent<MicrowaveTurretController>();
                affectedController.generatorBuffParticles.Play();
                affectedController.IsgeneratorBuffed = true;
            }
        }
    }

    void RemoveBuff() {
        Collider[] affectedTowers = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider affectedTower in affectedTowers)
        {
            if (affectedTower.CompareTag(targetTag)) {
                //Apply buff to direct fire towers
                TurretController affectedController = affectedTower.GetComponent<TurretController>();
                affectedController.generatorBuffParticles.Stop();
                affectedController.IsgeneratorBuffed = false;
            }
            else if (affectedTower.CompareTag(aoeTargetTag)) {
                //Apply buff to AoE towers
                MicrowaveTurretController affectedController = affectedTower.GetComponent<MicrowaveTurretController>();
                affectedController.generatorBuffParticles.Stop();
                affectedController.IsgeneratorBuffed = false;
            }
        }
    }

    void Sell() {
        RemoveBuff();
        //TODO: Refund percentage of cost
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
