using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathMovement : MonoBehaviour
{

    [SerializeField] Transform targets;
    Transform target;
    NavMeshAgent nav;
    private Animator animator;
    Vector3 motionless = new Vector3(0,0,0);
    int index = 0;

    [SerializeField] public float walkingSpeed, runningSpeed, runDistance;



    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = targets.GetChild(0);
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            changeTarget();
        }

        if (Vector3.Distance(target.position, nav.gameObject.transform.position) > runDistance)
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

    void changeTarget()
    {
        Debug.Log("CHANGING " + index);
        index++;
        if (index >= targets.childCount)
        {
            index = 0;
        }
        target = targets.GetChild(index);
        nav.SetDestination(target.position);
    }
}
