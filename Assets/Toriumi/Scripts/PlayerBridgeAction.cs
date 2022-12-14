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

    // トリガーを作って無理やりアクションポイント作ってる
    public void OnTriggerStay(Collider other)
    {
        Debug.Log("入った");

        if (Input.GetKey(KeyCode.Space))
        {

            Debug.Log(actionPush);

            // 秒数を数える
            actionPush += Time.deltaTime;

            // 3秒経ったら
            if (actionPush >= push1)
            {
                Debug.Log("3秒経過");
                start = true;

            }
            else
            {
                Debug.Log("3秒ない、テキスト表示");
            }

        }

        else if (!Input.GetKeyUp(KeyCode.Space))
        {
            actionPush = 0;

        }
    }
}
