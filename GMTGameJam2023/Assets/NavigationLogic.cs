using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationLogic : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform goalTransform;
    public Vector3 currentGoalPos;
    public bool debugCheckGoalPath;
    public LayerMask doorMask;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    public void Update()
    {
        if (debugCheckGoalPath) CheckGoalPath(goalTransform.position);
        agent.SetDestination(currentGoalPos);
    }
    void CheckGoalPath(Vector3 goalPosition)
    {
        NavMeshPath pathToGoal = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, goalPosition, 1, pathToGoal);
        Vector3[] goalCorners = pathToGoal.corners;
        Debug.Log(pathToGoal.corners.Length);
        for (int eachCorner = 1; eachCorner < goalCorners.Length; eachCorner++)
        {
            if (Physics.Linecast(goalCorners[eachCorner - 1] + Vector3.up, goalCorners[eachCorner] + Vector3.up, out RaycastHit foundDoor, doorMask, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("Door Found!");
                currentGoalPos = foundDoor.point - ((goalCorners[eachCorner] - goalCorners[eachCorner - 1]).normalized * 1.5f);
                return;
            }
        }
        currentGoalPos = goalTransform.position;
    }
}
