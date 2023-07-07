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
    public static int[] floorHeights = { 4, 8 };
    public static int[] doorHeights = {  };
    public static int[] stairHeights = {  };
    public Transform floor;

    // Start is called before the first frame update
    void Start()
    {
        floor = Instantiate(floorCellPrefab, new Vector3(0, 0, 0), Quaternion.identity).transform;
        floor.localScale = new Vector3(56, 1, 56);
        for (int x = -3; x < 3; x++)
        {
            for (int z = -3; z < 3; z++)
            {
                for (int y = 0; y < floorHeights.Length; y++)
                {
                    floors[y] = Instantiate(floorCellPrefab, new Vector3(x * 8, floorHeights[y], z * 8), Quaternion.identity).transform;
                }
            }
        }
        for (int i = 0; i < doors.Length; i++)
        {
            //doors[i] = Instantiate(doorwayPrefab, new Vector3(i*2, 0, 0), Quaternion.identity).transform;
        }
        //doors[0].position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
