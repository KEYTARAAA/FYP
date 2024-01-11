using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISitting : MonoBehaviour
{
    public enum ANIMATION {TALKING, TALKIG_MIRRORED, TALKING_LEG_CROSSED, TALKING_LEG_CROSSED_MIRRORED,
                    LYING, LEG_SHAKE, FLOOR, LEG_SWING,LEG_CROSSED, POINTING, REGULAR}

    [SerializeField] ANIMATION state;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        switch (state)
        {
            case ANIMATION.TALKING:
                animator.SetBool("Talking", true);
                break;
            case ANIMATION.TALKIG_MIRRORED:
                animator.SetBool("TalkingMirrored", true);
                break;
            case ANIMATION.TALKING_LEG_CROSSED:
                animator.SetBool("TalkingLegCrosed", true);
                break;
            case ANIMATION.TALKING_LEG_CROSSED_MIRRORED:
                animator.SetBool("TalkingLegCrosedMirrored", true);
                break;
            case ANIMATION.LYING:
                animator.SetBool("Lying", true);
                break;
            case ANIMATION.FLOOR:
                animator.SetBool("Floor", true);
                break;
            case ANIMATION.LEG_SHAKE:
                animator.SetBool("LegShake", true);
                break;
            case ANIMATION.LEG_CROSSED:
                animator.SetBool("LegCross", true);
                break;
            case ANIMATION.LEG_SWING:
                animator.SetBool("LegSwing", true);
                break;
            case ANIMATION.POINTING:
                animator.SetBool("Pointing", true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
