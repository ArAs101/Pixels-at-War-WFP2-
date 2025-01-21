using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int bandagesCurrentlyInInventory;
    public int ammoCurrentlyInInventory;
    public int coinsCurrentlyInInventory;
    public GameObject inventoryPanel;
    public TextMeshProUGUI bandagesText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI coinsText;
    public PlayerController playerController;
    public TextMeshProUGUI ammoFraction;
    public int ammoInLoadedMagazine;
    public Gun gun;
    public LevelManager levelManager;
    public TextMeshProUGUI fireRateLevelText;
    public Button fireRateUpgradeButton;
    public TextMeshProUGUI precisionLevelText;
    public Button gunControlUpgradeButton;
    public int fireRateLevel;
    public int precisionLevel;
    public static Inventory Instance {  get; private set; }

    private void Start()
    {
        gun = FindObjectOfType<Gun>();
        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            LoadCurrentInventory();
            Debug.Log($"Anzeige aktualisieren: FireRateLevelText = {fireRateLevel}, PrecisionLevelText = {precisionLevel}");
        }
        else
        {
            ResetInventory();
            Debug.Log("keinen spielstand gefunden. setze inventar zurück");
        }

        inventoryPanel.SetActive(false);
        UpdateInventoryDisplay();        
    }

    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);
        playerController.DisableMovementAndShowCursor();
        Time.timeScale = 0f;
        UpdateInventoryDisplay();

        if (isActive)
        {  
            playerController.EnableMovement();
            Time.timeScale = 1f;
        }
    }

    public void UpdateInventoryDisplay()
    {
        
        bandagesText.text = bandagesCurrentlyInInventory.ToString();
        ammoText.text = ammoCurrentlyInInventory.ToString();
        coinsText.text = coinsCurrentlyInInventory.ToString();
        fireRateLevelText.text = fireRateLevel.ToString();
        precisionLevelText.text = precisionLevel.ToString();
        Debug.Log("updateinventorydisplay: level " + fireRateLevelText.text + " für feuergeschw., level " + precisionLevelText.text + " für präzision");
    }

    public void LoadCurrentInventory()
    {
        ammoCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointAmmo");
        bandagesCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointBandages");
        coinsCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointCoins");
        fireRateLevel = PlayerPrefs.GetInt("CheckpointFireRateLevel");
        precisionLevel = PlayerPrefs.GetInt("CheckpointPrecisionLevel");
        Debug.Log("lade inventar: " + ammoCurrentlyInInventory + " kugeln im inventar, " + bandagesCurrentlyInInventory + " bandagen im inventar, " + coinsCurrentlyInInventory + " münzen im inventar, level " + 
            fireRateLevel + " feuergeschwindigkeit, level " + precisionLevel + " präzision");
        Debug.Log($"FireRateLevel: {fireRateLevel}, PrecisionLevel: {precisionLevel}");

    }

    public void AddBandages(int amount)
    {
        //Debug.Log("addbandages aufgerufen");
        bandagesCurrentlyInInventory += amount;
        UpdateInventoryDisplay();        
    }

    public void AddCoin(int amount)
    {
        //Debug.Log("addcoin aufgerufen");
        coinsCurrentlyInInventory += amount;
        UpdateInventoryDisplay();
    }

    public void AddAmmo(int amount)
    {
        //Debug.Log("addammo aufgerufen");
        ammoCurrentlyInInventory += amount;        
        UpdateInventoryDisplay();
        gun.UpdateAmmoFraction();
    }

    public void ResetInventory()
    {
        bandagesCurrentlyInInventory = 0;
        ammoCurrentlyInInventory = 0;
        coinsCurrentlyInInventory = 0;
        fireRateLevel = 1;
        precisionLevel = 1;
        fireRateLevelText.text = "1";
        precisionLevelText.text = "1";
        UpdateInventoryDisplay();
        Debug.Log("inventar zurückgesetzt");
    }

    public void IncreaseFireRateLevel()
    {
        if (fireRateUpgradeButton.interactable == false)
        {
            return;
        }

        if (fireRateLevel == 3)
        {
            fireRateUpgradeButton.interactable = false;
        }

        if (coinsCurrentlyInInventory >= 5)
        {
            Gun.fireRate += 5;
            coinsCurrentlyInInventory -= 5;
            ++fireRateLevel;
            fireRateLevelText.text = (fireRateLevel).ToString();
            UpdateInventoryDisplay();
        }
    }

    public void IncreasePrecision()
    {
        if (gunControlUpgradeButton.interactable == false)
        {
            return;
        }

        if (precisionLevel >= 3)
        {
            gunControlUpgradeButton.interactable = false;
        }

        if (coinsCurrentlyInInventory >= 5)
        {
            Gun.gunControl -= 5;
            coinsCurrentlyInInventory -= 5;
            ++precisionLevel;
            precisionLevelText.text = (precisionLevel).ToString();
            UpdateInventoryDisplay();
        }
    }
}
