using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage;
    public float range = 100f;
    public static float fireRate = 10f;
    public static float gunControl = 25f;
    public Camera fpsCam;
    private float nextTimeToFire;
    public ParticleSystem particles;
    public GameObject impactEffect;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    private bool isReloading = false;
    public int currentAmmoInMagazine;
    public TextMeshProUGUI ammoFraction;
    public Animator gunAnimator;
    public Inventory playerInventory;
    public float reloadTime = 0.5f;
    public TextMeshProUGUI reloadText;
    private bool isDisplayingText = false;




    void Start()
    {
        currentAmmoInMagazine = PlayerPrefs.GetInt("CheckpointLoadedAmmo", 30);
        if (ammoFraction == null)
        {
            ammoFraction = GameObject.Find("AmmoFraction").GetComponent<TextMeshProUGUI>();
            if (ammoFraction == null)
            {
                Debug.LogError("ammofraction nicht gefunden");
            }
            else
            {
                Debug.Log("ammofraction gefunden");
            }
        }
        if (particles != null)
        {
            Debug.Log("Particles assigned: " + particles.name);
        }
        else
        {
            Debug.LogWarning("Particles reference is missing or destroyed!");
        }
        if (playerInventory == null)
        {
            playerInventory = GameObject.FindObjectOfType<Inventory>();
            if (playerInventory == null)
            {
                Debug.LogError("inventar nicht gefunden");
            }
        }
        if (gunAnimator == null)
        {
            gunAnimator = GetComponent<Animator>();
        }
        if (reloadText == null)
        {
            reloadText = GameObject.Find("ReloadText").GetComponent<TextMeshProUGUI>();
            if (reloadText == null)
            //{
            //reloadText.gameObject.SetActive(false);
            //}
            //else
            {
                Debug.LogWarning("textfeld nicht gefunden");
            }
        }
        else
        {
            Debug.LogWarning("textfeld gefunden");
        }
    }

    void Awake()
    {
        if (particles == null)
        {
            particles = GetComponentInChildren<ParticleSystem>();
            if (particles == null)
            {
                Debug.LogWarning("Particles not found in children!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmoInMagazine > 0 && !isReloading)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();

            particles.Play();
            //Debug.Log("schuss...");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            particles.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && currentAmmoInMagazine == 0 && playerInventory.ammoCurrentlyInInventory >= 1)
            {
                StartCoroutine(ReloadGun());
                isReloading = false;
            }
            else
            {
                if (currentAmmoInMagazine > 0)
                {
                    string message = currentAmmoInMagazine + " Kugeln sind noch im Magazin!";
                    if (!isDisplayingText)
                    {
                        StartCoroutine(DisplayHUDMessage(message));
                    }
                }
            }
        }
        UpdateAmmoFraction();
    }

    private IEnumerator DisplayHUDMessage(string message)
    {
        isDisplayingText = true;
        reloadText.text = message;
        reloadText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        reloadText.gameObject.SetActive(false);
        isDisplayingText = false;
    }

    void Shoot()
    {
        if (particles == null)
        {
            Debug.LogWarning("referenz auf partikel fehlt...");
        }
        RaycastHit hit;


        float randomY = Random.Range(-gunControl, gunControl);
        float randomX = Random.Range(-gunControl, gunControl);
        Quaternion randomRotation = Quaternion.Euler(randomX, randomY, 0);
        Vector3 randomDirection = randomRotation * fpsCam.transform.forward;
        if (Physics.Raycast(fpsCam.transform.position, randomDirection, out hit, range))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                Debug.Log(hit.collider.name);
                damage = Random.Range(10, 15);
                target.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, Random.Range(1f, 100f));
            }
            else
            {
                Debug.LogWarning("impact referenz fehlt...");
            }
        }
        //Debug.DrawRay(fpsCam.transform.position, randomDirection * range, Color.green, 10f);
        currentAmmoInMagazine--;
        UpdateAmmoFraction();

    }


    public void UpdateAmmoFraction()
    {
        if (ammoFraction != null)
        {
            int ammoUnits = playerInventory.ammoCurrentlyInInventory * 30;
            ammoFraction.text = currentAmmoInMagazine + "/" + ammoUnits;
            //Debug.Log(currentAmmoInMagazine);
        }
        else
        {
            Debug.LogWarning("ammofraction-element nicht zugewiesen");
        }
        //ammoFraction.text = "test";
        //Debug.Log(ammoFraction.text);
    }

    private IEnumerator ReloadGun()
    {
        isReloading = true;
        Debug.Log("nachladen...");
        gunAnimator.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(reloadTime);
        currentAmmoInMagazine = 30;
        playerInventory.ammoCurrentlyInInventory -= 1;
        Debug.Log("nachgeladen, " + 30 + " kugeln im magazin der ak74su");
        playerInventory.UpdateInventoryDisplay();
        UpdateAmmoFraction();
    }
}