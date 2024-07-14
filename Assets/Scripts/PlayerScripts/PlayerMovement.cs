using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpForce;
    private Rigidbody rb;
    private bool isJumping;
    private bool isRunning;
    // public int itemCount;
    public float turnSpeed = 0.5f;
    private Vector3 targetDirection;
    private Vector2 input;
    private Quaternion targetRotation;
    public Animator playerAnimator;
    //public GameObject bullet;
    //public GameObject firePosition;
    //public float coolTime;


    void Awake()
    {
        walkingSpeed = 0.10f;
        runningSpeed = 0.15f;
        speed = walkingSpeed;
        jumpForce = 3f;
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        UpdateTargetDirection();

        if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
        {
            Vector3 lookDerection = targetDirection.normalized;

            targetRotation = Quaternion.LookRotation(lookDerection, transform.up);

            var differceRotation = targetRotation.eulerAngles.y - transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;

            if(differceRotation < 0 || differceRotation > 0)
            {
                eulerY = targetRotation.eulerAngles.y;
            }
            var newForward = new Vector3(0, eulerY, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newForward), turnSpeed);
        }

        if (input.x == 0 && input.y == 0)
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", true);
        }

        rb.AddForce(targetDirection * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            playerAnimator.SetTrigger("IsJumping");
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = runningSpeed;
            isRunning = true;
            playerAnimator.SetBool("IsRunning", true);
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = walkingSpeed;
            isRunning = false;
            playerAnimator.SetBool("IsRunning", false);
        }
        if (Input.GetMouseButton(0) ) 
        {
            playerAnimator.SetTrigger("IsPunching");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Floor"){
            isJumping = false;
        }
    }
    

    public void UpdateTargetDirection()
    {
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward); //TransformDirection은 단위 벡터를 구하는 식
        forward.y = 0;

        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

        targetDirection = input.x * right + input.y * forward;
    }

}
