using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += Vector3.up * Time.deltaTime * 720;
    }
}
