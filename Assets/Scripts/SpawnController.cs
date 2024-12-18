using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject ammoPrefab;
    public GameObject bandagePrefab;
    public DrunkardsWalk drunkardsWalkInstance;
    public LevelManager levelManager;
    public PlayerController playerController;
    public static GameObject player;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<Vector3> enemySpawnPositions = new List<Vector3>();
    private List<Vector3> itemSpawnPositions = new List<Vector3>();
    private List<GameObject> ammoBoxes = new List<GameObject>();
    private List<GameObject> bandages = new List<GameObject>();


    public void Initialize(DrunkardsWalk dwInstance)
    {
        drunkardsWalkInstance = dwInstance;
        if (drunkardsWalkInstance == null)
        {
            Debug.LogError("dw-instanz nicht zugewiesen");
            return;
        }
    }

    public void ClearPreviousSpawns()
    {
        foreach (var enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }

        spawnedEnemies.Clear();
        enemySpawnPositions.Clear();
        itemSpawnPositions.Clear();
    }

    public void DeleteCollectables()
    {
        foreach (var ammoBox in ammoBoxes)
        {
            Destroy(ammoBox);
        }

        foreach (var bandage in bandages)
        {
            Destroy(bandage);
        }
    }

    public void DespawnPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Destroy(player);
    }

    public void CollectSpawnPositions(int enemyCount, int ammoCount)
    {
        for (int x = 0; x < drunkardsWalkInstance.width; x++)
        {
            for (int y = 0; y < drunkardsWalkInstance.length; y++)
            {
                if (drunkardsWalkInstance.grid[x, y] == 0) // Überprüfe, ob die Position als begehbar markiert ist
                {
                    Vector3 position = GridToWorldPosition(x, y);
                    Vector3 spawnRadius = new Vector3(1.5f, 0, 1.5f);
                    Debug.Log("Überprüfte Position: (" + x + ", " + y + ") - Status: " + (drunkardsWalkInstance.grid[x, y] == 0 ? "Begehbar" : "Blockiert"));
                    //Debug.DrawLine(position, position + Vector3.up, Color.blue, 20f);
                    if (IsAreaClear(position, spawnRadius))
                    {
                        enemySpawnPositions.Add(position);
                        itemSpawnPositions.Add(position);
                        Debug.Log("Begehbare Position hinzugefügt: " + position);
                        //Debug.DrawLine(position, position + Vector3.up, Color.red, 20f);
                    }
                    else
                    {
                        Debug.Log("Position blockiert: " + position);
                    }
                }
            }
        }
    }

    private void ShuffleList(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void SpawnEnemies(int enemyCount)
    {
        levelManager.SetEnemyCount(enemyCount);

        for (int i = 0; i < enemyCount && enemySpawnPositions.Count > 0; i++)
        {
            if (enemySpawnPositions.Count == 0)
            {
                Debug.LogWarning("Keine verfügbaren Spawn-Positionen für weitere Gegner");
                break;
            }

            int randomIndex = Random.Range(0, enemySpawnPositions.Count);
            Vector3 enemyPos = enemySpawnPositions[randomIndex];
            enemyPos.y = 1.3f;
            GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);
            Debug.Log("Gegner gespawnt an: " + enemyPos);
            enemySpawnPositions.RemoveAt(randomIndex);
            enemy.GetComponent<Enemy>().levelManager = levelManager;
        }
    }

    public bool IsAreaClear(Vector3 position, Vector3 size)
    {
        Collider[] colliders = Physics.OverlapBox(position, size / 2, Quaternion.identity);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Wall"))
            {
                Debug.Log("Kollidiert mit " + collider.tag + " bei: " + position);
                return false;
            }
        }
        return true;
    }

    public void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            if (enemySpawnPositions.Count > 0)
            {
                int randomIndex = Random.Range(0, enemySpawnPositions.Count);
                Vector3 playerPos = enemySpawnPositions[randomIndex];

                player = Instantiate(playerPrefab, playerPos, Quaternion.identity);

                if (levelManager != null)
                {
                    Debug.Log("playercontroller wird levelmanager übergeben");
                    PlayerController playerController = player.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.InitializeCamera();
                        playerController.EnableMovement();
                    }
                }
                else
                {
                    Debug.LogError("levelmanager ist nicht gesetzt");
                }
                StartCoroutine(InitializeCamera(player));
            }
            else
            {
                Debug.Log("keine spawnposition mehr für den spieler verfügbar");
            }
        }
        else
        {
            Debug.LogError("Player-Prefab ist nicht zugewiesen!");
        }
    }

    IEnumerator InitializeCamera(GameObject player)
    {
        yield return new WaitForEndOfFrame();
        Camera mainCamera = player.GetComponentInChildren<Camera>();
        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(true);
            mainCamera.enabled = true;
            mainCamera.tag = "MainCamera";
            Debug.Log("main camera läuft: " + mainCamera.name);
        }
        else
        {
            Debug.LogError("Keine Kamera im Player-Objekt gefunden!");
        }
    }

    public void SpawnAmmo(int ammoCount)
    {
        for (int i = 0; i < ammoCount && itemSpawnPositions.Count > 0; i++)
        {
            if (itemSpawnPositions.Count == 0)
            {
                Debug.LogWarning("Keine verfügbaren Spawn-Positionen für mehr Munition");
                break;
            }
            int randomIndex = Random.Range(0, itemSpawnPositions.Count);
            Vector3 ammoPos = itemSpawnPositions[randomIndex];
            ammoPos.y = 0f;
            GameObject ammo = Instantiate(ammoPrefab, ammoPos, Quaternion.identity);
            ammoBoxes.Add(ammo);
            Debug.Log("ammo gespawnt an Position: " + ammoPos);
            itemSpawnPositions.RemoveAt(randomIndex);
        }
    }

    public void SpawnBandages(int bandagesCount)
    {
        for (int i = 0; i < bandagesCount && itemSpawnPositions.Count > 0; i++)
        {
            if (itemSpawnPositions.Count == 0)
            {
                Debug.LogWarning("Keine verfügbaren Spawn-Positionen für mehr bandagen");
                break;
            }
            int randomIndex = Random.Range(0, itemSpawnPositions.Count);
            Vector3 bandagePos = itemSpawnPositions[randomIndex];
            bandagePos.y = 0f;
            GameObject bandage = Instantiate(bandagePrefab, bandagePos, Quaternion.identity);
            bandages.Add(bandage);
            Debug.Log("bandage gespawnt an Position: " + bandagePos);
            itemSpawnPositions.RemoveAt(randomIndex);
        }
    }


    public IEnumerator CheckAllEnemiesDead()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            spawnedEnemies.RemoveAll(enemy => enemy == null);

            if (spawnedEnemies.Count == 0)
            {
                Debug.Log("level geschafft");
                yield break;
            }
        }
    }

    public Vector3 GridToWorldPosition(int gridX, int gridY)
    {
        return new Vector3(gridX + 0.5f, 0f, gridY + 0.5f);
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int gridX = Mathf.RoundToInt(worldPosition.x + 0.5f);
        int gridY = Mathf.RoundToInt(worldPosition.z + 0.5f);
        return new Vector2Int(gridX, gridY);
    }

}