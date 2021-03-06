﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBuilder{

    public string m_name;
    public string m_description;
    public float m_cost;
    public float m_quality;
    public int[] m_growthStages;

    public string m_owner;

    public int m_growth;
    public float m_water;
    public float m_fertiliser;
    public float m_value;
    public Emotion m_emotion;
    public bool m_harvestable;

    public PlantBuilder(string name) {
        if (name != null)
            m_name = name;
        else
            Debug.LogError("Plant name cannot be null!");
    }

    public PlantBuilder SetUniqueValues() {
        JSONParser parser = JSONParser.Instance();

        m_description = parser.GetDescription(m_name);
        m_cost = parser.GetCost(m_name);
        m_quality = parser.GetQuality(m_name);
        m_growthStages = parser.GetGrowthStage(m_name);

        return this;
    }

    public PlantBuilder SetCommonValues() {

        m_owner = "";
        m_growth = 0;
        m_water = 100f;
        m_value = m_cost;
        m_emotion = Emotion.Happy;
        m_harvestable = false;

        return this;
    }

    public Plant Build() {
        return new Plant(this);
    }
}
