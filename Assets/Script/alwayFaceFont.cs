using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alwayFaceFont : MonoBehaviour {
    Vector3 v ;
    
	void Awake () {
        v = transform.position - Camera.main.transform.position;
        v.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(v);
        //Camera.main.transform.parent = gameObject.transform;
    }
}
