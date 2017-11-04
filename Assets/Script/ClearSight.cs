using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSight : MonoBehaviour {

    public float DistanceToPlayer = 5.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, transform.forward, DistanceToPlayer);

        foreach (RaycastHit hit in hits) {
            Renderer R = hit.collider.GetComponent<Renderer>();
            if (R == null)
                continue;


            AutoTransparent AT = R.GetComponent<AutoTransparent>();
            if (AT == null) 
            {
                AT = R.gameObject.AddComponent<AutoTransparent>();
            }
            AT.BeTransparent();
        }
    }
}
