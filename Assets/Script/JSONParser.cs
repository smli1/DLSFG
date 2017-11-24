using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONParser{

    public static JSONParser parser;

    protected JSONParser() {
        ReadJson();
        Debug.Log("New instance made.");
    }

    public static JSONParser Instance(){
        if ( parser == null ){
            parser = new JSONParser();
        }

        return parser;
    }

    public Data flowers;

    [System.Serializable]
    public class Data {
        public Plants[] plants;
    }

    [System.Serializable]
	public struct Plants {
        public string name;
        public string description;
        public float cost;
        public float quality;
        public float[] growthstage;
    }
    
    public void ReadJson() {
        string filePath = Application.dataPath + "/Resources/plants.json";

        Debug.Log(filePath);

        if (File.Exists(filePath)) {
            string data = File.ReadAllText(filePath);
            flowers = new Data();

            flowers = JsonUtility.FromJson<Data>(data);
            Debug.Log(flowers.plants[0].name);
        } else {
            Debug.LogError("JSON File does not exist!");
        }
    }

    private Plants SearchData(string value) {
        Plants tempPlant = new Plants();

        for(int i = 0; i < flowers.plants.Length; i++) {
            if(flowers.plants[i].name == value) {
                tempPlant = flowers.plants[i];
            }
        }

        return tempPlant;
    }

    public string GetDescription(string value) {
        return SearchData(value).description;
    }

    public float GetCost(string value) {
        return SearchData(value).cost;
    }

    public float GetQuality(string value) {
        return SearchData(value).quality;
    }

    public float[] GetGrowthStage(string value) {
        return SearchData(value).growthstage;
    }
}
