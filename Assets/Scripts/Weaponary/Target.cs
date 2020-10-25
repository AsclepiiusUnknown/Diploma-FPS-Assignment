using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FPS;
using FPS.GameModes;

public class Target : MonoBehaviour
{
    [System.Serializable]
    public class DeathEvent : UnityEvent<Target> { }

    public float maxHealth = 100f;
    float currentHealth;

    public DeathEvent onDeath = new DeathEvent();


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        // print("Health -" + _damage);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        onDeath.Invoke(this);
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        currentHealth = maxHealth;
    }
}