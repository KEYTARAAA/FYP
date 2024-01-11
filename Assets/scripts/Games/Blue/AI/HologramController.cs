using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HologramController : MonoBehaviour
{

    private Transform target;
    NavMeshAgent nav;
    private Animator animator;

    [SerializeField]
    public float walkingSpeed, runningSpeed, gravity;

    [SerializeField]
    private bool countNodesOnPath = false;

    [SerializeField]
    Slider slider;

    private List<Transform> nodes = null;

    private MazeCell[,] cells = null;
    List<int> searchOrder = null;
    int current = 0;
    Vector3 motionless = new Vector3(0, 0, 0);
    float time, maxTime = .5f;


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = nodes[current];
        current++;
        time = maxTime;
    }

    private void OnEnable()
    {
        nodes = GetComponentInParent<MakeMaze>().getNodes();
        cells = GetComponentInParent<MakeMaze>().getGrid();
        AStar aStar = new AStar(cells);
        searchOrder = aStar.getPath();
        slider.value = 0;
        current = 0;
        target = nodes[searchOrder[current]];
        if (countNodesOnPath)
        {
            Debug.Log(searchOrder.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.velocity.Equals(motionless) && time <= 0)
        {
            time = maxTime;
            if (current < searchOrder.Count)
            {
                //Debug.Log("current -> "+ current +  " cell -> " + searchOrder[current]);
                target = nodes[ searchOrder[ current ] ];
                slider.value = ((float)current / searchOrder.Count);
                current++;
            }
        }

        time -= Time.deltaTime;

        nav.SetDestination(target.position);
        if (nav.isOnOffMeshLink)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

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
