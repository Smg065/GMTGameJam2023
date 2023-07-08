using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : NavigationLogic
{
    public int guardMode;
    public bool searchOverride;
    public Transform guardWaypoint1;
    public Transform guardWaypoint2;
    public Transform guardChaseTarget;
    public float waitTime;
    public bool patrolFirstPoint;

    // Update is called once per frame
    new void Update()
    {
        if (!searchOverride)
        {
            int guardModeToUse = guardMode;
            if (guardChaseTarget != null) guardModeToUse = 1;
            switch (guardModeToUse)
            {
                //Stand Guard
                case 0:
                    goalTransform.position = guardWaypoint1.position;
                    CheckGoalPath(goalTransform.position);
                    break;
                //Follow
                case 1:
                    goalTransform.position = guardChaseTarget.position;
                    goalTransform.position -= (guardChaseTarget.position - transform.position).normalized * 4;
                    CheckGoalPath(goalTransform.position);
                    break;
                //Patrol
                case 2:
                    //Move to the point
                    if (CloseEnough(goalTransform.position, 1.5f))
                    {
                        //Wait for 3 seconds
                        waitTime -= Time.deltaTime;
                        if (waitTime <= 0)
                        {
                            waitTime += Random.Range(2, 4);
                            //Once you're done waiting, move to the other point
                            if (patrolFirstPoint) goalTransform.position = guardWaypoint1.position;
                            else goalTransform.position = guardWaypoint2.position;
                            patrolFirstPoint = !patrolFirstPoint;
                            CheckGoalPath(goalTransform.position);
                        }
                    }
                    break;
                //Search
                case 3:
                    //Wait for a random amount of time seconds
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        //Once you're done waiting, move to another random point
                        waitTime += Random.Range((guardWaypoint1.position - guardWaypoint2.position).magnitude, (guardWaypoint1.position - guardWaypoint2.position).magnitude) * 2;
                        Vector3 randomPoint = Random.insideUnitSphere;
                        randomPoint.y = 0;
                        randomPoint = randomPoint.normalized * (guardWaypoint1.position - guardWaypoint2.position).magnitude;
                        goalTransform.position = guardWaypoint1.position + randomPoint;
                        CheckGoalPath(goalTransform.position);
                    }
                    break;
            }
        }
        else
        {
            //Move to the point
            CheckGoalPath(goalTransform.position);
            if (CloseEnough(goalTransform.position, 1.5f))
            {
                //Wait for 3 seconds
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    //Back to your guard duty
                    waitTime += Random.Range(2, 4);
                    searchOverride = false;
                }
            }
        }
        base.Update();
    }
}
