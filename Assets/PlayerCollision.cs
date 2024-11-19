using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Inventory playerInventory;

    private void Start()
    {
        if (playerInventory == null)
        {
            playerInventory = GameObject.FindObjectOfType<Inventory>();
            if (playerInventory == null)
            {
                Debug.LogError("inventory in playercollision nicht gefunden");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Spieler hat einen Gegner berührt!");

            PlayerHealth playerHealth = GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                int damageAmount = Random.Range(10, 20);
                Debug.Log("Spieler erleidet " + damageAmount + " Schaden.");
                playerHealth.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("PlayerHealth-Komponente nicht gefunden!");
            }
        }

        if (other.CompareTag("Ammo"))
        {
            Debug.Log("munition berührt");
            playerInventory.AddAmmo(1);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Bandages"))
        {
            Debug.Log("bandage berührt");
            playerInventory.AddBandages(1);
            Destroy(other.gameObject);
        }
    }
}
