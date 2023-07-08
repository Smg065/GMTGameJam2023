using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheifScript : NavigationLogic
{
    public int thiefMode;
    public bool searchOverride;
    public Transform goalTarget;
    public Transform exitTarget;
    public bool patrolFirstPoint;
    public float waitTime;
    new void Update()
    {
        if (CloseEnough(goalTransform.position, 1.5f))
        {
            //Wait for 3 seconds
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                waitTime += Random.Range(2, 4);
                //Once you're done waiting, move to the other point
                if (patrolFirstPoint) goalTransform.position = goalTarget.position;
                else goalTransform.position = exitTarget.position;
                patrolFirstPoint = !patrolFirstPoint;
                CheckGoalPath(goalTransform.position);
            }
        }
        base.Update();
    }
}
