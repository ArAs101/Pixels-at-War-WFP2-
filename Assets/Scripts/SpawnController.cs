using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject spikeyEnemyPrefab;
    public GameObject fireyEnemyPrefab;
    public GameObject ammoPrefab;
    public GameObject bandagePrefab;
    public GameObject coinPrefab;
    public DrunkardsWalk drunkardsWalkInstance;
    public LevelManager levelManager;
    public PlayerController playerController;
    public static GameObject player;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<Vector3> enemySpawnPositions = new List<Vector3>();
    private List<Vector3> itemSpawnPositions = new List<Vector3>();
    private List<GameObject> ammoBoxes = new List<GameObject>();
    private List<GameObject> bandages = new List<GameObject>();
    private List<GameObject> coins = new List<GameObject>();


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
                if (drunkardsWalkInstance.grid[x, y] == 0)
                {
                    Vector3 position = GridToWorldPosition(x, y);
                    Vector3 spawnRadius = new Vector3(1.5f, 0, 1.5f);
                    if (IsAreaClear(position, spawnRadius))
                    {
                        enemySpawnPositions.Add(position);
                        itemSpawnPositions.Add(position);
                    }                    
                }
            }
        }
    }

    public void SpawnEnemies(int enemyCount)
    {
        levelManager.SetEnemyCount(enemyCount);

        for (int i = 0; i < enemyCount && enemySpawnPositions.Count > 0; i += 2)
        {
            if (enemySpawnPositions.Count == 0)
            {
                Debug.LogWarning("Keine verfügbaren Spawn-Positionen für weitere Gegner");
                break;
            }

            int randomIndex = Random.Range(0, enemySpawnPositions.Count);
            Vector3 spikeyEnemyPos = enemySpawnPositions[0];
            spikeyEnemyPos.y = 1.3f;
            GameObject spikeyEnemy = Instantiate(spikeyEnemyPrefab, spikeyEnemyPos, Quaternion.identity);
            spawnedEnemies.Add(spikeyEnemy);
            
            spikeyEnemy.GetComponent<SpikeyEnemy>().levelManager = levelManager;

            Vector3 enemyPos = enemySpawnPositions[1];
            enemyPos.y = 1.3f;
            GameObject fireyEnemy = Instantiate(fireyEnemyPrefab, enemyPos, Quaternion.identity);
            spawnedEnemies.Add(fireyEnemy);
            enemySpawnPositions.RemoveAt(0);
            enemySpawnPositions.RemoveAt(1);
            fireyEnemy.GetComponent<FireyEnemy>().levelManager = levelManager;
        }
    }

    public bool IsAreaClear(Vector3 position, Vector3 size)
    {
        Collider[] colliders = Physics.OverlapBox(position, size / 2, Quaternion.identity);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Wall"))
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnPlayer()
    {
        int randomIndex = Random.Range(0, enemySpawnPositions.Count);
        if (playerPrefab != null && player == null)
        {
            if (enemySpawnPositions.Count > 0)
            {
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
            player.transform.position = enemySpawnPositions[randomIndex];
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
            GameObject ammo = Instantiate(ammoPrefab, ammoPos, Quaternion.identity);
            ammoBoxes.Add(ammo);
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
            itemSpawnPositions.RemoveAt(randomIndex);
        }
    }

    public void SpawnCoins(int coinsCount)
    {
        for (int i = 0; i < coinsCount && itemSpawnPositions.Count > 0; i++)
        {
            if (itemSpawnPositions.Count == 0)
            {
                Debug.LogWarning("Keine verfügbaren Spawn-Positionen für mehr bandagen");
                break;
            }
            int randomIndex = Random.Range(0, itemSpawnPositions.Count);
            Vector3 coinPos = itemSpawnPositions[randomIndex];
            coinPos.y = 0.3f;
            GameObject coin = Instantiate(coinPrefab, coinPos, Quaternion.identity);
            coins.Add(coin);
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