using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterMovement : MonoBehaviour
{
    public float bossHP = 100f;
    public bool isFind;
    public bool isClose;
    public bool isDie;

    private Rigidbody rb;
    private Animator wicherAnimator;
    private GameObject player;
    public float findDistance = 50f;
    public float closeDistance = 10f;
    public float normalSpeed = 1f;
    public float chaseSpeed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wicherAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player"); // Assuming the player has a tag "Player"
    }

    void FixedUpdate()
    {
        if (isDie)
        {
            return; // Do nothing if the boss is dead
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < findDistance)
            {
                isFind = true;
            }
            else
            {
                isFind = false;
            }

            if (distanceToPlayer < closeDistance)
            {
                isClose = true;
            }
            else
            {
                isClose = false;
            }

            // Update animator parameters
            wicherAnimator.SetBool("isFind", isFind);
            wicherAnimator.SetBool("isClose", isClose);

            if (isFind)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveRandomly();
            }
        }
    }

    private void MoveRandomly()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        Vector3 newPosition = transform.position + randomDirection * normalSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        if (randomDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(randomDirection, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.fixedDeltaTime));
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 newPosition = transform.position + directionToPlayer * chaseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.fixedDeltaTime));
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
        bossHP -= damage;
        if (bossHP <= 0)
        {
            isDie = true;
            wicherAnimator.SetTrigger("isDie");
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
