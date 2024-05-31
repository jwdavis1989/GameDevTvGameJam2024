using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveTurretController : MonoBehaviour
{
[Header("Attributes")]
    public float damage = 10.0f;
    public float radius = 5f;
    public float attackSpeed = 2.0f;
    private float fireCountdown = 0f;
    private Transform[] targets;
    private Transform target;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Customer";
    public GameObject onHitEffectPrefab;
    private AudioSource auraSound;
    

    // Start is called before the first frame update
    void Start() {
        //Called every 0.5 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        auraSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        // if (!target) {
        //     return;
        // }

        if (fireCountdown <= 0f) {
            Shoot();
            fireCountdown = 1f/attackSpeed;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot() {
        GameObject onHitEffectGameObject = (GameObject)Instantiate(onHitEffectPrefab, target.position, target.rotation);
        BulletController bullet = onHitEffectGameObject.GetComponent<BulletController>();

        if (bullet) {
            bullet.SetTarget(target);
            bullet.damage = damage;
        }

        if (auraSound && GameController.instance.isTurretFireSoundOn) {
            auraSound.Play();
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

        if (nearestEnemey && shortestDistance <= radius) {
            target = nearestEnemey.transform;
        }
        else {
            target = null;
        }
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}