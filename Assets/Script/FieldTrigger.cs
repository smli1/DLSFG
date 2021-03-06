﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider col) {
        if(col.tag == "Player") {
            col.GetComponent<PlayerAction>().SetCanPlant(true);
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.tag == "Player") {
            col.GetComponent<PlayerAction>().SetCanPlant(false);
        }
    }
}
