using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant{

    public string m_name;
    private string m_description;
    private float m_cost;
    private float m_quality;
    private int[] m_growthStages;

    private string m_owner;

    private int m_growth;
    private float m_water;
    private float m_fertiliser;
    private float m_value;
    private Emotion m_emotion;

    public Plant(PlantBuilder builder) {
        m_name = builder.m_name;
        m_description = builder.m_description;
        m_cost = builder.m_cost;
        m_quality = builder.m_quality;
        m_growthStages = builder.m_growthStages;
        m_owner = builder.m_owner;
        m_growth = builder.m_growth;
        m_water = builder.m_water;
        m_emotion = builder.m_emotion;
    }

    //Getters and Setters
    public void SetName(string name) {
        m_name = name;
    }
    public void SetDescription(string description) {
        m_description = description;
    }
    public void SetCost(float cost) {
        m_cost = cost;
    }
    public void SetQuality(float quality) {
        m_quality = quality;
    }
    public void SetGrowthStages(int[] growthStages) {
       m_growthStages = growthStages;
    }
    public void SetOwner(string owner) {
        m_owner = owner;
    }
    public void SetGrowth(int growth) {
        m_growth = growth;
    }
    public void SetWater(float water) {
        m_water = water;
    }
    public void SetFertiliser(float fertiliser) {
        m_fertiliser = fertiliser;
    }
    public void SetValue(float value) {
        m_value = value;
    }
    public void SetEmotion(Emotion emotion) {
        m_emotion = emotion;
    }

    public string GetName() {
        return m_name;
    }
    public string GetDescription() {
        return m_description;
    }
    public float GetCost() {
        return m_cost;
    }
    public float GetQuality() {
        return m_quality;
    }
    public int[] GetGrowthStages() {
        return m_growthStages;
    }
    public string GetOwner() {
        return m_owner;
    }
    public int GetGrowth() {
        return m_growth;
    }
    public float GetWater() {
        return m_water;
    }
    public float GetFertiliser() {
        return m_fertiliser;
    }
    public float GetValue() {
        return m_value;
    }
    public Emotion GetEmotion() {
        return m_emotion;
    }

    public void IncrementGrowthStage() {
        if(m_growth < 2) {
            m_growth++;
        }
    }
}