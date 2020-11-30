using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FPS;

public class Target : MonoBehaviour
{
    [System.Serializable]
    public class DeathEvent : UnityEvent<Target> { }

    [HideInInspector]
    public int playerId;

    public float maxHealth = 100f;

    public DeathEvent onDeath = new DeathEvent();

    private float currentHealth;

    private void Awake()
    {
        if (GameManager.gameMode == GameModes.Duel)
            if (GetComponent<FpsCustomNetworked>() == FindObjectOfType<GM1v1>().player1.custom)
            {
                playerId = 1;
            }
            else if (GetComponent<FpsCustomNetworked>() == FindObjectOfType<GM1v1>().player1.custom)
            {
                playerId = 2;
            }
    }

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
        onDeath.Invoke(this);
    }
}