using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    

    private int counter = 0;

    // �X�e�[�W�ŃA�N�V�����{�^����������
    [SerializeField]
    private int Goalcount = 0;

    bool Push = false;

    
    void Update()
    {

       
        //space�̓A�N�V�����{�^���̃L�[�����Ă�������
            if (Input.GetKey("space"))
            {
                if (Push == false)  
                {
                Debug.Log("�����ꂽ");
                counter++;
                Push = true;   
                }
            }
            else
            {
        
            Push = false;
           
            
        }

           
        
    }
      

    private void OnTriggerEnter(Collider other)
    {

        // Debug.Log("hit");


        if (counter >= Goalcount)
        {
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
    
}
