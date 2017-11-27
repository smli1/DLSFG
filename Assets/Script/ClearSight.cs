using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSight : MonoBehaviour {

    public float DistanceToPlayer = 5.0f;

    public Material standardTree;
    public Material transparentTree;

    public Material standardBush;
    public Material transparentBush;

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
            if (hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Field") {
                AutoTransparent AT = R.GetComponent<AutoTransparent>();
                if (AT == null) {
                    AT = R.gameObject.AddComponent<AutoTransparent>();
                }

                if (R.gameObject.tag == "Tree") {
                    AT.m_oldMaterial = standardTree;
                    AT.m_newMaterial = transparentTree;
                } else if(R.gameObject.tag == "Bush"){
                    AT.m_oldMaterial = standardBush;
                    AT.m_newMaterial = transparentBush;
                }

                AT.BeTransparent();
            }
        }
    }
}
