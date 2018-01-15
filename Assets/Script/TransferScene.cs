using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            GameObject temp = GameObject.FindGameObjectWithTag("Player");
            
            SceneManager.MoveGameObjectToScene(temp, SceneManager.GetSceneAt(1));
            SceneManager.LoadSceneAsync(1);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //SceneManager.LoadSceneAsync(0);
        }
    }
}
