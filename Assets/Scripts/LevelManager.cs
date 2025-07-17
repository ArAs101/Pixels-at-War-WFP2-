using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;

public class LevelManager : MonoBehaviour
{
    public DrunkardsWalk dungeonGenerator;
    public SpawnController spawnController;
    public PlayerController playerController;
    public int currentLevel;
    private int enemyCount;
    private int ammoCount;
    private int bandagesCount;
    private int coinsCount;
    private int remainingEnemies;
    public GameObject levelDonePanel;
    public GameObject hudPanel;
    public GameObject gameFinishedPanel;
    public Inventory playerInventory;
    public MenuManager menuManager;
    public GameObject deathScreen;
    public PlayerHealthBar playerHealthBar;
    public PlayerHealth playerHealth;
    public TextMeshProUGUI levelIndicator;
    private int savedAmmoCount;
    private int savedBandagesCount;
    private int savedCoins;
    private int savedLoadedAmmo;
    private int savedFireRateLevel;
    private int savedPrecisionLevel;
    private Gun gun;
    public NavMeshSurface navMeshSurface;
    public GameObject shouldBeDeadPanel;
    public DungeonBatchGenerator dungeonBatchGenerator;

    private void Start()
    {
        enemyCount = 10;
        ammoCount = 10;
        bandagesCount = 10;
        coinsCount = 25;
        deathScreen.SetActive(false);
        StartCoroutine(InitializeLevel(currentLevel));
    }

    IEnumerator InitializeLevel(int level)
    {

        currentLevel = level;
#if UNITY_WEBGL
if (PlayerPrefs.HasKey("ExperimentAbgeschlossen") && PlayerPrefs.GetInt("ExperimentAbgeschlossen") == 1)
{
    //Debug.Log("Spiel wurde bereits abgeschlossen. Beende Spiel.");
    SceneManager.LoadScene("MainMenuWelcome");
    yield break;
}
#endif

        //Debug.Log("level " + currentLevel + " wird initialisiert");
        Time.timeScale = 1;
        spawnController.DeleteCollectables();
        //dungeonGenerator.GenerateGrid();
        //dungeonGenerator.GenerateDrunkardsWalk();
        //dungeonGenerator.DrawDungeon();
        spawnController.Initialize(dungeonGenerator);
        //spawnController.ClearPreviousSpawns();
        //spawnController.CollectSpawnPositions(enemyCount, ammoCount);
        //dungeonBatchGenerator.GenerateMultipleDungeons();
        //dungeonGenerator.VisualizeDungeonWithColorTiles();
        //StartCoroutine(dungeonGenerator.CaptureTopDownScreenshot());
        SetEnemyCount(10);
        //spawnController.SpawnPlayer();
        playerController.HideCursor();
        //spawnController.SpawnAmmo(ammoCount);
        //spawnController.SpawnBandages(bandagesCount);
        //spawnController.SpawnCoins(coinsCount);
        //spawnController.SpawnEnemies(enemyCount);
        navMeshSurface.BuildNavMesh();
        yield return new WaitForEndOfFrame();
        playerInventory = GameObject.FindObjectOfType<Inventory>();
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        playerHealthBar = GameObject.FindObjectOfType<PlayerHealthBar>();
        gun = GameObject.FindObjectOfType<Gun>();
        //yield return new WaitForEndOfFrame();

        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            //Debug.Log(PlayerPrefs.GetInt("CheckpointAmmo") + " kugeln im inventar, bandagen: " + PlayerPrefs.GetInt("CheckpointBandages") + ", gesundheit: "
            //+ playerHealth.currentHealth + ", münzen: " + PlayerPrefs.GetInt("CheckpointCoins") + ", im magazin: " + PlayerPrefs.GetInt("CheckpointLoadedAmmo") +
            //", feuergeschwindigkeitslevel: " + PlayerPrefs.GetInt("CheckpointFireRateLevel") + ", präzisionslevel: " + PlayerPrefs.GetInt("CheckpointPrecisionLevel"));
            LoadCheckpoint();
        }
        else
        {
            //Debug.Log("keinen gespeicherten spielstand gefunden. starte neues spiel");
        }
        
        levelIndicator.text = "Level " + currentLevel;
        //Debug.Log("Level " + currentLevel + " gestartet");

