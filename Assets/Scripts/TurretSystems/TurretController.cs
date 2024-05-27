using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Attributes")]
    public float damage = 5.0f;
    public float range = 5f;
    public float attackSpeed = 1.0f;
    public float turnSpeed = 10f;
    private float fireCountdown = 0f;
    private Transform target;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Customer";
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform partToRotate;
    

    // Start is called before the first frame update
    void Start() {
        //Called every 0.5 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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
            fireCountdown = 1f/attackSpeed;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot() {
        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletController bullet = bulletGameObject.GetComponent<BulletController>();

        if (bullet) {
            bullet.SetTarget(target);
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

        if (nearestEnemey && shortestDistance <= range) {
            target = nearestEnemey.transform;
        }
        else {
            target = null;
        }
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
