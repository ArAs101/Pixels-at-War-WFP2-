using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkardsWalk : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 50;
    public int length = 50;
    int walkSteps = 3750;

    [Header("Tile Prefab")]
    public GameObject newWall;

    [Header("Plane Object")]
    public GameObject planeObject;

    private Vector3 planeSize;
    private Vector3 gridOffset;
    public int[,] grid;

    [Header("Top-Down Screenshot Camera")]
    public Camera topDownCamera;

    [Header("Visualisierung")]
    public GameObject tilePrefab;
    public Material wallMaterial;
    public Material floorMaterial;
    private Vector2Int startPos;

    void Start()
    {
        if (grid == null)
        {
            return;
        }

        if (planeObject != null)
        {
            planeSize = Vector3.Scale(planeObject.transform.localScale, new Vector3(10, 1, 10));
            width = Mathf.RoundToInt(planeSize.x);
            length = Mathf.RoundToInt(planeSize.z);
        }
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid = new int[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                grid[x, y] = 1;
            }
        }
    }

    public void GenerateDrunkardsWalk()
    {
        int x = width / 2;
        int y = length / 2;
        startPos = new Vector2Int(x, y);
        grid[x, y] = 0;

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

            grid[x, y] = 0;
        }

    }

    public void DrawDungeon()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                if (grid[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x + 0.5f, 0f, y + 0.5f);
                    Instantiate(newWall, pos, Quaternion.identity);
                }
            }
        }
    }

    public void VisualizeDungeonWithColorTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                Vector3 pos = new Vector3(x + 0.5f, 0f, y + 0.5f);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                tile.tag = "Tile";

                Renderer renderer = tile.GetComponent<Renderer>();

                if (grid[x, y] == 1 && wallMaterial != null)
                {
                    renderer.material = wallMaterial;
                }
                else if (grid[x, y] == 0 && floorMaterial != null)
                {
                    renderer.material = floorMaterial;
                }
                if (x == startPos.x && y == startPos.y)
                {
                    renderer.material.color = Color.gray;
                    //Debug.Log($"Startposition: ({startPos.x}, {startPos.y}) bei Gridgröße {width}×{length}");
                }
            }
        }
    }

    public IEnumerator CaptureTopDownScreenshot()
    {
        yield return new WaitForSeconds(0.2f);
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string folderPath = Application.dataPath + "/../Screenshots";
        string filename = folderPath + "/DungeonLayout_" + timestamp + ".png";

        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        ScreenCapture.CaptureScreenshot(filename);
        //Debug.Log("Screenshot gespeichert unter: " + filename);
    }

}
