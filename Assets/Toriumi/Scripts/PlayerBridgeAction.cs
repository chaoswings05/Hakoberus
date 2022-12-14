using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBridgeAction : MonoBehaviour
{
    private Animator anim;

    private float actionPush = 0f,
                  push1 = 3;

    public EnemyActionScript enemyAct;

    private PlayerBridgeAction playerBri;

    bool start = false;
    void Start()
    {

        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (start)
        {
            enemyAct.BridgeAction();
            StartCoroutine(PlayerMove());
        }
    }

    IEnumerator PlayerMove()
    {
        anim.enabled = true;

        yield return new WaitForSeconds(5.0f);

        if (anim != null)
        {
            anim.SetBool("PlayerCross", true);
        }

        yield return new WaitForSeconds(7.0f);

        Destroy(anim);
        actionPush = 0;
        start = false;

    }

    // �g���K�[������Ė������A�N�V�����|�C���g����Ă�
    public void OnTriggerStay(Collider other)
    {
        Debug.Log("������");

        if (Input.GetKey(KeyCode.Space))
        {

            Debug.Log(actionPush);

            // �b���𐔂���
            actionPush += Time.deltaTime;

            // 3�b�o������
            if (actionPush >= push1)
            {
                Debug.Log("3�b�o��");
                start = true;

            }
            else
            {
                Debug.Log("3�b�Ȃ��A�e�L�X�g�\��");
            }

        }

        else if (!Input.GetKeyUp(KeyCode.Space))
        {
            actionPush = 0;

        }
    }
}
