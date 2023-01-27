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
            SoundManager.Instance.PlaySE(1);
            GameClear();
        }     
    }

    private void GameClear()
    {
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("GameClear");
    }
}
