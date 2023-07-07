using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianScript : NavigationLogic
{
    public Transform staticWanderCenter;
    public float waitTime;
    public void MoveCivilian(float excessTime)
    {
        Vector3 p = Random.insideUnitSphere * 5;
        p.y = 0;
        goalTransform.position = staticWanderCenter.position + p;
        waitTime = excessTime + 5f;
    }
    void Start()
    {
        MoveCivilian(0f);
    }

    void Update()
    {
        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            MoveCivilian(waitTime);
        }
    }
}
