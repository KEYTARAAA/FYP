using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private Animator animator;
    private bool groundedJump = false;


    [SerializeField] private float movementSpeed, walkingSpeed, runningSpeed, rotationSpeed, jumpSpeed, gravity, jumpHeight;
    [SerializeField] private Transform player;

    private Vector3 movementDirection = Vector3.zero, cameraOriginP, cameraOriginR;
    private float initialHeigth = 0;
    private bool land = false, active = true;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraOriginP = player.GetComponentInChildren<Camera>().transform.localPosition;
        cameraOriginR = player.GetComponentInChildren<Camera>().transform.localEulerAngles;
    }




    // Update is called once per frame
    void Update()
    {
        if (active) {
            groundedJump = (Input.GetButton("Jump") && characterController.isGrounded);
            Vector3 inputMovement = transform.forward * movementSpeed * Input.GetAxisRaw("Vertical");
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

            if (Input.GetKeyDown(KeyCode.L))
            {
                /*Transform camera = player.transform.Find("Student").transform.Find("Main Camera");
                camera.localRotation = Quaternion.Euler(15.695f, 180, 0); ;
                camera.localPosition = new Vector3(0, 1.2f, 1.56f);*/

                player.GetComponentInChildren<Camera>().transform.localPosition = new Vector3(cameraOriginP.x, cameraOriginP.y, -cameraOriginP.z);
                player.GetComponentInChildren<Camera>().transform.localEulerAngles = new Vector3(cameraOriginR.x, (cameraOriginR.y+180), cameraOriginR.z);
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {
                /*Transform camera = player.transform.Find("Student").transform.Find("Main Camera");
                camera.localRotation = Quaternion.Euler(15.695f, 0, 0); ;
                camera.localPosition = new Vector3(0, 1.411f, -1.18f);*/
                player.GetComponentInChildren<Camera>().transform.localPosition = cameraOriginP;
                player.GetComponentInChildren<Camera>().transform.localEulerAngles = cameraOriginR;
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

