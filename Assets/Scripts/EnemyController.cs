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
    private bool isShooting = false;
    public float detectionRadius = 15f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (SpawnController.player != null)
        {
            player = SpawnController.player.transform;
        }
        else
        {
            GameObject manuallSetPlayer = GameObject.FindWithTag("Player");
            if (manuallSetPlayer != null)
            {
                player = manuallSetPlayer.transform;
                //Debug.Log("manuell platzierten spieler gefunden");
            }
            else
            {
                //Debug.LogError("Spieler mit Tag 'Player' nicht gefunden!");
            }
        }
        
    }

    void Update()
    {
        if (player == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            agent.SetDestination(player.position);

            if (agent.name.Contains("FireyEnemy") && !isShooting)
            {
                isShooting = true;
                InvokeRepeating(nameof(ShootAtPlayer), 0f, 5f);
            }
        }
        else
        {
            agent.ResetPath();

            if (isShooting)
            {
                isShooting = false;
                CancelInvoke(nameof(ShootAtPlayer));
            }
        }
    }


    private void ShootAtPlayer()
    {
            if (fireBallPrefab != null && fireBallSpawnpoint != null)
            {
                GameObject fireBall = Instantiate(fireBallPrefab, fireBallSpawnpoint.position, Quaternion.identity);
                BulletController bulletController = fireBall.GetComponent<BulletController>();
                if (bulletController != null)
                {
                    bulletController.SetDirection(transform);
                }
            }
            else
            {
                //Debug.LogError("Feuerball oder SpawnPoint fehlt!");
            }
    }

    private void OnDisable()
    {
        isShooting = false;
        CancelInvoke(nameof(ShootAtPlayer));
    }
}