using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideAndSeekChase : MonoBehaviour
{
    enum MODE { PLAYER, LAST, RANDOM, NONE}

    [SerializeField] Transform player;
    NavMeshAgent nav;
    private Animator animator;
    GameObject aim = null, heard = null;
    Vector3 lastPos, randomPos;

    MODE mode = MODE.NONE;

    [SerializeField] public float walkingSpeed, runningSpeed, runDistance, randomAreaRadius, maxTime = .5f;
    [HideInInspector] public HideAndSeekVision vision;

    private float soundInterval, soundTimer;

    public int soundFrequency = 5;
    bool hasAim;
    Vector3 motionless = new Vector3(0, 0, 0), point;
    float time;


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        vision = GetComponent<HideAndSeekVision>();
        soundInterval =  soundFrequency;
        randomPos = transform.position;
        time = maxTime;
    }

    // Update is called once per frame
    void Update()
    {

        soundTimer -= Time.deltaTime;
        if (soundTimer < 0)
        {
            heard = null;
        }

        setAim();

        setWander();

        changeAnimation();
        time -= Time.deltaTime;
    }

    void setAim()
    {
        if (vision.Objects.Count > 0)
        {
            aim = vision.Objects[0];
            heard = null;
            mode = MODE.PLAYER;
        }
        else if (heard)
        {
            aim = heard;
            mode = MODE.PLAYER;
        }
        else
        {
            mode = MODE.RANDOM;
            aim = null;
        }
    }

    void setWander()
    {
        if (mode == MODE.PLAYER)
        {
            nav.SetDestination(aim.transform.position);
            lastPos = aim.transform.position;
            mode = MODE.LAST;
        }
        else
        {
            if (mode == MODE.LAST)
            {
                if (nav.destination == lastPos)
                {
                    mode = MODE.RANDOM;
                }
                else
                {
                    nav.SetDestination(lastPos);
                }
            }

            if(mode == MODE.RANDOM && nav.velocity.Equals(motionless) && time <= 0)
            {
                time = maxTime;
                point = (player.position + (Random.insideUnitSphere * randomAreaRadius));

                NavMeshHit dest;
                if (NavMesh.SamplePosition(point, out dest, randomAreaRadius, 1))
                {
                    nav.SetDestination(dest.position);
                }
            }
        }
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

    public void setHeard(GameObject heard)
    {
        this.heard = heard;
        soundTimer = soundInterval;
    }

    public bool isAimNull()
    {
        return hasAim;
    }
    
    private void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.DrawWireSphere(player.position, randomAreaRadius);
            Gizmos.DrawSphere(nav.destination, 5);

        }
    }

}
