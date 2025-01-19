using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireyEnemy : MonoBehaviour, IDamageable
{
    public LevelManager levelManager;
    public int health;
    [SerializeField]
    EnemyHealthBar healthBar;

    private void onDeath()
    {
        levelManager.EnemyDied();
        Destroy(gameObject);
    }

    void Start()
    {
        health = 100;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.UpdateHealthBar(health, 100);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("lebenspunkte des gegners: " + health);
        healthBar.UpdateHealthBar(health, 100);
        if (health <= 0)
        {
            onDeath();
        }
    }
}
