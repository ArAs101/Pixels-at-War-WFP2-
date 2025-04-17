using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Inventory playerInventory;
    [SerializeField]
    public GameObject gameObject;
    int wait = 0;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Spieler hat einen Gegner berührt!");
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                int damageAmount = 1;
                Debug.Log("Spieler erleidet " + damageAmount + " Schaden.");
                if (wait > 10)
                {
                    wait = 0;
                    playerHealth.TakeDamage(damageAmount);
                }
                else
                {
                    wait++;
                }
            }
            else
            {
                Debug.LogWarning("PlayerHealth-Komponente nicht gefunden!");
            }
        }
        if (!gameObject.CompareTag("Enemy"))
        {
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

            if (other.CompareTag("Coins"))
            {
                Debug.Log("coin berührt");
                playerInventory.AddCoin(1);
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Fire"))
            {
                Debug.Log("Spieler hat einen Feuerball berührt!");
                PlayerHealth playerHealth = GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    int damageAmount = 1;
                    Debug.Log("Spieler erleidet " + damageAmount + " Schaden.");
                    playerHealth.TakeDamage(damageAmount);
                    Destroy(other.gameObject);
                }
                else
                {
                    Debug.LogWarning("PlayerHealth-Komponente nicht gefunden!");
                }
            }
        }
    }
}
