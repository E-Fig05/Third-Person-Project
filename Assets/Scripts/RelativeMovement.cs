using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float jumpSpeed = 15.0f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float terminalVelocity = -10.0f;
    [SerializeField] private Transform target;
    private CharacterController charController;
    private float minFall = -1.5f;
    private float vertSpeed;
    private ControllerColliderHit contact;
    private Animator animator;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        vertSpeed = minFall;
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 Movement = Vector3.zero;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 right = target.right;

            Vector3 forward = Vector3.Cross(right, Vector3.up);

            Movement = (right * horizontalInput) + (forward * verticalInput);
            Movement *= moveSpeed;
            Movement = Vector3.ClampMagnitude(Movement, moveSpeed);

            transform.rotation = Quaternion.LookRotation(Movement);
        }

        animator.SetFloat("Speed", Movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;
        if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (charController.height + charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }
        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
                animator.SetBool("Jumping", false);
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
            if (contact != null)
            {
                animator.SetBool("Jumping", true);
            }
        }

        //animator.SetFloat("Speed", Movement.sqrMagnitude);

        /*if(charController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if(vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }*/
        Movement.y = vertSpeed;
        Movement *= Time.deltaTime;
        charController.Move(Movement);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;
    }

}
