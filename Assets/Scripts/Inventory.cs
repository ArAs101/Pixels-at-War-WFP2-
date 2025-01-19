using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int bandagesCurrentlyInInventory;
    public int ammoCurrentlyInInventory;
    public int coinsCurrentlyInInventory;
    //private int savedBandages;
    //private int savedAmmo;
    //private int savedCoins;
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
    public TextMeshProUGUI gunControl;
    public Button gunControlUpgradeButton;
    public static Inventory Instance {  get; private set; }

   
private void Awake(){
    fireRateLevelText.text = "0";
    gunControl.text = "0";
}

    private void Start()
    {
        gun = FindObjectOfType<Gun>();
        if (PlayerPrefs.HasKey("CheckpointLevel"))
        {
            LoadCurrentInventory();
        }
        else
        {
            ResetInventory();
            //Debug.Log("keinen spielstand gefunden. setze inventar zurück");
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
    }

    public void LoadCurrentInventory()
    {
        ammoCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointAmmo");
        bandagesCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointBandages");
        coinsCurrentlyInInventory = PlayerPrefs.GetInt("CheckpointCoins");
        //ammoInLoadedMagazine = PlayerPrefs.GetInt("CheckpointLoadedAmmo");
        Debug.Log("lade inventar: " + ammoCurrentlyInInventory + " kugeln im inventar, " + bandagesCurrentlyInInventory + " bandagen im inventar, " + coinsCurrentlyInInventory + " münzen im inventar");
        //UpdateInventoryDisplay();
    }

    public void AddBandages(int amount)
    {
        Debug.Log("addbandages aufgerufen");
        bandagesCurrentlyInInventory += amount;
        UpdateInventoryDisplay();        
    }

    public void AddCoin(int amount)
    {
        Debug.Log("addcoin aufgerufen");
        coinsCurrentlyInInventory += amount;
        UpdateInventoryDisplay();
    }

    public void AddAmmo(int amount)
    {
        Debug.Log("addammo aufgerufen");
        ammoCurrentlyInInventory += amount;        
        UpdateInventoryDisplay();
        gun.UpdateAmmoFraction();
    }

    public void ResetInventory()
    {
        bandagesCurrentlyInInventory = 0;
        ammoCurrentlyInInventory = 0;
        coinsCurrentlyInInventory = 0;
        //ammoInLoadedMagazine = 30;
        //UpdateInventoryDisplay();
        Debug.Log("inventar zurückgesetzt");
    }
}
