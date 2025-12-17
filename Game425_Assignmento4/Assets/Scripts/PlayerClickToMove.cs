using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClickToMove : MonoBehaviour
{
    public GridManager gridManager;
    public Pathfinder pathfinder;
    public AudioSource errorAudio;
    public Animator animator;       // <-- add animator reference
    public float moveSpeed = 5f;

    private List<GridNode> path;
    private int pathIndex = 0;
    private bool isMoving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Ray hit: " + hit.collider.name);

                GridNode startNode = gridManager.WorldToNode(transform.position);
                GridNode targetNode = gridManager.WorldToNode(hit.point);

                if (!targetNode.walkable)
                {
                    Debug.Log("Clicked a Wall!");
                    TriggerError("Wall!");
                    return;
                }

                path = pathfinder.FindPath(startNode, targetNode);

                if (path == null || path.Count == 0)
                {
                    Debug.Log("Path is NULL or Empty!"); // A* failed
                    TriggerError("Unreachable!");
                }
                else
                {
                    Debug.Log("Path Found! Steps: " + path.Count);
                    pathIndex = 0;
                    isMoving = true;
                }
            }
            else
            {
                Debug.Log("Ray hit NOTHING. Check your floor colliders!");
            }
        }

        if (isMoving)
        {
            if (path == null || pathIndex >= path.Count)
            {
                isMoving = false;
                animator.SetFloat("Speed", 0f); // idle
                return;
            }

            Vector3 targetPos = path[pathIndex].worldPosition;
            targetPos.y = transform.position.y;

            // Direction vector
            Vector3 dir = targetPos - transform.position;

            // Move
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            animator.SetFloat("Speed", dir.magnitude);

            // Check if reached this node
            float distanceXZ = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(targetPos.x, targetPos.z)
            );

            if (distanceXZ < 0.05f)
            {
                pathIndex++;
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    void TriggerError(string message)
    {
        Debug.LogWarning(message);
        if (errorAudio != null) errorAudio.Play();
    }
}
