using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public Inventory playerInventory;
    public PlayerController playerController;
    public LevelManager levelManager;
    private bool isNewGame = false;
    public PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene" && playerInventory != null)
        {
            playerController = GameObject.FindObjectOfType<PlayerController>();
            playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
            playerController.EnableMovement();
            Debug.Log("Inventar zurückgesetzt und bewegung aktiviert nach Szenenwechsel.");

            if (isNewGame)
            {
                playerInventory.ResetInventory();
                playerHealth.ResetHealth();
            }
            else
            {
                playerHealth.LoadHealth();
            }


            if (levelManager == null)
            {
                levelManager = GameObject.FindObjectOfType<LevelManager>();
                if (levelManager == null)
                {
                    Debug.LogError("levelmanager in onsceneloaded nicht zugewiesen");
                }
            }
        }
    }

    public void NewGame()
    {
        isNewGame = true;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LevelC");
    }

    public void LoadGame()
    {
        isNewGame = false;
        SceneManager.LoadScene("LoadGameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuWelcome");
    }
}
