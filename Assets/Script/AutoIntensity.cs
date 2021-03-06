﻿using System.Collections;
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

    public float growthMultiplier = 0;
    public float timeOfDay = 0;
    private Vector3 rotationOfSun = Vector3.zero;

    public GameObject pin;

    public int day = 1;

    void Start() {

        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;

        rotationOfSun.x += 90;
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

        if (dot > 0){
            transform.Rotate(dayRotateSpeed * Time.deltaTime * skySpeed);
            rotationOfSun -= (dayRotateSpeed * Time.deltaTime * skySpeed);
        }
        else{
            transform.Rotate(nightRotateSpeed * Time.deltaTime * skySpeed);
            rotationOfSun -= (nightRotateSpeed * Time.deltaTime * skySpeed);
        }
        
        //Keyboard controls for testing purposes
        if (Input.GetKeyDown(KeyCode.O)) skySpeed *= 0.5f;
        if (Input.GetKeyDown(KeyCode.P)) skySpeed *= 2f;


        NormaliseRotation();
        growthMultiplier += (Mathf.Sin((Mathf.PI / 180) * rotationOfSun.x)+1)/2;
        timeOfDay = (rotationOfSun.x)/15;

        pin.transform.rotation = Quaternion.Euler(0, 0, -(rotationOfSun.x + 90));
    }

    public void NormaliseRotation(){
        if(rotationOfSun.x >= 360){
            rotationOfSun.x -= 360;
        }
    }

    public float GetCurrentTime(){
        return timeOfDay;
    }
}
