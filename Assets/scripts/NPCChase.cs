using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChase : MonoBehaviour
{

    private CharacterController characterController;
    private Animator animator;

    [SerializeField] private float movementSpeed, walkingSpeed, runningSpeed, rotationSpeed, jumpSpeed, gravity;

    private Vector3 movementDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
