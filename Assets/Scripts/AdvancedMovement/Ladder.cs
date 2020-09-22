using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;

public class Ladder : MonoBehaviour
{
    GameObject playerOBJ;
    bool canClimb = false;
    public float speed = 1;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<FpsCustom>() != null)
            {
                FpsCustom _custom = other.gameObject.GetComponent<FpsCustom>();
            }

            canClimb = true;
            playerOBJ = other.gameObject;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            canClimb = false;
            playerOBJ = null;
        }
    }
    void Update()
    {
        if (canClimb)
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerOBJ.transform.Translate(Vector3.up * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerOBJ.transform.Translate(Vector3.down * Time.deltaTime * speed);
            }
        }
    }
}
