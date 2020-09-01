using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;
using Rewired;

public class LadderClimb : MonoBehaviour
{
    public LayerMask whatIsWall;
    public float checkDist = 1;
    public float jumpSpeed;
    public float pushSpeed;
    public float maxJumpSpeed;
    public FpsCustom _custom;

    private bool _isWallFront;
    private bool _isWallJumping = false;
    private CharacterController _controller;
    private Player _player;
    private RaycastHit hit;

    private void Start()
    {
        _player = _custom._player;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckForWall();

        if (_player.GetButtonDown("Jump") && _isWallFront)
            StartWallJump();
    }

    void StartWallJump()
    {
        _custom._useGravity = false;
        _isWallJumping = true;

        // if (_controller.velocity.magnitude <= maxJumpSpeed)
        // {
        _controller.Move(transform.up * (jumpSpeed * Time.deltaTime));
        _controller.Move(hit.normal * (pushSpeed * Time.deltaTime));
        // }
    }

    void StopWallJump()
    {
        _isWallJumping = false;
        _custom._useGravity = true;
    }

    void CheckForWall()
    {
        var position = transform.position;
        _isWallFront = Physics.Raycast(position, transform.forward, out hit, checkDist, whatIsWall);

        if (!_isWallFront) StopWallJump();
    }
}