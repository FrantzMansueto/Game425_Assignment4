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

    void Start()
    {
        Debug.Log("GridManager Awake called");
        BuildCellLookup();
        BuildGrid();
    }

    void BuildCellLookup()
    {
        cellLookup = new Dictionary<Vector2Int, Transform>();

        foreach (Transform child in labyrinthRoot)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.z);
            
            Vector2Int pos = new Vector2Int(x, y);
            
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
                bool walkable = false;
                Vector3 worldPos = new Vector3(x, 0, z);
                
                Vector2Int pos = new Vector2Int(x, z);

                if (cellLookup.ContainsKey(pos))
                {
                    Transform cell = cellLookup[pos];
                    walkable = cell.name.Contains("Cube_White");
                }

                grid[x, z] = new GridNode(walkable, pos, worldPos);
            }
        }
    }

    public GridNode WorldToNode(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x);
        int y = Mathf.RoundToInt(worldPos.z);
        
        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);

        return grid[x, y];
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
