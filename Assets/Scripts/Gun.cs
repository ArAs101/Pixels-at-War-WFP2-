using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage;
    public float range = 100f;
    public float fireRate = 15f;
    public Camera fpsCam;
    private float nextTimeToFire = 0f;
    public ParticleSystem particles;
    public GameObject impactEffect;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;


    void Start()
    {
        if (particles != null)
        {
            Debug.Log("Particles assigned: " + particles.name);
        }
        else
        {
            Debug.LogWarning("Particles reference is missing or destroyed!");
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
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            
            particles.Play();
            Debug.Log("schuss...");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            particles.Stop();
        }
    }
    
    void Shoot()
    {
        if (particles == null)
        {
            Debug.LogWarning("referenz auf partikel fehlt...");
        }
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                Debug.Log(hit.collider.name);
                damage = Random.Range(1, 10);
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
    }

}