using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFactory{
    
    public void CreatePlant(string plantName) {
        Plant newPlant = new Plant();

        switch (plantName) {
            case "Clivia":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);               
                break;
            case "Convallaria Majalis":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Hyacinthus Orientalis":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Viola Tricolor":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Pear Blossom":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Rhododendron":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Lotus":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Validus":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Rose":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Yellow Iris":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Pickerelweed":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Chrysanthemum":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Melastoma":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Begonia":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Hibiscus":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Water Hyacinth":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Tulipa":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Poinsettia":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Camellia":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Narcissus":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Jatropha Pandurifolia":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Rafflesia Arnoldii":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
            case "Yellow Water Lily":
                //Code to create plant
                newPlant = InitilisePlant(newPlant);
                break;
        }
    }

    private Plant InitilisePlant(Plant newPlant) {
        newPlant.SetName("");
        newPlant.SetDescription("");
        newPlant.SetCost(0f);
        newPlant.SetQuality(0f);
        //newPlant.SetGrowthStages();
        newPlant.SetOwner("");
        newPlant.SetGrowth(0f);
        newPlant.SetWater(0f);
        newPlant.SetFertiliser(0f);
        newPlant.SetValue(0f);
        newPlant.SetEmotion(Emotion.Happy);

        return newPlant;
    }
}
