using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int teamID;
    public bool isWeaponLocked = false;
    public bool isDroppable = false;

    public GameObject worldWeaponGameObject;
    public Vector3 ogLocation;

    public void SetUp(int teamID, GameObject worldGameObject, Vector3 ogLocation)
    {
        this.teamID = teamID;
        if (worldGameObject != null)
            worldWeaponGameObject = worldGameObject;
    }

    public void DropWeapon(Rigidbody player, Vector3 dropLocation)
    {
        Vector3 dirToDrop = dropLocation - Camera.main.transform.position;

        Ray rayToDropLocation = new Ray(Camera.main.transform.position, dirToDrop);
        RaycastHit hit;

        if (Physics.Raycast(rayToDropLocation, out hit, dirToDrop.magnitude))
        {
            dropLocation = hit.point;
        }

        worldWeaponGameObject.transform.position = dropLocation;

        Renderer rend = worldWeaponGameObject.GetComponent<Renderer>();
    }
}
