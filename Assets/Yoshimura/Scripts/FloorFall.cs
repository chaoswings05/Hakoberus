using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFall : MonoBehaviour
{
   

    [SerializeField]
    int count;

    //乗れるハコベロスの数を設定してください
    [SerializeField]
    int fallcount;


    void Update()
    {
        
        if(fallcount==count)
        {
           
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        //PlayerかSubタグにあったらカウントを増やす
        //Subタグは赤ハコベロスのこと
        if (other.tag == "Player" || other.tag == "Sub")
        {
            count++;

        }
    }

    void OnTriggerExit(Collider other)
    {
        //PlayerかSubタグが離れたらカウントを減らす
        if (other.tag == "Player" || other.tag == "Sub")
        {
            count--;
           
        }
    }
}
