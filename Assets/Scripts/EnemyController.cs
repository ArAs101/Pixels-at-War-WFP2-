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
        agent.SetDestination(player.position);
        if (agent.name.Contains("FireyEnemy") && !isShooting)
        {
            isShooting = true;
            InvokeRepeating(nameof(ShootAtPlayer), 0f, 3f);
        }
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
                Debug.LogError("Feuerball oder SpawnPoint fehlt!");
            }
    }

    private void OnDisable()
    {
        isShooting = false;
        CancelInvoke(nameof(ShootAtPlayer));
    }
}