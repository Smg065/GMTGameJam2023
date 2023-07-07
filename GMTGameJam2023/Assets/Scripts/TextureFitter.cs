using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material targetMaterial = GetComponent<MeshRenderer>().material;
        targetMaterial.mainTextureScale = new Vector2(transform.lossyScale.x, transform.lossyScale.y);
    }
}
