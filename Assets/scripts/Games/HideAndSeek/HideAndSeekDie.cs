using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HideAndSeekDie : MonoBehaviour
{
    [SerializeField] GameObject ui, sign, mainUI;
    [SerializeField] Transform player, winUI;
    Vector3 origin;
    NavMeshAgent agent;
    private Animator animator;
    Vector3 motionless = new Vector3(0, 0, 0);
    private void OnTriggerEnter(Collider other)
    {
        die(true);
    }

    private void Start()
    {
        origin = transform.position;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void die(bool loss)
    {
        if (loss && GetComponent<SphereCollider>().radius < 2f)
        {
            winUI.gameObject.SetActive(true);
            winUI.GetComponent<GameOutcome>().lose();
        }
        ui.GetComponent<HideAndSeekManager>().enabled = false;
        GetComponent<SphereCollider>().radius = 2f;
        GetComponent<HideAndSeekInteractable>().enabled = true;
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.activate();
        interacter.enabled = true;
        ui.SetActive(false);
        agent.SetDestination(origin);
        sign.SetActive(true);
        mainUI.SetActive(true);
    }

    private void Update()
    {

        if (agent.velocity.Equals(motionless))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
    }
}
