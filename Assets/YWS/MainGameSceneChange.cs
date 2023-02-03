using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneChange : MonoBehaviour
{
    [SerializeField] private PlayerController player = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player.followingEnemy.Count == 0)
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
