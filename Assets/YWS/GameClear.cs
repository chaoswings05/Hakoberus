using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneChange_MainGameToGameClear();
        }     
    }

    private void SceneChange_MainGameToGameClear()
    {
        SceneManager.LoadScene("GameClear");
    }
}
