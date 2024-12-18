using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int bandagesCurrentlyInInventory = 0;
    public int ammoCurrentlyInInventory = 0;
    private int savedBandages;
    private int savedAmmo;

    public GameObject inventoryPanel;
    public TextMeshProUGUI bandagesText;
    public TextMeshProUGUI ammoText;
    public PlayerController playerController;
    public TextMeshProUGUI ammoFraction;
    public LevelManager levelManager;
    public static Inventory Instance {  get; private set; }

   
    private void Start()
    {
        LoadCurrentInventory();       
        inventoryPanel.SetActive(false);
        UpdateInventoryDisplay();
    }

    private void Update()
    {
        // Inventar anzeigen/verstecken
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
        

        if (isActive)
        {
            UpdateInventoryDisplay();
            playerController.EnableMovement();
        }
    }

    private void UpdateInventoryDisplay()
    {
        bandagesText.text =  bandagesCurrentlyInInventory.ToString();
        ammoText.text = ammoCurrentlyInInventory.ToString();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadCurrentInventory()
    {
        ammoCurrentlyInInventory = PlayerPrefs.GetInt("CurrentAmmo", 0);
        bandagesCurrentlyInInventory = PlayerPrefs.GetInt("CurrentBandages", 0);
        UpdateInventoryDisplay();
    }

    public void AddBandages(int amount)
    {
        Debug.Log("addbandages aufgerufen");
        bandagesCurrentlyInInventory += amount;
        UpdateInventoryDisplay();        
    }

    public void AddAmmo(int amount)
    {
        Debug.Log("addammo aufgerufen");
        ammoCurrentlyInInventory += amount;
        UpdateInventoryDisplay();
        //ammoFraction.text.Insert(2, ammoCurrentlyInInventory.ToString());
    }

    /*public void SaveInventory()
    {
        savedBandages = bandagesCurrentlyInInventory;
        savedAmmo = ammoCurrentlyInInventory;
        Debug.Log("inventar gespeichert: " + bandagesCurrentlyInInventory + " Bandagen, " + ammoCurrentlyInInventory + " Munition");
    }

    public void LoadInventory()
    {
        bandagesCurrentlyInInventory = PlayerPrefs.GetInt("Bandages", 0);
        ammoCurrentlyInInventory = PlayerPrefs.GetInt("Ammo", 0);
        Debug.Log("inventar geladen: " + bandagesCurrentlyInInventory + " Bandagen, " + ammoCurrentlyInInventory + " Munition");
    }*/

    public void ResetInventory()
    {
        bandagesCurrentlyInInventory = savedBandages;
        ammoCurrentlyInInventory = savedAmmo;
        UpdateInventoryDisplay();
        Debug.Log("inventar zurückgesetzt");
    }
}