        //Debug.Log("zu beginn des jeweiligen levels im levelmanager: " + playerInventory.ammoCurrentlyInInventory + " kugeln im inventar, bandagen: " + playerInventory.bandagesCurrentlyInInventory + ", gesundheit: "
            //+ playerHealth.currentHealth + ", münzen: " + playerInventory.coinsCurrentlyInInventory + ", im magazin: " + playerInventory.ammoInLoadedMagazine + "im magazin: " + gun.currentAmmoInMagazine +
            //", feuergeschw.: " + playerInventory.fireRateLevelText.text + ", präzision: " + playerInventory.precisionLevelText.text);        
    }

    public void SaveCheckpoint()
    {
        if (playerInventory != null)
        {
            PlayerPrefs.SetInt("CheckpointLevel", currentLevel);
            PlayerPrefs.SetInt("CheckpointAmmo", playerInventory.ammoCurrentlyInInventory);
            PlayerPrefs.SetInt("CheckpointBandages", playerInventory.bandagesCurrentlyInInventory);
            PlayerPrefs.SetInt("CheckpointHealth", playerHealth.currentHealth);
            PlayerPrefs.SetInt("CheckpointCoins", playerInventory.coinsCurrentlyInInventory);
            PlayerPrefs.SetInt("CheckpointLoadedAmmo", gun.currentAmmoInMagazine);
            PlayerPrefs.SetInt("CheckpointFireRateLevel", int.Parse(playerInventory.fireRateLevelText.text));
            PlayerPrefs.SetInt("CheckpointPrecisionLevel", int.Parse(playerInventory.precisionLevelText.text));
            PlayerPrefs.Save();
            //Debug.Log("checkpoint gespeichert: level " + currentLevel + ", munition: " + playerInventory.ammoCurrentlyInInventory + ", bandagen: " + playerInventory.bandagesCurrentlyInInventory + ", gesundheit: " +
                //playerHealth.currentHealth + ", münzen: " + playerInventory.coinsCurrentlyInInventory + ", im magazin: " + playerInventory.ammoInLoadedMagazine + ", feuergeschwindigkeitslevel: " + 
                //playerInventory.fireRateLevelText.text + ", präzisionslevel: " + playerInventory.precisionLevelText.text);
        }
        else
        {
            //Debug.LogError("inventar in savecheckpoint nicht gefunden");
        }
    }

    public void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CheckpointLevel");
            savedAmmoCount = PlayerPrefs.GetInt("CheckpointAmmo");
            savedBandagesCount = PlayerPrefs.GetInt("CheckpointBandages");
            playerHealth.currentHealth = PlayerPrefs.GetInt("CheckpointHealth");
            savedCoins = PlayerPrefs.GetInt("CheckpointCoins");
            savedLoadedAmmo = PlayerPrefs.GetInt("CheckpointLoadedAmmo");
            playerHealth.UpdateHealthBar(playerHealth.currentHealth);
            savedFireRateLevel = PlayerPrefs.GetInt("CheckpointFireRateLevel");
            savedPrecisionLevel = PlayerPrefs.GetInt("CheckpointPrecisionLevel");
            //Debug.Log("checkpoint geladen: level " + currentLevel + ", munition: " + savedAmmoCount + ", bandagen: " + savedBandagesCount + ", gesundheit: " + playerHealth.currentHealth + ", münzen: " + savedCoins
                //+ ", im magazin: " + savedLoadedAmmo + ", feuergeschwindigkeitslevel: " + savedFireRateLevel + ", präzisionslevel: " + savedPrecisionLevel);
        }
        else
        {
            //Debug.LogWarning("keinen checkpoint gefunden. starte neues spiel...");
        }
    }

    public void OnSaveButtonClicked()
    {
        if (playerInventory != null)
        {
            currentLevel++;
            SaveCheckpoint();
#if !UNITY_WEBGL
            PlayerPrefs.DeleteAll();
#endif
            Application.Quit();
        }
        else
        {
            //Debug.LogError("inventory ist nicht zugewiesen");
        }
    }

    public void OnPlayerDeath()
    {
        if (deathScreen != null)
        {
            shouldBeDeadPanel.SetActive(true);
        }
        else
        {
            //Debug.LogError("deathscreen nicht zugewiesen");
        }
    }

    public void SetEnemyCount(int count)
    {
        remainingEnemies = 10;
    }

    public void EnemyDied()
    {
        remainingEnemies--;
        if (remainingEnemies == 0)
        {
            //Debug.Log("Alle Gegner besiegt");
            if (currentLevel == 3)
            {
                ShowGameFinishedPanel();
            }
            else
            {
                ShowLevelDonePanel();
            }
        }
    }

    public void ShowLevelDonePanel()
    {
        if (levelDonePanel != null)
        {
            levelDonePanel.SetActive(true);
            Time.timeScale = 0;
            playerController.DisableMovementAndShowCursor();
            PlayerPrefs.SetInt("ExperimentAbgeschlossen", 1);
            PlayerPrefs.Save();
        }
        else
        {
            //Debug.LogError("Level-Done-Panel ist nicht zugewiesen!");
        }
    }

    public void ShowGameFinishedPanel()
    {
        if (gameFinishedPanel != null)
        {
            gameFinishedPanel.SetActive(true);
            Time.timeScale = 0;
            playerController.DisableMovementAndShowCursor();
        }
        else
        {
            //Debug.LogError("game-finished-panel ist nicht zugewiesen!");
        }
    }


    public void StartNextLevel()
    {
        levelDonePanel.SetActive(false);
        if (playerHealth == null)
        {
            //Debug.Log("playerhealth ist null");
            playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        }
        currentLevel++;
        SaveCheckpoint();
        StartCoroutine(InitializeLevel(currentLevel));
    }

    public void RestartCurrentLevelWithSavedProgress()
    {
        //Debug.Log("starte level neu...");
        deathScreen.SetActive(false);
        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            //Debug.Log(PlayerPrefs.GetInt("CheckpointAmmo") + " kugeln im inventar, bandagen: " + PlayerPrefs.GetInt("CheckpointBandages") + ", gesundheit: "
            //+ playerHealth.currentHealth + ", münzen: " + PlayerPrefs.GetInt("CheckpointCoins") + ", im magazin: " + PlayerPrefs.GetInt("CheckpointLoadedAmmo"));
            LoadCheckpoint();
        }
        else
        {
            //Debug.Log("kein checkpointlevel gefunden");
            playerHealth.ResetHealth();
            playerInventory.ResetInventory();
        }

        //Debug.Log("in restartmethode: " + playerInventory.ammoCurrentlyInInventory + " kugeln im inventar, bandagen: " + playerInventory.bandagesCurrentlyInInventory + ", gesundheit: " 
            //+ playerHealth.currentHealth + ", münzen: " + playerInventory.coinsCurrentlyInInventory + ", im magazin: " + playerInventory.ammoInLoadedMagazine);

        StartCoroutine(InitializeLevel(currentLevel));
    }
}