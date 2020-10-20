using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float maxHp = 10;
    private float hp;
    [HideInInspector]
    public bool isRespawning = false;

    void Start()
    {
        SetKinematic(true);
        hp = maxHp;
    }

    void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }

    public void Damage(float damage, bool _isHeadshot)
    {
        if (_isHeadshot)
        {
            StartCoroutine(Die());
            return;
        }

        if (hp <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            hp -= damage;
            print("dummy damaged");
        }
    }

    IEnumerator Die()
    {
        SetKinematic(false);
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
