using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRandomMovement : MonoBehaviour
{
    [SerializeField] Transform centers;
    NavMeshAgent nav;
    private Animator animator;


    [SerializeField] public float walkingSpeed, runningSpeed, runDistance, wanderRadius = 214, maxTime = 2.5f;
    Vector3 motionless = new Vector3(0, 0, 0), point;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        time = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.velocity.Equals(motionless) && time <= 0)
        {
            time = maxTime;
            int choice = Random.Range(0, centers.childCount);
            point = (centers.GetChild(choice).position + (Random.insideUnitSphere * wanderRadius));

            NavMeshHit dest;
            if (NavMesh.SamplePosition(point, out dest, 100, 1))
            {
                nav.SetDestination(dest.position);
            }
        }
        changeAnimation();
        time -= Time.deltaTime;
    }

    public void changeAnimation()
    {
        if (nav.isOnOffMeshLink)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        if (nav.velocity.Equals(motionless))
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            nav.speed = walkingSpeed;
            animator.SetBool("isWalking", true);
        }

        if (Vector3.Distance(nav.destination, nav.transform.position) > runDistance)
        {
            nav.speed = runningSpeed;
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }



    }

    private void OnDrawGizmos()
    {/*
        if (enabled)
        {
            for (int i = 0; i < centers.childCount; i++)
            {
                Gizmos.DrawSphere(centers.GetChild(i).position, wanderRadius);
            }
            Gizmos.DrawSphere(nav.destination, 5);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point, 15);

        }
        */
    }
}
