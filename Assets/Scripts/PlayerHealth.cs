using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    public int currentHealth;
    public event Action<int> OnHealthChange;
    public PlayerHealthBar playerHealthBar;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        LoadHealth();
        if (playerHealthBar == null)
        {
            playerHealthBar = GameObject.FindObjectOfType<PlayerHealthBar>();
            if (playerHealthBar == null)
            {
                Debug.LogError("playerhealthbar nicht gefunden in playerhealth");
            }
            else
            {
                playerHealthBar.UpdateHealthBar(currentHealth);
            }
        }
        
        if (levelManager == null)
        {
            levelManager = GameObject.FindObjectOfType<LevelManager>();
            if (levelManager == null)
            {
                Debug.LogError("levelmanager in playerhealth nicht gefunden");
            }
        }
    }

    public void UpdateHealthBar(int health)
    {
        currentHealth = health;
        if (playerHealthBar != null)
        {
            playerHealthBar.UpdateHealthBar(currentHealth);
        }
        else
        {
            Debug.LogWarning("playerhealthbar ist in updatehealthbar null");
        }
    }

    public void SaveHealth()
    {
        PlayerPrefs.SetInt("PlayerHealth", currentHealth);
        PlayerPrefs.Save();
        Debug.Log("gesundheit gespeichert: " + currentHealth);
    }

    public void LoadHealth()
    {
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            currentHealth = PlayerPrefs.GetInt("PlayerHealth");
            Debug.Log("gesundheit geladen: " + currentHealth);
        }
        else
        {
            currentHealth = maxHealth;
            Debug.Log("keinen gesundheitswert gefunden. setze 100 ein...");
        }
        UpdateHealthBar(currentHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        PlayerPrefs.SetInt("PlayerHealth", maxHealth);
        Debug.Log("gesundheit auf 100 gesetzt");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("lebenspunkte des spielers: " + currentHealth);
        if (playerHealthBar != null)
        {
            playerHealthBar.UpdateHealthBar(currentHealth);
        }
        else
        {
            Debug.Log("playerhealthbar nicht zugewiesen...");
        }

        OnHealthChange?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            //Death
            Debug.Log("death");
            levelManager.OnPlayerDeath();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            Debug.Log("geheilt");
        }
    }
}