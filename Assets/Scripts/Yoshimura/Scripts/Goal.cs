using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    //�v���C���[�̃I�u�W�F�N�g
    public GameObject BlueHakoberos;
   
    

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("hit");

        //�����S�[���I�u�W�F�N�g�̃R���C�_�[�ɐڐG�������̏����B
        if (other.name == BlueHakoberos.name)
        {
            //Goal�����Ƃ��Ƀ��O�ɏo��
            Debug.Log("Goal");
           
                //�L�����N�^�[���\���ɂ��鏈��
                BlueHakoberos.SetActive(false);
            
        }
    }
}
