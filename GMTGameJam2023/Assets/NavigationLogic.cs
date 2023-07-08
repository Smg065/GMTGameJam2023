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
    public Transform checkingDoor;
    public Camera viewCam;
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
    //See if you can walk there without needing to open doors
    public void CheckGoalPath(Vector3 goalPosition)
    {
        NavMeshPath pathToGoal = new NavMeshPath();
        NavMesh.SamplePosition(goalPosition, out NavMeshHit closestPoint, 100, 1);
        NavMesh.CalculatePath(transform.position, closestPoint.position, 1, pathToGoal);
        currentGoalPos = goalTransform.position;
    }
    public bool CloseEnough(Vector3 goalPosition, float minDistance)
    {
        NavMesh.SamplePosition(goalPosition, out NavMeshHit closestPoint, 100, 1);
        return (closestPoint.position - (transform.position - transform.up)).magnitude <= minDistance;

    }
}
