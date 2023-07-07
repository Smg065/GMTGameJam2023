using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public Vector2 floorSize;
    public int totalFloors;
    public GameObject doorwayPrefab;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject ceilingPrefab;
    public GameObject stairsPrefab;
    public GameObject floorCellPrefab;
    public GameObject stairCellPrefab;
    public Transform[] doors;
    public Transform[] walls;
    public Transform[] floors;
    public Transform[] stairs;
    public static int[] wallHeights = { };
    public static int[] floorHeights = { 0, 4, 8 };
    public static int[] doorHeights = {  };
    public static int[] stairHeights = {  };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i] = Instantiate(floorPrefab, new Vector3(0, floorHeights[i], 0), Quaternion.identity).transform;
            floors[i].localScale = new Vector3(50, 1, 50);
        }
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i] = Instantiate(doorwayPrefab, new Vector3(i*2, 0, 0), Quaternion.identity).transform;
        }
        //doors[0].position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
