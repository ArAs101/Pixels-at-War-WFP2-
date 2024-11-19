using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public DrunkardsWalk dungeonGenerator;
    public SpawnController spawnController;
    public PlayerController playerController;
    public int currentLevel;
    [SerializeField]
    private int enemyCount;
    [SerializeField]
    private int ammoCount;
    [SerializeField]
    private int bandagesCount;
    private int remainingEnemies;
    public GameObject levelDonePanel;
    public GameObject hudPanel;
    //public static Camera MainCamera { get; private set; }
    public static Camera MainCamera;
    public Inventory playerInventory;
    public MenuManager menuManager;
    public GameObject deathScreen;
    public PlayerHealthBar playerHealthBar;
    public PlayerHealth playerHealth;
    public TextMeshProUGUI levelIndicator;
    public int tempAmmoCount;
    public int tempBandagesCount;
    public int tempHealth;

    private void Start()
    {
        enemyCount = Random.Range(5, 5);
        ammoCount = 10;
        bandagesCount = 10;
        deathScreen.SetActive(false);
        StartCoroutine(InitializeLevel(currentLevel));
    }

    IEnumerator InitializeLevel(int level)
    {
        currentLevel = level;

        Debug.Log("level " + currentLevel + " wird initialisiert");
        Time.timeScale = 1;
        //LoadGame();
        spawnController.DeleteCollectables();
        spawnController.DespawnPlayer();
        dungeonGenerator.GenerateGrid();
        dungeonGenerator.GenerateDrunkardsWalk();
        dungeonGenerator.DrawDungeon();
        spawnController.Initialize(dungeonGenerator);
        spawnController.ClearPreviousSpawns();
        spawnController.CollectSpawnPositions(enemyCount, ammoCount);
        spawnController.SpawnEnemies(enemyCount);
        spawnController.SpawnAmmo(ammoCount);
        spawnController.SpawnBandages(bandagesCount);
        spawnController.SpawnPlayer();
        playerInventory = GameObject.FindObjectOfType<Inventory>();
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        playerHealthBar = GameObject.FindObjectOfType<PlayerHealthBar>();
        yield return new WaitForEndOfFrame();

        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            LoadCheckpoint();
        }

        if (PlayerPrefs.HasKey("CheckpointHealth") && playerHealth != null)
        {
            playerInventory.ammoCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointAmmo");
            playerInventory.bandagesCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointBandages");
            playerHealth.currentHealth = PlayerPrefs.GetInt("CheckpointHealth");
            playerHealth.UpdateHealthBar(playerHealth.currentHealth);
            Debug.Log("gesundheit für level " + currentLevel + " geladen: " + playerHealth.currentHealth);
        }
        else
        {
            Debug.Log("keine gespeicherte gesundheit gefunden");
        }
        levelIndicator.text = "Level " + currentLevel;
        Debug.Log("Level " + currentLevel + " gestartet");
    }

    public void SaveCheckpoint()
    {
        PlayerPrefs.SetInt("CheckpointLevel", currentLevel);
        PlayerPrefs.SetInt("CheckpointAmmo", playerInventory.ammoCurrentlyInInventory);
        PlayerPrefs.SetInt("CheckpointBandages", playerInventory.bandagesCurrentlyInInventory);
        PlayerPrefs.SetInt("CheckpointHealth", playerHealth.currentHealth);
        PlayerPrefs.Save();
        Debug.Log("checkpoint gespeichert: level " + currentLevel + ", munition: " + tempAmmoCount + ", bandagen: " + tempBandagesCount + ", gesundheit: " + playerHealth.currentHealth);
    }

    public void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            int checkpointHealth = PlayerPrefs.GetInt("CheckpointHealth");
            currentLevel = PlayerPrefs.GetInt("CheckpointLevel");
            tempAmmoCount = PlayerPrefs.GetInt("CheckpointAmmo");
            tempBandagesCount = PlayerPrefs.GetInt("CheckpointBandages");
            playerHealth.currentHealth = PlayerPrefs.GetInt("CheckpointHealth");
            playerHealth.UpdateHealthBar(playerHealth.currentHealth);
            Debug.Log("checkpoint gespeichert: level " + currentLevel + ", munition: " + tempAmmoCount + ", bandagen: " + tempBandagesCount + ", gesundheit: " + playerHealth.currentHealth);
        }
        else
        {
            Debug.LogWarning("keinen checkpoint gefunden. starte neues spiel...");
        }
    }

    public void OnSaveButtonClicked()
    {
        if (playerInventory != null)
        {
            currentLevel++;
            SaveCheckpoint();
            menuManager.BackToMainMenu();
        }
        else
        {
            Debug.LogError("inventory ist nicht zugewiesen");
        }
    }

    public void OnPlayerDeath()
    {
        if (deathScreen != null)
        {
            Debug.Log("du bist gestorben");
            deathScreen.SetActive(true);
            Time.timeScale = 0;
            playerController.DisableMovementAndShowCursor();
        }
        else
        {
            Debug.LogError("deathscreen nicht zugewiesen");
        }
    }

    public void SetEnemyCount(int count)
    {
        remainingEnemies = count;
    }

    public void EnemyDied()
    {
        remainingEnemies--;
        if (remainingEnemies == 0)
        {
            Debug.Log("Alle Gegner besiegt");

            ShowLevelDonePanel();
        }
    }

    public void ShowLevelDonePanel()
    {
        if (levelDonePanel != null)
        {
            levelDonePanel.SetActive(true);
            Time.timeScale = 0;
            playerController.DisableMovementAndShowCursor();
        }
        else
        {
            Debug.LogError("Level-Done-Panel ist nicht zugewiesen!");
        }
    }

    public void StartNextLevel()
    {
        levelDonePanel.SetActive(false);
        if (playerHealth == null)
        {
            Debug.Log("playerhealth ist null");
            playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        }
        currentLevel++;
        SaveCheckpoint();
        StartCoroutine(InitializeLevel(currentLevel));
    }

    public void RestartCurrentLevelWithSavedProgress()
    {
        Debug.Log("starte level neu...");
        deathScreen.SetActive(false);
        LoadCheckpoint();
        StartCoroutine(InitializeLevel(currentLevel));
    }
}
