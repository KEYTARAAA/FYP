using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStanding : MonoBehaviour
{
    public enum ANIMATION { REGULAR, HIP, ARMS}

    [SerializeField] ANIMATION state;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        switch (state)
        {
            case ANIMATION.HIP:
                animator.SetBool("Hip", true);
                break;
            case ANIMATION.ARMS:
                animator.SetBool("Arms", true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
