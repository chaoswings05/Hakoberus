using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //横と縦方向のキー入力
    float x,z;
    //Playerの速度調整
    public float moveSpeed = 3;

    //RigidBodyを取得1
    Rigidbody rb;
    //アニメーターを取得1
    //Animator animator;

    // Start is called before the first frame update
    //アップデート関数の前に一度だけ実行される：
    void Start()
    {
        rb = GetComponent<Rigidbody>();//RigidBodyを取得2
        //animator = GetComponent<Animator>();//アニメーターを取得2
    }

    // Update is called once per frame
    //約0.02秒に一回実行される
    void Update()
    {
        //横方向のキー入力
        x = Input.GetAxisRaw("Horizontal");
        //縦方向のキー入力
        z = Input.GetAxisRaw("Vertical");

        //Spaceキーを押したら攻撃キー入力
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("攻撃!!");
            //animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        //自分の位置 + 速度
        Vector3 direction = transform.position + new Vector3(x,0,z) * moveSpeed;
        //移動した方向にPlayerの向きを変更する
        transform.LookAt(direction);
        //速度設定
        rb.velocity = new Vector3(x,0,z) * moveSpeed;//moveSpeedで速度の調整
        //animator.SetFloat("Speed", rb.velocity.magnitude);//Speedの中にrb.velocityの値を入れる
    }

    //なにかにぶつかったときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ぶつかったよ");
    }
}
