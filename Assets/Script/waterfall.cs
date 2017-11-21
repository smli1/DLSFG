using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterfall : MonoBehaviour {

    Material material;
    // Use this for initialization
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset = new Vector2(material.mainTextureOffset.x, material.mainTextureOffset.y + Time.deltaTime * 2.5f);

    }
}
