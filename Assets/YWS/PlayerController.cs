using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] private float speed = 2f;
    [SerializeField, Header("回転速度")] private float rotSpeed = 30f;
    [SerializeField, Header("跳躍力")] private float jumpPower = 40f;
    [SerializeField, Header("重力の値")] private float gravSpeed = 1f;
    //今現在追従している赤ハコベロスのリスト
    public List<Enemy> followingEnemy = new List<Enemy>();
    //ピクミンが積み上げる場所の取得
    private Transform pileUpPos = null;
    //Rigidbodyの取得
    [SerializeField] private Rigidbody rb = null;
    private bool CanAction = false;
    private float actionPush = 0f;
    [SerializeField, Header("アクションの必要時間")] private float actionTime = 3f;
    private int actionCost = 0;
    [SerializeField] private float enemyY = 0.5f;
    [SerializeField] private float enemyZ = 0.75f;
    private bool InAction = false;
    
    void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //Spaceキーを押したらジャンプする
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            //ジャンプ
            rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }*/

//Playerの移動-------------
        //Playerの座標を計算
        Vector3 direction = transform.position + new Vector3(x,0,z) * speed;
        //移動下方向にPlayerの向きを変更する
        transform.LookAt(direction);
        rb.velocity = (new Vector3(x,0,z) * speed + Vector3.down * gravSpeed);
//-------------
    }
    
    //Colliderになにか当たったら
    private void OnTriggerEnter(Collider other)
    {
        //PlayerがActionPointに入ったら
        if(other.tag == "ActionPoint")
        {
            Debug.Log("範囲内に入りました");
            CanAction = true;
            if (other.gameObject.GetComponent<ActionArea>().IsPileUp)
            {
                pileUpPos = other.gameObject.GetComponent<ActionArea>().targetPoint;
                actionCost = other.gameObject.GetComponent<ActionArea>().needNum;
            }
        }

        if (other.tag == "Bone")
        {
            Debug.Log("骨を拾いました");
            other.tag = "Untagged";
            if (followingEnemy.Count > 0)
            {
                followingEnemy[0].bone = other.gameObject.GetComponent<Bone>();
                followingEnemy.RemoveAt(0);
            }
        }

        if (other.tag == "Goal" && followingEnemy.Count == 0)
        {
            //Goalしたときにログに出す
            Debug.Log("Goal");
           
            //キャラクターを非表示にする処理
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButton("DS4x") || Input.GetKey(KeyCode.Space))
        {
            if (CanAction)
            {
                // 秒数を数える
                actionPush += Time.deltaTime;

                // 3秒経ったら
                if (actionPush >= actionTime && !InAction)
                {
                    Debug.Log("3秒経過");
                    StartCoroutine(EnemyHighMove());
                    other.gameObject.GetComponent<ActionArea>().IsCostPayed = true;
                }
            }
        }
        else if(!Input.GetButtonUp("DS4x") || !Input.GetKeyUp(KeyCode.Space))
        {
            actionPush = 0;
            InAction = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ActionPoint")
        {
            Debug.Log("範囲内から離れました");
            CanAction = false;
            actionCost = 0;
        }
    }

    IEnumerator EnemyHighMove()
    {
        InAction = true;
        // 簡単な表記にするために
        Vector3 h_brockPos = pileUpPos.transform.position;

        // エネミーの移動

        // enemyのスクリプトとAIコンポーネントをfalse
        foreach (Enemy obj in followingEnemy)
        {
            obj.GetComponent<Enemy>().follow = false;
            obj.GetComponent<NavMeshAgent>().enabled = false;
        }

        for (int i = 0; i < actionCost; i++)
        {
            //followingEnemy[i].GetComponent<Enemy>().follow = false;
            followingEnemy[i].transform.position = new Vector3(h_brockPos.x, h_brockPos.y + enemyY * i, h_brockPos.z - enemyZ);
        }
        Debug.Log("アクション");
        followingEnemy.RemoveRange(0,actionCost);

        yield return new WaitForSeconds(3f);

        // Plyerと赤ハコベロスの障害物上に移動
        this.transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);
        foreach (Enemy obj in followingEnemy)
        {
            obj.transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);
        }

        //yield return new WaitForSeconds(1f);

        // 外したコンポーネントを再度ON
        foreach (Enemy obj in followingEnemy)
        {
            obj.GetComponent<Enemy>().follow = true;
            obj.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
    
    [SerializeField] Camera mainCamera = null;
    private void LateUpdate()
    {
        mainCamera.transform.LookAt(transform.position);
    }
}