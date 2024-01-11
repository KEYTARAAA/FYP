using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class GreenMazePlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private Animator animator;
    private bool groundedJump = false;


    [SerializeField] private float movementSpeed, walkingSpeed, runningSpeed, rotationSpeed, jumpSpeed, gravity, jumpHeight;
    [SerializeField] private Transform player, camera;
    private Vector3 cameraRotation;

    private Vector3 movementDirection = Vector3.zero;
    private float initialHeigth = 0;
    private bool land = false, active = true;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraRotation = new Vector3(camera.rotation.x, camera.rotation.y, camera.rotation.z); ;
    }




    // Update is called once per frame
    void Update()
    {
        if (active) {
            groundedJump = (Input.GetButton("Jump") && characterController.isGrounded);
            Vector3 inputMovement = (camera.forward ) * movementSpeed * Input.GetAxisRaw("Vertical");
            characterController.Move(inputMovement * Time.deltaTime);

            transform.Rotate(Vector3.up * Input.GetAxisRaw("Horizontal") * rotationSpeed);

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
            animator.SetBool("isWalking", (Input.GetAxisRaw("Vertical") != 0));
            animator.SetBool("isJumping", !characterController.isGrounded);

        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
    }
}

