using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;
    public Slider slider;

    void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = GameObject.FindWithTag("Player")?.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth-Komponente im Spielerobjekt nicht gefunden.");
            }
        }

        if (fillImage == null)
        {
            fillImage = transform.Find("Fill Area/Fill")?.GetComponent<Image>();
            if (fillImage == null)
            {
                Debug.LogError("Fill Image-Objekt nicht gefunden.");
            }
        }

        if (slider == null)
        {
            slider = GetComponent<Slider>();
            if (slider == null)
            {
                Debug.LogError("Slider-Komponente im PlayerHealthBar nicht gefunden.");
            }
        }

        if (playerHealth != null && slider != null)
        {
            slider.maxValue = playerHealth.maxHealth;
            UpdateHealthBar(playerHealth.currentHealth);
        }
    }

    public void UpdateHealthBar(int currentHealth)
    {
        Debug.Log("aktualisierte gesundheitsanzeige: " + currentHealth);
        slider.value = currentHealth;
        fillImage.gameObject.SetActive(slider.value > 0);
        if (slider.value <= 0)
        {
            fillImage.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthChange -= UpdateHealthBar;
    }
}