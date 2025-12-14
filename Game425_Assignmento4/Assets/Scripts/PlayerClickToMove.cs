using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The result of Research, ChatGPT, and experimenting. 

public class PlayerClickToMove : MonoBehaviour
{
    public GridManager gridManager;
    public Pathfinder pathfinder;

    public float moveSpeed = 4f;

    private List<GridNode> path;
    private int pathIndex = 0;

    void Update()
    {
        HandleClick();
        FollowPath();
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GridNode start = gridManager.WorldToNode(transform.position);
                GridNode goal = gridManager.WorldToNode(hit.point);

                path = pathfinder.FindPath(start, goal);
                pathIndex = 0;

                if (path != null)
                    DebugDrawPath(path);
            }
        }
    }

    void FollowPath()
    {
        if (path == null || pathIndex >= path.Count)
            return;

        Vector3 targetPos = path[pathIndex].worldPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            pathIndex++;
    }

    void DebugDrawPath(List<GridNode> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].worldPosition, path[i + 1].worldPosition, Color.green, 2f);
        }
    }
}
