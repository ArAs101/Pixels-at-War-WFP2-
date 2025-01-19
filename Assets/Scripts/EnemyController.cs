using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public Transform fireBallSpawnpoint;
    public GameObject fireBallPrefab;
    //public float fireRate = 2f;
    //private float nextTimeToFire = 3f;
    //public float fireBallSpeed = 10;
    private bool isShooting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (SpawnController.player != null)
        {
            player = SpawnController.player.transform;
        }
        else
        {
            Debug.LogError("Spieler mit Tag 'Player' nicht gefunden!");
        }
    }

    void Update()
    {
        //if (player != null && agent != null)
        //{
        agent.SetDestination(player.position);
        if (agent.name == "FireyEnemy(Clone)" && !isShooting)
        {
            isShooting = true;
            InvokeRepeating(nameof(ShootAtPlayer), 0f, 3f);
            //ShootAtPlayer();
            //StartCoroutine(ShootAtPlayer());
            /*if (Time.time >= nextTimeToFire)
            {
            nextTimeToFire = Time.time + (1f / fireRate);
                ShootAtPlayer();
                Debug.Log("schuss abgegeben, " + Time.time);
                
                Debug.Log("nächster schuss: " + nextTimeToFire);
            }*/
        }
        //}
        else if (agent == null)
        {
            Debug.LogError("agent nicht gefunden...");
        }
        else if (player == null)
        {
            Debug.LogError("player nicht gefunden...");
        }
    }

    private void ShootAtPlayer()
    {
        //while (true)
        //{
            //yield return new WaitForSeconds(nextTimeToFire);
            if (fireBallPrefab != null && fireBallSpawnpoint != null)
            {
                GameObject fireBall = Instantiate(fireBallPrefab, fireBallSpawnpoint.position, Quaternion.identity);
                BulletController bulletController = fireBall.GetComponent<BulletController>();
                if (bulletController != null)
                {
                    bulletController.SetDirection(transform); // Setze die Blickrichtung des Gegners
                }
            }
            else
            {
                Debug.LogError("Feuerball oder SpawnPoint fehlt!");
            }
        //}
    }

    private void OnDisable()
    {
        isShooting = false;
        CancelInvoke(nameof(ShootAtPlayer));
    }
}