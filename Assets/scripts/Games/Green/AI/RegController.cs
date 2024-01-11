using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RegController : MonoBehaviour
{

    [SerializeField] Transform target;
    NavMeshAgent nav;
    private Animator animator;
    Vector3 targetVec, motionless = new Vector3(0,0,0);
    bool moving = true;

    [SerializeField] public float walkingSpeed, runningSpeed, runDistance;



    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            targetVec = target.position;
        }
        if (nav.isOnOffMeshLink)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        nav.SetDestination(targetVec);

        if (Vector3.Distance(targetVec, nav.gameObject.transform.position) > runDistance && (!nav.velocity.Equals(motionless)))
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

    public void setTarget(Vector3 targetVec)
    {
        this.targetVec = targetVec;
    }
    public void setMoving(bool moving)
    {
        this.moving = moving;
    }
}
