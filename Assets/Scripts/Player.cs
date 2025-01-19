using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Gun gun;

    public void IncreaseFireRateLevel()
    {
          Inventory playerInventory = GameObject.FindObjectOfType<Inventory>();
        if (playerInventory.fireRateUpgradeButton.interactable == false)
        {
            return;
        }

        if (playerInventory.fireRateLevelText.text == "3")
        {
            playerInventory.fireRateUpgradeButton.interactable = false;
        }

        if (playerInventory.coinsCurrentlyInInventory >= 5)
        {

            Gun.fireRate += 5;
            playerInventory.coinsCurrentlyInInventory -= 5;
            playerInventory.fireRateLevelText.text = (int.Parse(playerInventory.fireRateLevelText.text) + 1).ToString();
            playerInventory.UpdateInventoryDisplay();
        }

    }


      public void IncreasePrecision()
    {
          Inventory playerInventory = GameObject.FindObjectOfType<Inventory>();
        if (playerInventory.gunControlUpgradeButton.interactable == false)
        {
            return;
        }

        if (playerInventory.gunControl.text == "3")
        {
            playerInventory.gunControlUpgradeButton.interactable = false;
        }

        if (playerInventory.coinsCurrentlyInInventory >= 5)
        {

            Gun.gunControl -= 5;
            playerInventory.coinsCurrentlyInInventory -= 5;
            playerInventory.gunControl.text = (int.Parse(playerInventory.gunControl.text) + 1).ToString();
            playerInventory.UpdateInventoryDisplay();
        }
    }
}
