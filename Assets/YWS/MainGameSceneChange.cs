using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneChange : MonoBehaviour
{
    [SerializeField] private PlayerController player = null;

    [SerializeField, Header("�p�[�e�B�N��")]
    GameObject cracker;

    [SerializeField, Header("�����ꏊ")]
    GameObject particleArea;

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
    }

    IEnumerator ParticlStart()
    {
        Instantiate(cracker, particleArea.transform.position, Quaternion.identity);

        Debug.Log("�Đ�");

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("GameClear");

    }
}
