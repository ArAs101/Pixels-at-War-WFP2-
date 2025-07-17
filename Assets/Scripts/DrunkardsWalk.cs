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
    public Transform wallParent;

    void Start()
    {
        GenerateGrid();
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

        wallParent = new GameObject("WallParent").transform;
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
                    GameObject wall = Instantiate(newWall, pos, Quaternion.identity, wallParent);
                    //Instantiate(newWall, pos, Quaternion.identity);
                }
            }
        }
        wallParent.gameObject.AddComponent<CombineWalls>().Combine();
    }
}
