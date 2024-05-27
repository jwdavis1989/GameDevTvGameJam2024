using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int currentHealth = 100;
    public int maxHealth = 100;
    public float despawnTime = 2f;
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        StartCoroutine(DespawnAfterDelay(despawnTime));
    }

    IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    
}
