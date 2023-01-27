using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFall : MonoBehaviour
{
   

    [SerializeField]
    int count;

    //����n�R�x���X�̐���ݒ肵�Ă�������
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
        //Player��Sub�^�O�ɂ�������J�E���g�𑝂₷
        //Sub�^�O�͐ԃn�R�x���X�̂���
        if (other.tag == "Player" || other.tag == "Sub")
        {
            count++;

        }
    }

    void OnTriggerExit(Collider other)
    {
        //Player��Sub�^�O�����ꂽ��J�E���g�����炷
        if (other.tag == "Player" || other.tag == "Sub")
        {
            count--;
           
        }
    }
}
