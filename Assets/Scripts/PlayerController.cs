using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 2.0f;
    private float gravityValue = -9.81f;
    private bool canAttack = true;
    private bool isCurrentlyAttacking = false;

    [Header("Unity Set-up")]
    public GameObject attackHitBoxReference;
    public bool hasAttackDuration = false;

    [Header("Player Stats")]
    public int attackDamage = 40;
    public float attackRate = 0.5f;
    public float attackRadius = 2f;
    public float knockbackForce = 10;
    public float attackDuration = 0.25f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        //Falling
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        //Normal Movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Attacking
        if (canAttack && Input.GetMouseButtonDown(0))
        {
            canAttack = false;
            isCurrentlyAttacking = true;
            StartCoroutine(AttackCooldown());
            if (hasAttackDuration)
            {
                StartCoroutine(AttackDuration());
            }
            Attack();
        }

        if (isCurrentlyAttacking && hasAttackDuration)
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackHitBoxReference.transform.position, attackRadius);
        foreach (Collider hitEnemy in hitEnemies)
        {
            if (hitEnemy.CompareTag("Customer"))
            {
                //Damage the Enemy
                Damage(hitEnemy.gameObject);

                //Knockback
                Knockback(hitEnemy);
            }
        }
    }

    public void Damage(GameObject enemy)
    {
        enemy.GetComponent<CustomerController>().health -= attackDamage;
    }

    void Knockback(Collider enemy)
    {
        Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
        Vector3 knockbackDirection = enemy.transform.position - transform.position;
        knockbackDirection.y = 0;
        knockbackDirection.Normalize();
        enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(attackDuration);
        isCurrentlyAttacking = false;
    }
}