using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PlayerController>().followingEnemy.Count == 0)
        {
            GameClear();
        }     
    }

    private void GameClear()
    {
        SceneManager.LoadScene("GameClear");
    }
}
