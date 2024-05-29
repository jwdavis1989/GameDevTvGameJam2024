using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 35f;
    public float explosionRadius = 0f;

    [Header("Unity Setup")]
    public GameObject impactParticleEffect = null;
    private Transform target;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        if (direction.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    public void SetTarget(Transform _target) {
        target = _target;
    }

    public void HitTarget() {
        if (explosionRadius > 0f) {
            Explode();
        }
        else {
            Damage(target);
        }


        if (impactParticleEffect) {
            Instantiate(impactParticleEffect, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    public void Damage(Transform enemy) {
        enemy.GetComponent<CustomerController>().health -= damage;
    } 

    public void Explode() {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitObject in hitObjects) {
            if (hitObject.tag == "Customer") {
                Damage(hitObject.transform);
            }
        }
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
