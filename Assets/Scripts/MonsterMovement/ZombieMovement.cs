using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float wacher1HP;
    public bool isFind;
    public bool isClose;
    public bool isDie;

    private Rigidbody rb;
    private Animator wacherAnimator;
    private GameObject player;
    public float findDistance = 10f;
    public float attackDistance = 1f;
    public float normalSpeed = 1f;
    public float chaseSpeed = 3f;

    private Vector3 targetDirection;
    private Vector3 randomDirection;
    private Quaternion targetRotation;
    public float turnSpeed = 0.1f;
    public float maxVelocity = 10f; // 최대 속도 제한

    void Awake()
    {
        wacher1HP = 1f;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Ensure gravity is enabled
        wacherAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player"); // Assuming the player has a tag "Player"
        SetRandomDirection();
    }

    void FixedUpdate()
    {
        if (isDie)
        {
            rb.velocity = Vector3.zero; // Stop movement if dead
            return; // Do nothing if the monster is dead
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < findDistance)
            {
                isFind = true;

                if (distanceToPlayer < attackDistance)
                {
                    isClose = true;
                }
                else
                {
                    isClose = false;
                }
            }
            else
            {
                isFind = false;
                isClose = false;
            }

            // Update animator parameters
            wacherAnimator.SetBool("isFind", isFind);
            wacherAnimator.SetBool("isClose", isClose);

            if (isFind)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveRandomly();
            }
        }

        // 속도 제한 적용
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        // 무한대 값 체크
        if (float.IsInfinity(rb.velocity.x) || float.IsInfinity(rb.velocity.y) || float.IsInfinity(rb.velocity.z))
        {
            Debug.LogError("Velocity has become infinite.");
            rb.velocity = Vector3.zero;
        }
    }

    private void SetRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void MoveRandomly()
    {
        if (randomDirection == Vector3.zero)
        {
            SetRandomDirection();
        }

        Vector3 movement = randomDirection * normalSpeed;

        if (!float.IsNaN(movement.x) && !float.IsNaN(movement.z)) // Only check necessary axes
        {
            rb.AddForce(movement, ForceMode.Impulse);
        }

        if (randomDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(randomDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed);
        }

        if (Random.Range(0f, 1f) < 0.01f) // Change direction occasionally
        {
            SetRandomDirection();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 movement = directionToPlayer * chaseSpeed;

        if (!float.IsNaN(movement.x) && !float.IsNaN(movement.z)) // Only check necessary axes
        {
            rb.AddForce(movement, ForceMode.Impulse);
        }

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon")) // Assuming the player's weapon has a tag "PlayerWeapon"
        {
            TakeDamage(1f);
        }
    }

    private void TakeDamage(float damage)
    {
        wacher1HP -= damage;
        if (wacher1HP <= 0)
        {
            isDie = true;
            wacherAnimator.SetTrigger("isDie"); // Use SetTrigger for immediate state change
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
