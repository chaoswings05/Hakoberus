using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("hit");

        //�����S�[���I�u�W�F�N�g�̃R���C�_�[�ɐڐG�������̏����B
        if (other.tag == "Player")
        {
            //Goal�����Ƃ��Ƀ��O�ɏo��
            Debug.Log("Goal");
           
            //�L�����N�^�[���\���ɂ��鏈��
            other.gameObject.SetActive(false);
        }
    }
}
