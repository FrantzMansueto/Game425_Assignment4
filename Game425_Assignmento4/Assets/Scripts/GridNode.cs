using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The result of Research, ChatGPT, and experimenting.

// Removed Monobehavior part because it was causing issues. - Matthew
public class GridNode
{
    public bool walkable;
    public Vector2Int gridPos;
    public Vector3 worldPosition;
    
    // A* values
    public float g;
    public float h;
    public float f;
    public GridNode parent;

    public GridNode(bool walkable, Vector2Int gridPos, Vector3 worldPosition)
    {
        this.walkable = walkable;
        this.gridPos = gridPos;
        this.worldPosition = worldPosition;
        g = float.MaxValue; // was g = 0;
        h = 0;
        f = 0;
        parent = null;
    }
}
