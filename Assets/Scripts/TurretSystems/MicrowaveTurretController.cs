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
    private float fireCountdown = 0f;
    private Transform[] targets;
    private Transform target;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Customer";
    private float lightningPulseDurationModifier = 0.25f;
    //private GameObject lightningPulseParticles;
    public ParticleSystem onHitEffectPrefab;
    public Transform partToRotate;
    private AudioSource auraSound;
    

    // Start is called before the first frame update
    void Start() {
        //Initializes Turret Attributes based on global baselines
        initializeAttributes();

        //Initialize Lightning Pulse Particle Effect
        // lightningPulseParticles = transform.GetChild(0).GetComponent<GameObject>();
        // lightningPulseParticles.SetActive(false);

        //Called every 0.5 seconds
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        auraSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (fireCountdown <= 0f) {
            PulseDamage();
            fireCountdown = 1f/attackSpeed;
        }
        partToRotate.transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed);
        fireCountdown -= Time.deltaTime;
    }

    void initializeAttributes() {
        damage = GameController.instance.damage * damageMultiplier;
        radius = GameController.instance.range * rangeMultiplier;
        attackSpeed = GameController.instance.attackSpeed * attackSpeedMultiplier;
    }
    void PulseDamage() {
        // lightningPulseParticles.SetActive(true);
        // StartCoroutine(LightningPulseDuration());


        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius);

        bool playSound = false;
        foreach (Collider hitEnemy in hitEnemies)
        {
            if (hitEnemy.CompareTag("Customer"))
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
    
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
