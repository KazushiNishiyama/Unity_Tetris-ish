using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour
{
    Material material;


    void Start()
    {
        if (material == null) Initialize();
    }

    private void Initialize()
    {
        material = new Material(Shader.Find("Standard"));
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = material;
    }

    public void SetState(Color color)
    {
        if(material == null) Initialize();
        material.color = color;
    }
}
