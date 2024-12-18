using System.Collections.Generic;
using UnityEngine;

public class DrunkardsWalk : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 50;  // Breite des Spielfelds
    public int length = 50; // Höhe des Spielfelds
    public int walkSteps = 9000; // Anzahl der Schritte des Drunkard's Walk

    [Header("Tile Prefab")]
    public GameObject newWall;

    [Header("Plane Object")]
    public GameObject planeObject; // Referenz zur Plane, um die Skalierung zu verwenden

    private Vector3 planeSize;
    private Vector3 gridOffset;

    // 2D-Array zur Speicherung der Tiles (1 = Wand, 0 = kein Wand)
    public int[,] grid;

    void Start()
    {
        Debug.Log("start in dw");
        //grid = new int[width, height];
        GenerateGrid();
        if (grid == null)
        {
            Debug.LogError("grid ist null");
            return;
        }

        if (planeObject != null)
        {
            // Berechne die tatsächliche Grid-Größe basierend auf der Skalierung der Plane
            planeSize = Vector3.Scale(planeObject.transform.localScale, new Vector3(10, 1, 10));
            width = Mathf.RoundToInt(planeSize.x);
            length = Mathf.RoundToInt(planeSize.z);
        }
    }

    public void GenerateGrid()
    {
        // Initialisiere das Grid und setze alle Zellen auf 1 (Wand)
        grid = new int[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                grid[x, y] = 1; // 1 bedeutet Wand
            }
        }
        Debug.Log("generategrid vorbei");
        DungeonOutput();
    }

    public void DungeonOutput()
    {
        string formattedGrid = "";
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                // Verwenden eines festen Formats mit einer Breite von zwei Zeichen
                formattedGrid += (grid[x, y] == 1 ? "# " : ". ");
            }
            formattedGrid += "\n";  // Neue Zeile für die nächste Reihe
        }
    }


    public void GenerateDrunkardsWalk()
    {
        // Startposition in der Mitte des Spielfelds
        int x = width / 2;
        int y = length / 2;
        grid[x, y] = 0; // Startpunkt begehbar machen

        // Führe den Walk durch
        for (int i = 0; i < walkSteps; i++)
        {
            int direction = Random.Range(0, 4);

            switch (direction)
            {
                case 0: // Move up
                    y = Mathf.Clamp(y + 1, 1, length - 2);
                    break;
                case 1: // Move down
                    y = Mathf.Clamp(y - 1, 1, length - 2);
                    break;
                case 2: // Move right
                    x = Mathf.Clamp(x + 1, 1, width - 2);
                    break;
                case 3: // Move left                    
                    x = Mathf.Clamp(x - 1, 1, width - 2);

                    break;
            }

            // Markiere die aktuelle Position als begehbar
            grid[x, y] = 0;
        }

    }
    /*
        public void AddBoundaryWalls()
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, 0] = 1;
                grid[x, height - 1] = 1;
            }

            for (int y = 0; y < height; y++)
            {
                grid[0, y] = 1;
                grid[width - 1, y] = 1;
            }
        }*/

    public void DrawDungeon()
    {
        // Zeichne den Dungeon basierend auf dem Grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                if (grid[x, y] == 1) // Nur Wände aufstellen
                {
                    Vector3 pos = new Vector3(x + 0.5f, 0f, y + 0.5f);
                    Instantiate(newWall, pos, Quaternion.identity);
                }
            }
        }
    }
}
