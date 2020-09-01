using System;
using FPS;
using Rewired;
using UnityEngine;

public class OldWallRun : MonoBehaviour
{
    public LayerMask whatIsWall;
    public float checkDist = 1;
    public float speed;
    public float maxSpeed;
    public bool stickToWall = false;
    public FpsCustom _custom;

    private bool _isWallRight, _isWallLeft;
    private bool _isWallRunning = false;
    private CharacterController _controller;
    private Player _player;

    private void Start ()
    {
        _player = _custom._player;
        _controller = GetComponent<CharacterController> ();
    }

    private void Update ()
    {
        CheckForWall ();
        WallRunInput ();
    }

    private void WallRunInput ()
    {
        if ((_player.GetAxis ("Horizontal") > 0) && _isWallRight)
            StartWallRun ();
        if ((_player.GetAxis ("Horizontal") < 0) && _isWallLeft)
            StartWallRun ();
    }

    void StartWallRun ()
    {
        _custom._useGravity = false;
        _isWallRunning = true;

        if (_controller.velocity.magnitude <= maxSpeed)
        {
            _controller.Move (transform.forward * (speed * Time.deltaTime));

            if (stickToWall)
            {
                if (_isWallRight)
                    _controller.Move (transform.right * speed / 5 * Time.deltaTime);
                else
                    _controller.Move (-transform.right * speed / 5 * Time.deltaTime);
            }
        }
    }

    void StopWallRun ()
    {
        _isWallRunning = false;
        _custom._useGravity = true;
    }

    void CheckForWall ()
    {
        var position = transform.position;
        _isWallRight = Physics.Raycast (position, transform.right, checkDist, whatIsWall);
        _isWallLeft = Physics.Raycast (position, -transform.right, checkDist, whatIsWall);

        if (!_isWallLeft && !_isWallRight) StopWallRun ();
    }
}