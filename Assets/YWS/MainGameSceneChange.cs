using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneChange : MonoBehaviour
{
    [SerializeField] private PlayerController player = null;
    [SerializeField, Header("クリア演出エフェクト")] private GameObject cracker = null;
    [SerializeField, Header("エフェクト発生ポイント")] private Transform crackerPoint = null;
    [SerializeField] private ClearWindow clearWindow = null;

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
        Instantiate(cracker, crackerPoint);
        clearWindow.WindowDeployment();
    }
}
