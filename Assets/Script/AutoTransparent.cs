﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTransparent : MonoBehaviour {

    private Shader m_OldShader = null;
    private Color m_OldColor = Color.black;
    private Material m_oldMaterial = null;

    [SerializeField]
    private float m_Transparency = 0.3f;
    private const float m_TargetTransparancy = 0.6f;
    private const float m_FallOff = 0.1f;

    // Use this for initialization
    void Start () {
		
	}

    public void BeTransparent() {
        // reset the transparency;
        m_Transparency = m_TargetTransparancy;
        if (m_OldShader == null) {
            // Save the current shader
            m_oldMaterial = GetComponent<Renderer>().material;
            m_OldShader = GetComponent<Renderer>().material.shader;
            m_OldColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        }
    }

    // Update is called once per frame
    void Update () {
        if (m_Transparency <= 1.0f){
            Color C = GetComponent<Renderer>().material.color;
            C.a = m_Transparency;
            GetComponent<Renderer>().material.color = C;
        }else{
            Debug.Log("reset");

            // Reset the shader
            GetComponent<Renderer>().material.shader = m_OldShader;
            GetComponent<Renderer>().material.color = m_OldColor;
            GetComponent<Renderer>().material = m_oldMaterial;
            // And remove this script
            Destroy(this);
        }
        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;
    }
}
