using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitRecolor : MonoBehaviour
{
    public MeshRenderer suitBody;
    public MeshRenderer suitBase;
    public MeshRenderer charBody;
    // Start is called before the first frame update
    void Start()
    {
        Color suitColor = Random.ColorHSV();
        suitBody.material.color = suitColor;
        suitBase.material.color = suitColor;
        charBody.material.color = Random.ColorHSV(0, 1, 0, .25f, .8f, 1);
    }
}
