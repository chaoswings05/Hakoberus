using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneChange : MonoBehaviour
{
    [SerializeField] private PlayerController player = null;
    [SerializeField, Header("クリア演出エフェクト")] GameObject cracker;

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
        StartCoroutine(ParticlStart());
        SoundManager.Instance.StopBGM();
        //SceneManager.LoadScene("GameClear");
    }

    IEnumerator ParticlStart()
    {
        //Instantiate(cracker, transform.position, Quaternion.identity);

        Debug.Log("再生");

        yield return new WaitForSeconds(5f);

        //SceneManager.LoadScene("GameClear");

    }
}
