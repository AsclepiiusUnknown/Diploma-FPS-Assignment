using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutHandler : MonoBehaviour
{
    public LoadoutTypes objectLoadout;
}

[System.Serializable]
public enum LoadoutTypes
{
    Assault,
    Close,
    Ranged
}