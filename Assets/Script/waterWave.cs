using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterWave : MonoBehaviour {

    Material material;
	// Use this for initialization
	void Start () {
        material = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        material.mainTextureOffset= new Vector2(material.mainTextureOffset.x + Mathf.Sin(Time.time) * Time.deltaTime / 50f, material.mainTextureOffset.y);
        material.SetTextureOffset("_DetailAlbedoMap", new Vector2(material.mainTextureOffset.x + Mathf.Cos(Time.time) * Time.deltaTime / 20f, material.mainTextureOffset.y));
    }
}
