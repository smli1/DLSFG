using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour {

    private Plant plant;

    // Use this for initialization
    void Start() {
        GetComponent<Animator>().runtimeAnimatorController = Resources.Load(plant.GetName()) as RuntimeAnimatorController;
    }
	// Update is called once per frame
	void Update () {
		
	}

    //Getter and Setter for the plant object
    public void SetPlant(Plant newPlant) {
        plant = newPlant;
    }
    public Plant GetPlant() {
        return plant;
    }
}
