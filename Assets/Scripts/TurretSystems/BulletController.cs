using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 35f;
    public GameObject impactParticleEffect = null;
    private Transform target;
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
    }

    public void SetTarget(Transform _target) {
        target = _target;
    }

    public void HitTarget() {
        //Damage the target once Customer class is established
        Debug.Log("Hit " + target + "!");
        
        if (impactParticleEffect) {
            Instantiate(impactParticleEffect, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
