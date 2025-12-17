Game425_Assignment4

Matthew Rust

Frantz Mansueto
PlayerClickToMove.cs

// Responsible of moving character based on A* script
   if (Input.GetMouseButtonDown(1))
   {
       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       if (Physics.Raycast(ray, out RaycastHit hit))
       {
           Debug.Log("Ray hit: " + hit.collider.name);

           GridNode startNode = gridManager.WorldToNode(transform.position);
           GridNode targetNode = gridManager.WorldToNode(hit.point);

// Responsible of rotation and animation when character is allowed to move due to A* script
    if (isMoving)
    {
        if (path == null || pathIndex >= path.Count)
        {
            isMoving = false;
            return;
        }

        Vector3 targetPos = path[pathIndex].worldPosition;
        targetPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        float distanceXZ = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(targetPos.x, targetPos.z)
        );

        if (distanceXZ < 0.05f)
        {
            pathIndex++;
        }
    }
}

Sam Dalton

Contributions:
  Sam
    I created three scripts called GridNode, PlayerClickToMove, and GridManager. I worked on the Pathfinder script and used the Manhattan heuristic (4-connected grid). Then I applied all the necessary elements to the scripts, except for GridNode. I made the GridNode script but forgot what I made it for and where to implement it. Right now, it’s just a script on hand, just in case.

  Matthew
  Modified PlayerClickToMove to work with animations. Added script to test and make sure the logic was working via wasd movement, transferred the logic to PlayerClickToMove and then deleted the wasd script entirely.
   Did further bug fixing to figure out what was wrong with the player movement. Which involved various modificaitons to the GridManager script.
