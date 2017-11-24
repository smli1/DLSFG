using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private string[,] playerBag = new string[6, 6];
    private int spaceInBag = 36;
    private int[] nullIndex = { -1, -1 };

    private Tool[] toolBelt = new Tool[] { Tool.Shovel, Tool.Dibber, Tool.WateringCan };
    private int currentTool = 0;

    private float moneyPouch = 500f;

    // Use this for initialization
    void Start() {
        playerBag[0, 0] = "Convallaria_Majalis/seed";
        playerBag[0, 1] = "Chrysanthemum/seed";
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.C)) {
            ChangeTool(true);
        }else if (Input.GetKeyDown(KeyCode.X)) {
            ChangeTool(false);
        }
	}

    /// <summary>
    /// Searching the bag for a given value
    /// </summary>
    /// <param name="value">Value to search for</param>
    /// <returns>Index of value</returns>
    public int[] SearchBag(string value) {
        bool stop = false;

        int[] index = new int[] { -1, -1 };
        //Loop through the bag searching for empty slots, incrementing 'space' each time one is found
        for(int i = 0; i < playerBag.GetLength(0) && !stop; i++) {
            for(int j = 0; j < playerBag.GetLength(1) && !stop; j++) {
                //Checking to see if the contents of the bag at the current index does not equal to null
                if (playerBag[i, j] != null) {
                    string[] tempValue = playerBag[i, j].Split('/');
                    //Split the value stored in the bag around '/' then loop through the values to compare next to the required values
                    for (int k = 0; k < tempValue.Length; k++) {
                        if (tempValue[k] == value) {
                            index[0] = i;
                            index[1] = j;

                            stop = true;
                            break;
                        }
                    }
                }
            }
        }
        return index;
    }

    /// <summary>
    /// Returning the name of the plant at a certain index
    /// </summary>
    /// <param name="index">Index of the plant</param>
    /// <returns>The name of the plant</returns>
    public string GetPlantName(int[] index) {
        string bagObject =  playerBag[index[0], index[1]];

        if(bagObject != "" && bagObject != null) {
            bagObject = bagObject.Split('/')[0];
        } else {
            bagObject = "";
        }

        return bagObject;
    }

    /// <summary>
    /// Public method for adding an item to the players bag
    /// </summary>
    /// <param name="item">Item to be added to the bag</param>
    public void AddItem(string item) {
        //Get index for next available space in the inventory
        int[] index = SearchBag(null);

        if (index != nullIndex) { 
            playerBag[index[0], index[1]] = item;
            spaceInBag--;
        }
    }

    /// <summary>
    /// Public method for removing an item from the players bag
    /// </summary>
    /// <param name="item">Item to be removed from the bag</param>
    public void RemoveItem(int[] index) {
        //Get index for the item in the bag
        //int[] index = SearchBag(item);

        if (index != nullIndex) {
            playerBag[index[0], index[1]] = null;
            spaceInBag++;
        }
    }

    /// <summary>
    /// Public method for switching two items in the inventory, will be used for drag and dropping item in the UI
    /// </summary>
    /// <param name="index1">Index of the first item</param>
    /// <param name="index2">Index of the second item</param>
    public void SwitchItems(int[] index1, int[] index2) {
        string tempItem = playerBag[index2[0], index2[1]];

        playerBag[index2[0], index2[1]] = playerBag[index1[0], index1[1]];
        playerBag[index1[0], index1[1]] = tempItem;
    }

    public int[] GetNullIndex() {
        return nullIndex;
    }

    /// <summary>
    /// Public method for accessing the current selected tool of the player
    /// </summary>
    /// <returns>The current tool</returns>
    public Tool CurrentTool() {
        return toolBelt[currentTool];
    }

    /// <summary>
    /// Private method for changing the selected tool
    /// </summary>
    /// <param name="up">Changing Up or down the tool belt</param>
    private void ChangeTool(bool up) {
        if (up) {
            currentTool++;
            if (currentTool == toolBelt.Length)
                currentTool = 0;
        } else {
            currentTool--;
            if (currentTool == -1)
                currentTool = toolBelt.Length - 1;
        }

        Debug.Log(toolBelt[currentTool]);
    }

    /// <summary>
    /// Public method for accessing the current balance of the players money pouch
    /// </summary>
    /// <returns>Current value of 'moneyPouch</returns>
    public float GetMoneyPouchBalance() {
        return moneyPouch;
    }

    /// <summary>
    /// Public method for adding funds to the players money pouch
    /// </summary>
    /// <param name="value">Value to be added to the money pouch</param>
    public void AddFunds(float value) {
        moneyPouch += value;
    }

    /// <summary>
    /// Public method for reducing the money in the money pouch by a given value
    /// </summary>
    /// <param name="value">Value to be removed from the money pouch</param>
    public void SpendMoney(float value) {
        moneyPouch -= value;
    }
}
