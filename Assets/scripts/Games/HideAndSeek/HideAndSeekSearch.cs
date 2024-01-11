using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideAndSeekSearch : MonoBehaviour
{
    [SerializeField] Transform player;
    NavMeshAgent nav;
    Animator animator;
    HideAndSeekChase chase;
    bool searching;
    Vector3 motionless = new Vector3(0, 0, 0);
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        chase = GetComponent<HideAndSeekChase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chase.isAimNull())
        {
            Debug.Log("MOTION?: " + nav.velocity.Equals(motionless));
            if (nav.velocity.Equals(motionless)) 
            {
                nav.SetDestination(new Vector3(Random.Range(player.position.x - 40, player.position.x + 40),
                                                            player.position.y,
                                               Random.Range(player.position.z - 40, player.position.z + 40)));
                Debug.Log(nav.destination);
            }
            chase.changeAnimation();
        }
    }
}
