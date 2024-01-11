using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GreenMazeAIAhead : MonoBehaviour
{

    public Transform target;
    NavMeshAgent nav;
    private Animator animator;

    [SerializeField] public float walkingSpeed, runningSpeed, gravity, ahead=5;



    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.isOnOffMeshLink)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        nav.SetDestination(target.position+(target.forward*ahead));

        if (Vector3.Distance(target.position, nav.gameObject.transform.position) > 2)
        {
            nav.speed = runningSpeed;
            animator.SetBool("isRunning", true);
        }
        else
        {
            nav.speed = walkingSpeed;
            animator.SetBool("isRunning", false);
        }

        if ((nav.velocity.x == 0) && (nav.velocity.y == 0) && (nav.velocity.z == 0))
        {
            nav.speed = walkingSpeed;
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }
}
