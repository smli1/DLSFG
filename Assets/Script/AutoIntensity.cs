using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoIntensity : MonoBehaviour {

    public Gradient nightDayColor;

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float nightLightMinIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;


    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public float dayAtmosphereThickness = 0.4f;
    public float nightAtmosphereThickness = 0.87f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;

    float skySpeed = 1;


    Light mainLight;
    Skybox sky;
    Material skyMat;

    public Light nightLight;

    void Start() {

        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;

    }

    void Update() {

        //Calculating new intensity using dot product of mainlight and downwards vector
        float tRange = 1 - minPoint;
        float dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
        float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        //Setting new intensity
        mainLight.intensity = i;
        if(i >= nightLightMinIntensity)
            nightLight.intensity = i;

        //Calculating new ambient intensity
        tRange = 1 - minAmbientPoint;
        dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        RenderSettings.ambientIntensity = i;

        //Setting ambient intensity
        mainLight.color = nightDayColor.Evaluate(dot);
        nightLight.color = nightDayColor.Evaluate(dot);
        RenderSettings.ambientLight = mainLight.color;

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        //Calculating new atmosphere thickness
        i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat("_AtmosphereThickness", i);

        if (dot > 0)
            transform.Rotate(dayRotateSpeed * Time.deltaTime * skySpeed);
        else
            transform.Rotate(nightRotateSpeed * Time.deltaTime * skySpeed);
        
        //Keyboard controls for testing purposes
        if (Input.GetKeyDown(KeyCode.Q)) skySpeed *= 0.5f;
        if (Input.GetKeyDown(KeyCode.E)) skySpeed *= 2f;
    }
}
