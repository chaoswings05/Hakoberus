using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestScript : MonoBehaviour
{
    [SerializeField]
    float _speed =1.0f;

    private Rigidbody rb;

    private Vector3 playerPos;

    private float actionPush = 0f,
                  push1 = 3;

    public EnemyActionScript enemyAct;

    bool start = false;

    void Start()
    {
        playerPos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {


        // 前
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector3(0, 0, _speed);
        }

        // 後
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector3(0, 0, -_speed);
        }

        // 右
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector3(_speed, 0, 0);

        }

        // 左
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector3(-_speed, 0, 0);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) ||
        Input.GetKeyUp(KeyCode.DownArrow) ||
        Input.GetKeyUp(KeyCode.RightArrow) ||
        Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.zero;
        }

        Vector3 diff = transform.position - playerPos;

        if(diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff);
        }

        playerPos = transform.position;

        if(start)
        {
            enemyAct.BridgeAction();
        }

    }

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



