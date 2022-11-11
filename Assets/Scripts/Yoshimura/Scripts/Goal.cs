using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    //プレイヤーのオブジェクト
    public GameObject BlueHakoberos;
   
    

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("hit");

        //もしゴールオブジェクトのコライダーに接触した時の処理。
        if (other.name == BlueHakoberos.name)
        {
            //Goalしたときにログに出す
            Debug.Log("Goal");
           
                //キャラクターを非表示にする処理
                BlueHakoberos.SetActive(false);
            
        }
    }
}
