using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{

   /* private int counter = 0;

    [SerializeField]
    private int Goalcount = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            counter++;

        }

        
    }*/


    private void OnTriggerEnter(Collider other)
    {

        // Debug.Log("hit");


       // if (counter >= Goalcount)
       // {
            //�����S�[���I�u�W�F�N�g�̃R���C�_�[�ɐڐG�������̏����B
            if (other.tag == "Player")
            {
                //Goal�����Ƃ��Ƀ��O�ɏo��
                Debug.Log("Goal");

                //�L�����N�^�[���\���ɂ��鏈��
                other.gameObject.SetActive(false);
            }
        }
   // }
    
}
