using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("hit");

        //もしゴールオブジェクトのコライダーに接触した時の処理。
        if (other.tag == "Player")
        {
            //Goalしたときにログに出す
            Debug.Log("Goal");
           
            //キャラクターを非表示にする処理
            other.gameObject.SetActive(false);
        }
    }
}
