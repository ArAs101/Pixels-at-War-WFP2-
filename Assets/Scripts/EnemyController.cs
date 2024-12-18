using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    //[SerializeField]
    private Transform player;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

       
        if (SpawnController.player != null)
        {
            player = SpawnController.player.transform;
            //Debug.Log("Spieler gefunden: " + player.name);
        }
        else
        {
            Debug.LogError("Spieler mit Tag 'Player' nicht gefunden!");
        }
    }

    void Update()
    {
        if (player != null && agent != null)
        {
            agent.SetDestination(player.position);
            Debug.Log("Ziel gesetzt: " + player.position + " pfadstatus: " + agent.pathStatus);
            //DrawPathGizmos(agent.path);
            //Debug.DrawLine(agent.destination, agent.destination + Vector3.up, Color.blue, 20f);
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

    private void DrawPathGizmos(NavMeshPath path)
    {
        if (path == null || path.corners.Length < 2)
            return;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
        }
    }
}