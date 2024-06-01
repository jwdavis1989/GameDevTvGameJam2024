using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Attributes - 1.0 = 100%")]
    public float damageMultiplier = 1f;
    public float rangeMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    private float damage = 50.0f;
    private float range = 5f;
    private float attackSpeed = 1.0f;
    private float generatorBuffMultiplier = 1.25f;
    public bool IsgeneratorBuffed = false;
    public float turnSpeed = 10f;
    private float fireCountdown = 0f;
    private Transform target;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Customer";
    public GameObject bulletPrefab;
    public ParticleSystem generatorBuffParticles;
    private AudioSource fireSound;
    public Transform firePoint;
    public Transform partToRotate;
    

    // Start is called before the first frame update
    void Start() {
        //Initializes Turret Attributes based on global baselines
        initializeAttributes();

        //Called every 0.5 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        fireSound = GetComponent<AudioSource>();
        generatorBuffParticles.Stop();
    }

    // Update is called once per frame
    void Update() {
        if (!target) {
            return;
        }

        //Target Lock-on
        Vector3 targetDirection = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f) {
            Shoot();
            if (!IsgeneratorBuffed) {
                fireCountdown = 1f/attackSpeed;
            }
            else {
                fireCountdown = 1f/(attackSpeed * generatorBuffMultiplier);
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    void initializeAttributes() {
        damage = GameController.instance.damage * damageMultiplier;
        range = GameController.instance.range * rangeMultiplier;
        attackSpeed = GameController.instance.attackSpeed * attackSpeedMultiplier;
        generatorBuffMultiplier = GameController.instance.generatorBuffMultiplier;
    }
    void Shoot() {
        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletController bullet = bulletGameObject.GetComponent<BulletController>();

        if (bullet) {
            bullet.SetTarget(target);
            bullet.damage = damage;
        }

        if (fireSound && GameController.instance.isTurretFireSoundOn) {
            fireSound.Play();
        }
    }
    
    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemey = null;
        foreach(GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemey = enemy;
            }
        }

        //Handle Generator range buff
        float adjustedRange = range;
        if (IsgeneratorBuffed) {
            adjustedRange *= generatorBuffMultiplier;
        }

        if (nearestEnemey && shortestDistance <= adjustedRange) {
            target = nearestEnemey.transform;
        }
        else {
            target = null;
        }
    }

    void Sell() {
        //TODO: Refund percentage of cost
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
