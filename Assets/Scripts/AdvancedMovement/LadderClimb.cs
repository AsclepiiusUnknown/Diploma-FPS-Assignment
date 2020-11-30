using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;
using Rewired;

[RequireComponent(typeof(CharacterController), typeof(FpsCustom))]
public class LadderClimb : MonoBehaviour
{
    public LayerMask whatIsLadder;
    public float checkDist = 1;
    public float climbSpeed;
    public float pushSpeed;
    public float maxClimbSpeed;

    private bool isLadderFront;
    private bool isClimbing = false;
    private CharacterController controller;
    private FpsCustomNetworked custom;
    private Player player;
    private RaycastHit hit;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        custom = GetComponent<FpsCustomNetworked>();
    }

    private void Start()
    {
        player = custom._player;
    }

    private void Update()
    {
        CheckFront();

        if (!controller.isGrounded && player.GetButtonDown("Jump") && isClimbing)
            PushOffWall();

        if (isLadderFront && !isClimbing)
            StartClimb();
        else if (!isLadderFront && isClimbing)
            StopClimb();
    }

    void StartClimb()
    {
        custom._useGravity = false;
        isClimbing = true;

        if (controller.velocity.magnitude <= maxClimbSpeed)
        {
            controller.Move(transform.up * (climbSpeed * Time.deltaTime));
        }
    }

    void StopClimb()
    {
        print("stopping");
        isClimbing = false;
        custom._useGravity = true;
    }

    void CheckFront()
    {
        Vector3 _pos = transform.position;
        isLadderFront = Physics.Raycast(_pos, transform.forward, out hit, checkDist, whatIsLadder);

        if (isLadderFront) print("ladder in front");
    }

    void PushOffWall()
    {
        controller.Move(hit.normal * (pushSpeed * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * checkDist);
    }
}