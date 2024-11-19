using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadGameMenuManager : MonoBehaviour
{
    public TextMeshProUGUI savedGameText;
    private PlayerHealth playerHealth;
    private Inventory playerInventory;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        DisplaySavedGame();
    }

    public void DisplaySavedGame()
    {
        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            int savedLevel = PlayerPrefs.GetInt("CheckpointLevel");
            int savedAmmo = PlayerPrefs.GetInt("CheckpointAmmo");
            int savedBandages = PlayerPrefs.GetInt("CheckpointBandages");
            int savedHealth = PlayerPrefs.GetInt("CheckpointHealth");
            savedGameText.text = $"Gespeicherter Spielstand: \nLevel {savedLevel}, \nMunition: {savedAmmo}, \nBandagen: {savedBandages}, \nGesundheit: {savedHealth}";
        }
        else
        {
            savedGameText.text = "Kein gespeicherter Spielstand vorhanden";
        }
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            int levelToLoad = PlayerPrefs.GetInt("CheckpointLevel");
            int health = PlayerPrefs.GetInt("CheckpointHealth");
            int ammo = PlayerPrefs.GetInt("CheckpointAmmo");
            int bandages = PlayerPrefs.GetInt("CheckpointBandages");

            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                Debug.Log("Spielstand geladen: Level " + levelToLoad + ", Gesundheit " + health + ", Munition " + ammo + ", Bandagen " + bandages);
                SceneManager.LoadScene("GameScene");
                
            }
        }
        else
        {
            Debug.LogError("Kein gespeicherter Spielstand vorhanden.");
        }
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuWelcome");
    }
}
