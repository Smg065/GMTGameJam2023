using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFitter : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 scaleMulti;
    void Start()
    {
        Material targetMaterial = GetComponent<MeshRenderer>().material;
        targetMaterial.mainTextureScale = new Vector2(transform.lossyScale.x * scaleMulti.x, transform.lossyScale.y * scaleMulti.y);
    }
}
