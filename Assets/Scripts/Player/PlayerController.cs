using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Animator animator;
    private float centerY = 1.0f;
    private bool groundedPlayer;
    private bool canAttack = true;
    private bool isCurrentlyAttacking = false;
    private bool canSprint = true;
    private bool isSprinting = false;

    [Header("Movement")]
    private float playerSpeed = 2.0f;
    private float sprintSpeed = 4.0f;
    private float sprintDuration = 3.0f;
    private float sprintCooldown = 4.0f;
    private float jumpHeight = 2.0f;
    private float gravityValue = -9.81f;

    [Header("Unity Set-up")]
    public GameObject attackHitBoxReference;
    public bool hasAttackDuration = false;
    public Camera playerCamera;

    [Header("Player Stats")]
    public int attackDamage = 40;
    public float attackRate = 0.5f;
    public float attackRadius = 2f;
    public float knockbackForce = 10;
    public float attackDuration = 0.25f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        controller.center = new Vector3(0, centerY, 0);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();

        Cursor.lockState = CursorLockMode.Locked;

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

    void Movement()
    {
        //Falling
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
            animator.SetBool("Jump", false);
        }

        //Sprint Movement
        float currentSpeed = playerSpeed;
        if (canSprint && groundedPlayer && Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            if (canSprint)
            {
                StartCoroutine(SprintDuration());
            }
        }

        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        //Normal Movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = playerCamera.transform.TransformDirection(move);
        move.y = 0;
        controller.Move(move * Time.deltaTime * currentSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            //walk
            animator.SetBool("Walk", true);
        }
        else
        {
            //idle
            animator.SetBool("Walk", false);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            //jump
            animator.SetBool("Jump", true);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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

    IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = true;
    }

    IEnumerator SprintDuration()
    {
        yield return new WaitForSeconds(sprintDuration);
        isSprinting = false;
        StartCoroutine(SprintCooldown());
    }
}