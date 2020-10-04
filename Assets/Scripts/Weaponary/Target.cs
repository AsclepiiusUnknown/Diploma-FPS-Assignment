using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float maxHealth = 100f;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        print("Health -" + _damage);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}