using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerControllerFake : MonoBehaviour
{

    private CharacterController characterController;
    private Animator animator;
    private bool preJumpLoaded = false;
    private bool jumpLanded = true;
    private bool groundedJump = false;


    [SerializeField] private float movementSpeed, walkingSpeed, runningSpeed, rotationSpeed, jumpSpeed, maxJump, gravity;

    private Vector3 movementDirection = Vector3.zero, jumpOrigin;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void loadedJump()
    {
        preJumpLoaded = true;
    }
    void landedJump()
    {
        jumpLanded = true;
    }




    // Update is called once per frame
    void Update()
    {
        
        groundedJump = (Input.GetButton("Jump") && characterController.isGrounded);
        Vector3 inputMovement = transform.forward * movementSpeed * Input.GetAxisRaw("Vertical");
        characterController.Move(inputMovement * Time.deltaTime);

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Horizontal") * rotationSpeed);

        if (groundedJump)
        {

            animator.SetBool("preJump", true);
            jumpOrigin = transform.position;
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


        if (preJumpLoaded)
        {
            if (transform.position.y < jumpOrigin.y + maxJump)
            {

                movementDirection.y = jumpSpeed;
            }
            preJumpLoaded = false;
            animator.SetBool("preJump", false);
            animator.SetBool("jumpLand", false);
        }


        if (jumpLanded)
        {
            
            jumpLanded = false;
            animator.SetBool("jumpLand", true);
        }


        movementDirection.y -= gravity * Time.deltaTime;

        characterController.Move(movementDirection * Time.deltaTime);

        //animations
        animator.SetBool("isWalking", (Input.GetAxisRaw("Vertical") != 0));
        animator.SetBool("isJumping", !characterController.isGrounded);


    }
}

