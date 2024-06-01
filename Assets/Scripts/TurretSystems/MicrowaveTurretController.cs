using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveTurretController : MonoBehaviour
{
    [Header("Attributes - 1.0 = 100%")]
    public float damageMultiplier = 1f;
    public float rangeMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public float turnSpeed = 10f;
    private float damage = 10.0f;
    private float radius = 5f;
    private float attackSpeed = 2.0f;
    private float generatorBuffMultiplier = 1.25f;
    public bool IsgeneratorBuffed = false;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Customer";
    public ParticleSystem generatorBuffParticles;
    private float lightningPulseDurationModifier = 0.25f;
    //private GameObject lightningPulseParticles;
    public ParticleSystem onHitEffectPrefab;
    public Transform partToRotate;
    private AudioSource auraSound;
    

    // Start is called before the first frame update
    void Start() {
        //Initializes Turret Attributes based on global baselines
        initializeAttributes();
        auraSound = GetComponent<AudioSource>();
        generatorBuffParticles.Stop();
    }

    // Update is called once per frame
    void Update() {
        if (fireCountdown <= 0f) {
            PulseDamage();
            if (!IsgeneratorBuffed) {
                fireCountdown = 1f/attackSpeed;
            }
            else {
                fireCountdown = 1f/(attackSpeed * generatorBuffMultiplier);
            }
        }
        partToRotate.transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed);
        fireCountdown -= Time.deltaTime;
    }

    void initializeAttributes() {
        damage = GameController.instance.damage * damageMultiplier;
        radius = GameController.instance.range * rangeMultiplier;
        attackSpeed = GameController.instance.attackSpeed * attackSpeedMultiplier;
        generatorBuffMultiplier = GameController.instance.generatorBuffMultiplier;
    }
    void PulseDamage() {
        //Handle Generator range buff
        float adjustedRadius = radius;
        if (IsgeneratorBuffed) {
            adjustedRadius *= generatorBuffMultiplier;
        }

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, adjustedRadius);

        bool playSound = false;
        foreach (Collider hitEnemy in hitEnemies)
        {
            if (hitEnemy.CompareTag(enemyTag))
            {
                //Turn on sound if it hits a customer
                playSound = true;
                //Apply onHit Particle Effects
                //GameObject onHitEffectGameObject = (GameObject)Instantiate(onHitEffectPrefab, target.position, target.rotation);

                //Damage the Enemy
                Damage(hitEnemy.gameObject);
            }
        }
        //Play sound if target hit
        if (playSound && auraSound && GameController.instance.isTurretFireSoundOn) {
            auraSound.Play();
        }
    }

    void Damage(GameObject enemy) {
        enemy.GetComponent<CustomerController>().health -= damage;
    }
    
    // IEnumerator LightningPulseDuration()
    // {
    //     yield return new WaitForSeconds(attackSpeed * lightningPulseDurationModifier);
    //     lightningPulseParticles.SetActive(false);
    // }

    void Sell() {
        //TODO: Refund percentage of cost
        Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
