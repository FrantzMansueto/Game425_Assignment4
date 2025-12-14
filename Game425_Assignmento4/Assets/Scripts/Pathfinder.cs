using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The result of Research, ChatGPT, and experimenting.


public class Pathfinder : MonoBehaviour
{
    public GridManager gridManager;

    private GridNode[,] grid;
    private int width;
    private int height;

    public float moveSpeed = 4f; // All units can use this. Added.

    void Start()
    {
        grid = gridManager.grid;
        width = gridManager.width;
        height = gridManager.height;
    }

    public List<GridNode> FindPath(GridNode startNode, GridNode goalNode)
    {
        gridManager.ResetNodes();

        startNode.g = 0;
        startNode.h = Heuristic(startNode, goalNode);
        startNode.f = startNode.h;

        List<GridNode> openSet = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode current = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].f < current.f || 
                    (openSet[i].f == current.f && openSet[i].h < current.h))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == goalNode)
                return RetracePath(startNode, goalNode);

            foreach (GridNode neighbor in GetNeighbors(current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                float tentativeG = current.g + 1;

                if (tentativeG < neighbor.g)
                {
                    neighbor.g = tentativeG;
                    neighbor.h = Heuristic(neighbor, goalNode);
                    neighbor.f = neighbor.g + neighbor.h;
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    float Heuristic(GridNode a, GridNode b)
    {
        return Mathf.Abs(a.gridPos.x - b.gridPos.x) +
               Mathf.Abs(a.gridPos.y - b.gridPos.y);
    }

    List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();

        Vector2Int[] dirs = {
            new Vector2Int( 1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0,  1),
            new Vector2Int(0, -1)
        };

        foreach (var d in dirs)
        {
            int x = node.gridPos.x + d.x;
            int y = node.gridPos.y + d.y;

            if (x >= 0 && x < width && y >= 0 && y < height)
                neighbors.Add(grid[x, y]);
        }

        return neighbors;
    }

    List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();

        GridNode current = endNode;
        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Add(startNode);
        path.Reverse();
        return path;
    }
    
    
    // Generic Follow Path Function
    // Works for player and enemies [Added]

    public IEnumerator FollowPath(Transform mover, List<GridNode> path)
    {
        if (path == null || path.Count == 0)
            yield break;

        foreach (GridNode node in path)
        {
            Vector3 target = node.worldPosition;
            
            // move to the next node
            while (Vector3.Distance(mover.position, target) > 0.05f)
            {
                mover.position = Vector3.MoveTowards(mover.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}












// ************************************************************************
//       First try


// public class Node
// {
//     public bool walkable;
//     public Vector2Int gridPos;
//     public Vector3 worldPosition;
//
//     public float g; // cost from start to this node
//     public float h; // Heuristic estimate to goal
//     public float f; // f = g + h
//
//     public Node parent;
//
//     public Node(bool walkable, Vector2Int gridPos, Vector3 worldPosition)
//     {
//         this.walkable = walkable;
//         this.gridPos = gridPos;
//         this.worldPosition = worldPosition;
//     }
// }
//
//
// public class Pathfinder : MonoBehaviour
// {
//     private Node[,] grid;
//     private int width;
//     private int height;
//     
//         
//     //Unity compatible initializer
//     public void Initialize(Node[,] grid)
//     {
//         this.grid = grid;
//         width = grid.GetLength(0);
//         height = grid.GetLength(1);
//     }
//
//     // Main A* SEARCH
//     public List<Node> FindPath(Node startNode, Node goalNode)
//     {
//         ResetNodes();
//         
//         List<Node> openSet = new List<Node>(); // Nodes to explore
//         HashSet<Node> closedSet = new HashSet<Node>(); // Nodes fully explored
//         
//         openSet.Add(startNode);
//         
//         while (openSet.Count > 0)
//         {
//             // Step 1: pick the node with lowest f
//             Node current = openSet[0];
//
//             for (int i = 1; i < openSet.Count; i++)
//             {
//                 if (openSet[i].f < current.f || 
//                     (openSet[i].f == current.f && openSet[i].h < current.h))
//                 {
//                     current = openSet[i];
//                 }
//             }
//             
//             openSet.Remove(current);
//             closedSet.Add(current);
//             
//             // Step 2: Goal reached
//             if (current == goalNode)
//                 return RetracePath(startNode, goalNode);
//             
//                 
//             
//             // Step 3: Check neighbors
//             foreach (Node neighbor in GetNeighbors(current))
//             {
//                 if (!neighbor.walkable || closedSet.Contains(neighbor))
//                     continue;
//
//                 float tentativeG = current.g + 1; // fixed cost for grid
//                 
//                 //Adoption rule
//                 if (tentativeG < neighbor.g || !openSet.Contains(neighbor))
//                 {
//                     neighbor.g = tentativeG;
//                     neighbor.h = Heuristic(neighbor, goalNode);
//                     neighbor.f = neighbor.g + neighbor.h;
//                     neighbor.parent = current;
//                     
//                     if (!openSet.Contains(neighbor))
//                         openSet.Add(neighbor);
//                 }
//             }
//         }
//         
//        // No path exists
//        return null;
//     }
//     
//     // Manhattan heuristic (4-connected grid)
//     private float Heuristic(Node a, Node b)
//     {
//         return Mathf.Abs(a.gridPos.x - b.gridPos.x) + Mathf.Abs(a.gridPos.y - b.gridPos.y);
//     }
//     
//     // 4-direction neighbors
//     private List<Node> GetNeighbors(Node node)
//     {
//         List<Node> neighbors = new List<Node>();
//
//         Vector2Int[] dirs =
//         {
//             new Vector2Int(1, 0),
//             new Vector2Int(-1, 0),
//             new Vector2Int(0, 1),
//             new Vector2Int(0, -1)
//         };
//
//         foreach (var dir in dirs)
//         {
//             int x = node.gridPos.x + dir.x;
//             int y = node.gridPos.y + dir.y;
//             
//             if (x >= 0 && x < width && y >= 0 && y < height)
//                 neighbors.Add(grid[x, y]);
//         }
//         
//         return neighbors;
//     }
//     
//     // Builds the path after reaching the goal.
//     private List<Node> RetracePath(Node startNode, Node endNode)
//     {
//         List<Node> path = new List<Node>();
//
//         Node current = endNode;
//
//         while (current != startNode)
//         {
//             path.Add(current);
//             current = current.parent;
//         }
//         
//         path.Add(startNode);
//         path.Reverse();
//         return path;
//     }
//     
//     // Reset old A* data
//     private void ResetNodes()
//     {
//         foreach (Node n in grid)
//         {
//             n.g = 0;
//             n.h = 0;
//             n.f = 0;
//             n.parent = null;
//         }
//     }
// }
