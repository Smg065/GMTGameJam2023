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
    public float navRefreshRate;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    public void Update()
    {
        if (debugCheckGoalPath) CheckGoalPath(goalTransform.position);
        navRefreshRate -= Time.deltaTime;
        if (navRefreshRate <= 0)
        {
            agent.SetDestination(currentGoalPos);
            navRefreshRate += .25f;
        }
    }
    //See if you can walk there without needing to open doors
    public void CheckGoalPath(Vector3 goalPosition)
    {
        currentGoalPos = goalTransform.position;
    }
    public bool CloseEnough(Vector3 goalPosition, float minDistance)
    {
        NavMesh.SamplePosition(goalPosition, out NavMeshHit closestPoint, 100, 1);
        return (closestPoint.position - (transform.position - transform.up)).magnitude <= minDistance;

    }
}
