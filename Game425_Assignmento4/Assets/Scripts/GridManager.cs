using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The result of Research, ChatGPT, and experimenting.

public class GridManager : MonoBehaviour
{
    public int width = 26;
    public int height = 20;
    public Transform labyrinthRoot;

    public GridNode[,] grid;
    Dictionary<Vector2Int, Transform> cellLookup;

    const int cubeSize = 4; // All cubes are 4x4

    void Start()
    {
        BuildCellLookup();
        BuildGrid();
    }

    void BuildCellLookup()
    {
        cellLookup = new Dictionary<Vector2Int, Transform>();

        foreach (Transform child in labyrinthRoot)
        {
            int x = Mathf.RoundToInt((child.position.x - 2) / cubeSize);
            int z = Mathf.RoundToInt((child.position.z - 2) / cubeSize);

            Vector2Int pos = new Vector2Int(x, z);
            if (!cellLookup.ContainsKey(pos))
                cellLookup.Add(pos, child);
        }
    }

    void BuildGrid()
    {
        grid = new GridNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector2Int gridPos = new Vector2Int(x, z);
                Vector3 worldPos = new Vector3(x * cubeSize + 2, 0, z * cubeSize + 2);

                bool walkable = false;
                if (cellLookup.ContainsKey(gridPos))
                {
                    Transform cell = cellLookup[gridPos];
                    walkable = cell.name.Contains("Cube_White");
                }

                grid[x, z] = new GridNode(gridPos, worldPos, walkable);

                // Debug overlay
                Color debugColor = walkable ? Color.green : Color.red;
                Debug.DrawRay(worldPos + Vector3.up * 0.5f, Vector3.up * 0.5f, debugColor, 30f);
            }
        }

        int walkableCount = 0;
        foreach (var node in grid)
            if (node.walkable) walkableCount++;

        Debug.Log("Total walkable cells: " + walkableCount);
    }

    public GridNode WorldToNode(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - 2) / cubeSize);
        int z = Mathf.FloorToInt((worldPos.z - 2) / cubeSize);

        return grid[x, z];
    }

    public void ResetNodes()
    {
        foreach (GridNode n in grid)
        {
            n.g = float.MaxValue;
            n.h = 0;
            n.f = 0;
            n.parent = null;
        }
    }
}

