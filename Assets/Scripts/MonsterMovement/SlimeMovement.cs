using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public bool isDie;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 randomDirection;
    public float HP;
    public float speed = 2f;
    public float turnSpeed = 0.1f;
    public float maxVelocity = 4f; // 최대 속도 제한

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Ensure gravity is enabled
        animator = GetComponent<Animator>();
        HP = 1f;
        SetRandomDirection();
    }

    void Update()
    {
        if (isDie)
        {
            rb.velocity = Vector3.zero; // Stop movement if dead
            return; // Do nothing if the monster is dead
        }

        MoveRandomly();
    }

    private void SetRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        // Ensure the random direction is not zero
        while (randomDirection == Vector3.zero)
        {
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }
    }

    private void MoveRandomly()
    {
        Vector3 movement = randomDirection * speed * Time.fixedDeltaTime;

        if (!float.IsNaN(movement.x) && !float.IsNaN(movement.z)) // Only check necessary axes
        {
            rb.MovePosition(rb.position + movement);
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon")) 
        {
            TakeDamage(1f);
        }
    }

    private void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            isDie = true;
            animator.SetTrigger("isDie"); // Use SetTrigger for immediate state change
            StartCoroutine(DestroyAfterDelay(1.1f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
