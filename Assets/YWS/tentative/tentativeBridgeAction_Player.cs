using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentativeBridgeAction_Player : MonoBehaviour
{
    private Animator anim;
    private float actionPush = 0f,
                  push1 = 3;
    public tentativeBridgeAction_Enemy enemyAct;
    public bool start = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

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
}