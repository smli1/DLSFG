using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTransparent : MonoBehaviour {

    public Material m_oldMaterial = null;
    public Material m_newMaterial = null;

    private float m_Transparency = 0.3f;
    private const float m_TargetTransparancy = 0.6f;
    private const float m_FallOff = 0.1f;

    // Use this for initialization
    void Start () {
        
    }

    public void BeTransparent() {
        // reset the transparency;
        m_Transparency = m_TargetTransparancy;
        //Debug.Log(m_oldMaterial.renderQueue);
    }

    // Update is called once per frame
    void Update () {
        if (m_Transparency <= 1.0f){
            GetComponent<Renderer>().material = m_newMaterial;
        }else{
            Debug.Log("reset");

            // Reset the shader
            GetComponent<Renderer>().material = m_oldMaterial;
            // And remove this script
            Destroy(this);
        }
        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;
    }
}
