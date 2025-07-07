using UnityEngine;
using System.Collections;

public class DungeonBatchGenerator : MonoBehaviour
{
    public DrunkardsWalk drunkardsWalk;
    int dungeonCount = 100;
    public SpawnController spawnController;

    void Start()
    {
        StartCoroutine(GenerateMultipleDungeons());
    }

    public IEnumerator GenerateMultipleDungeons()
    {
        for (int i = 0; i < dungeonCount; i++)
        {
            Debug.Log("Generiere Dungeon " + (i + 1));

            // Falls alte Tiles noch da sind --> löschen
            foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
            {
                Destroy(tile);
            }

            drunkardsWalk.GenerateGrid();
            drunkardsWalk.GenerateDrunkardsWalk();
            drunkardsWalk.VisualizeDungeonWithColorTiles();
            spawnController.ClearPreviousSpawns();
            spawnController.CollectSpawnPositions(10, 10);
            spawnController.SpawnAmmo(10);
            spawnController.SpawnBandages(10);
            spawnController.SpawnCoins(10);
            spawnController.SpawnEnemies(10);

            yield return new WaitForSeconds(0.5f);

            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string folderPath = Application.dataPath + "/../Screenshots";
            string filename = folderPath + "/DungeonLayout_" + (i + 1).ToString("D2") + ".png";

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            ScreenCapture.CaptureScreenshot(filename);
            Debug.Log("Screenshot gespeichert unter: " + filename);

            folderPath = Application.dataPath + "/../DungeonExports";
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            string layoutFilename = "DungeonLayout_" + (i + 1).ToString("D2") + ".txt";
            string path = System.IO.Path.Combine(folderPath, layoutFilename);

            int wallCount = 0;
            int floorCount = 0;

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
            {
                for (int y = drunkardsWalk.grid.GetLength(1) - 1; y >= 0; y--)
                {
                    for (int x = 0; x < drunkardsWalk.grid.GetLength(0); x++)
                    {
                        int cell = drunkardsWalk.grid[x, y];
                        writer.Write(cell);

                        if (cell == 1)
                            wallCount++;
                        else if (cell == 0)
                            floorCount++;
                    }
                    writer.WriteLine();
                }

                writer.WriteLine();
                writer.WriteLine("Wandzellen: " + wallCount);
                writer.WriteLine("Bodenzellen: " + floorCount);
            }


            Debug.Log("Dungeonlayout exportiert unter: " + path);

            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Alle Dungeons generiert.");
    }
}
