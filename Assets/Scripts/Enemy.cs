using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public LevelManager levelManager;
    public int health;
    private void onDeath()
    {
        levelManager.EnemyDied();
        Destroy(gameObject);
    }
    
    void Start()
    {
        health = Random.Range(1, 100);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("lebenspunkte des gegners: " + health);
        if (health <= 0)
        {
            onDeath();
        }
    }
}
