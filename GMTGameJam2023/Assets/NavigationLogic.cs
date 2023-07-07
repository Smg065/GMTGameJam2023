using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationLogic : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform goalTransform;
    public bool debugCheckGoalPath;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(goalTransform.position);
    }
    void CheckGoalPath(Vector3 goalPosition)
    {
        NavMeshPath pathToGoal = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, goalPosition, 0, pathToGoal);
        Vector3[] goalCorners = pathToGoal.corners;
    }
}
