using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float maxHp = 10;
    private float hp;

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
            Die();
            return;
        }

        if (hp <= 0)
        {
            Die();
        }
        else
        {
            hp -= damage;
            print("dummy damaged");
        }
    }

    void Die()
    {
        SetKinematic(false);
        Destroy(gameObject, 5);
    }
}
