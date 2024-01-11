using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardinalMove : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private bool groundedJump = false;


    [SerializeField] private float movementSpeed, walkingSpeed, runningSpeed, jumpSpeed, gravity, jumpHeight;
    [SerializeField] private Transform player;
    private Vector3 cameraRotation;

    private Vector3 movementDirection = Vector3.zero;
    private float initialHeigth = 0;
    private bool land = false, active = true;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
        animator = GetComponentInParent<Animator>();
        cameraRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }




    // Update is called once per frame
    void Update()
    {
        if (active) {
            groundedJump = (Input.GetButton("Jump") && characterController.isGrounded);
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            //movementDirection = (transform.right * inputX) + (transform.forward * inputZ);
            characterController.Move( ( ( ( transform.right * inputX ) + ( transform.forward * inputZ ) ) * movementSpeed ) );
            //characterController.Move(move * Time.deltaTime);

            if (groundedJump)
            {
                animator.SetBool("isJumping", true);
                land = false;
                initialHeigth = player.transform.position.y;
            }
            else if (Input.GetKey(KeyCode.LeftShift) && (animator.GetBool("isWalking")))
            {
                animator.SetBool("isRunning", true);
                movementSpeed = runningSpeed;
            }
            else
            {
                animator.SetBool("isRunning", false);
                movementSpeed = walkingSpeed;
            }

            if (player.transform.position.y > initialHeigth + jumpHeight)
            {
                land = true;
            }

            if (animator.GetBool("isJumping") && !land)
            {
                movementDirection.y = jumpSpeed;
            }


            movementDirection.y -= gravity * Time.deltaTime;


            characterController.Move(movementDirection * Time.deltaTime);

            //animations
            animator.SetBool("isWalking", ((Input.GetAxisRaw("Vertical") != 0) || (Input.GetAxisRaw("Vertical") != 0)));
            animator.SetBool("isJumping", !characterController.isGrounded);


            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("isShooting", true);
                animator.SetBool("isWhipping", false);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                animator.SetBool("isWhipping", true);
                animator.SetBool("isShooting", false);
            }

            if ((animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1) && (animator.GetCurrentAnimatorStateInfo(1).IsName("Shooting")))
            {
                animator.SetBool("isShooting", false);
            }
            else if ((animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1) && (animator.GetCurrentAnimatorStateInfo(1).IsName("Whip")))
            {
                animator.SetBool("isWhipping", false);
            }
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWhipping", false);
    }
}

