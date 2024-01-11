using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GreenMazeAIFollowPath : MonoBehaviour, GreenMazePathAI
{
    //[SerializeField] Transform player
    List<Transform> path;
    NavMeshAgent nav;
    Animator animator;
    Transform target = null;
    int index = -1;

    [SerializeField] public float walkingSpeed, runningSpeed, gravity;


    public void setPath(List<Transform> path)
    {
        this.path = path;
    }

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (nav.isOnOffMeshLink)
            {
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }

            nav.SetDestination(target.position);

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
                changeTarget();
            }
            else
            {
                animator.SetBool("isWalking", true);
            }

        }
        else
        {
            changeTarget();
        }
    }

    void changeTarget()
    {
        index++;

        if (index >= path.Count)
        {
            index = 0;
        }

        target = path[index];
    }
}
