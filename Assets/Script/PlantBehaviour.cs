using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour {

    public Plant plant;

    private float[] growingTimes = new float[4];
    private float timeOfPlanting;

    private AutoIntensity theSun;

    // Use this for initialization
    void Start() {
        GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animators/" + plant.GetName()) as RuntimeAnimatorController;

        theSun = GameObject.FindGameObjectWithTag("TheSun").GetComponent<AutoIntensity>();
        timeOfPlanting = theSun.GetCurrentTime();

        CalculateTimeofGrowing();
    }
	// Update is called once per frame
	void Update () {
		if(theSun.GetCurrentTime() >= growingTimes[plant.GetGrowth()] && plant.GetHarvestable() == false){
            plant.IncrementGrowthStage();
            GetComponent<Animator>().SetInteger("Stage", plant.GetGrowth());
            if(plant.GetGrowth() == 3) {
                plant.SetHarvestable(true);
            }
            
            Debug.Log("Plant has grown: " + plant.GetGrowth());
        }
	}

    //Getter and Setter for the plant object
    public void SetPlant(Plant newPlant) {
        plant = newPlant;
        //Debug.Log(plant.GetGrowthStages()[0]);
    }
    public Plant GetPlant() {
        return plant;
    }

    public void CalculateTimeofGrowing() {
        for (int i = 0; i < growingTimes.Length; i++) {
            growingTimes[i] = timeOfPlanting + plant.GetGrowthStages()[i];

            Debug.Log(growingTimes[i]);
        }
    }
}