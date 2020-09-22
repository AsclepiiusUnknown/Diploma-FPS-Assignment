using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;
using Rewired;

[RequireComponent(typeof(CharacterController), typeof(FpsCustom))]
public class LadderClimb : MonoBehaviour
{
    public LayerMask whatIsWall;
    public float checkDist = 1;
    public float climbSpeed;
    public float pushSpeed;
    public float maxClimbSpeed;

    private bool _isLadderFront;
    private bool _isClimbing = false;
    private CharacterController _controller;
    private FpsCustom _custom;
    private Player _player;
    private RaycastHit hit;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _custom = GetComponent<FpsCustom>();
    }

    private void Start()
    {
        _player = _custom._player;
    }

    private void Update()
    {
        CheckFront();

        if (!_controller.isGrounded && _player.GetButtonDown("Jump") && _isClimbing)
            PushOffWall();

        if (_isLadderFront && !_isClimbing)
            StartClimb();
        else if (!_isLadderFront)
            StopClimb();
    }

    void StartClimb()
    {
        _custom._useGravity = false;
        _isClimbing = true;

        if (_controller.velocity.magnitude <= maxClimbSpeed)
        {
            _controller.Move(transform.up * (climbSpeed * Time.deltaTime));
        }
    }

    void StopClimb()
    {
        print("stopping");
        _isClimbing = false;
        _custom._useGravity = true;
    }

    void CheckFront()
    {
        Vector3 position = transform.position;
        _isLadderFront = Physics.Raycast(position, transform.forward, out hit, checkDist, whatIsWall);

        if (!_isLadderFront && _isClimbing) StopClimb();
    }

    void PushOffWall()
    {
        _controller.Move(hit.normal * (pushSpeed * Time.deltaTime));
    }
}