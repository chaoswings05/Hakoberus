using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] private float speed = 2f;
    [SerializeField, Header("重力の値")] private float gravSpeed = 1f;
    public List<Enemy> followingEnemy = new List<Enemy>(); //プレイヤーが連れている赤ハコベロスのリスト
    private Transform pileUpPos = null; //赤ハコベロスをが積み上げる場所の
    [SerializeField] private Rigidbody rb = null; //PlayerのRigidbodyを取得
    private bool CanAction = false; //アクションを行える状態なのか
    private float actionPush = 0f; //アクションボタンを長押した時間
    [SerializeField, Header("アクションの必要時間")] private float actionTime = 3f;
    [SerializeField] private GaugeController gaugeController = null;
    private int actionCost = 0; //アクションに必要な赤ハコベロスの数
    private int PayedCost = 0;
    private bool InAction = false;
    private bool IsActionCharging = false;
    private bool IsActionConfirm = false;
    private bool IsEnemyMoving = false;
    private bool IsPileUp = false;
    private bool IsClimbing = false;
    private bool ClimbFinish = false;

    void Update()
    {
        SetEnemyNum();
        if (CanAction)
        {
            UpdateNotice();
        }

        if (IsActionConfirm && CheckIsEnemyActionFinish())
        {
            InAction = true;
            if (IsPileUp)
            {
                ClimbUp();
            }
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //Playerの移動
        if (!InAction)
        {
            //Playerの座標を計算
            Vector3 direction = transform.position + new Vector3(x,0,z) * speed;
            //移動した方向にPlayerの向きを変更する
            transform.LookAt(direction);
            rb.velocity = (new Vector3(x,0,z) * speed + Vector3.down * gravSpeed);
        }

        if (IsActionCharging && !IsEnemyMoving)
        {
            StartCoroutine(EnemyMoveToTargetArea());
            IsEnemyMoving = true;
        }
    }

    private void SetEnemyNum()
    {
        int actionNum = 0;
        for (int i = followingEnemy.Count-1; i > -1; i--)
        {
            followingEnemy[i].followNum = i;
            followingEnemy[i].actionNum = actionNum;
            actionNum++;
        }
    }
    
    //Colliderになにかが当たったら
    private void OnTriggerEnter(Collider other)
    {
        //PlayerがActionPointに入ったら
        if(other.tag == "ActionPoint")
        {
            Debug.Log("範囲内に入りました");
            //アクションを行う目標地点とアクションを行うための赤ハコベロスの数を取得
            if (other.gameObject.GetComponent<ActionArea>().IsPileUp)
            {
                pileUpPos = other.gameObject.GetComponent<ActionArea>().targetPoint;
                actionCost = other.gameObject.GetComponent<ActionArea>().needNum;
                IsPileUp = true;
            }
            //アクションに必要な分の赤ハコベロスを連れている時だけアクションを行う許可を出す
            if (followingEnemy.Count >= actionCost)
            {
                CanAction = true;
            }
        }

        //Playerが骨と接触した場合
        if (other.tag == "Bone")
        {
            Debug.Log("骨を拾いました");
            //処理が重複しないように骨のタグを変更
            other.tag = "Untagged";
            //骨のデータを一番目の赤ハコベロスに渡し、その赤ハコベロスをリストから削除
            if (followingEnemy.Count > 0)
            {
                followingEnemy[0].bone = other.gameObject.GetComponent<Bone>();
                followingEnemy.RemoveAt(0);
            }
        }

        //Playerが赤ハコベロスを連れていない状態でゴールに到達した場合
        if (other.tag == "Goal" && followingEnemy.Count == 0)
        {
            //Goalしたログを出す
            Debug.Log("Goal");
           
            //Playerを非表示にする
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ActionPoint")
        {
            other.gameObject.GetComponent<ActionArea>().PayedCost = PayedCost;
        }

        //CanAction状態でアクションボタンを長押しする場合
        if (Input.GetButton("DS4x") || Input.GetKey(KeyCode.Space))
        {
            if (CanAction && !IsActionConfirm)
            {
                IsActionCharging = true;
                // 秒数を数える
                actionPush += Time.deltaTime;
                //アクションゲージを出す
                gaugeController.gameObject.SetActive(true);
                gaugeController.DrawGauge(actionPush);

                // 3秒経ったら
                if (actionPush >= actionTime && !InAction)
                {
                    Debug.Log("3秒経過");
                    IsActionConfirm = true;
                    //アクションゲージを消す
                    gaugeController.gameObject.SetActive(false);
                    gaugeController.DrawGauge(0);
                }
            }
        }
        //アクションボタンを離した場合
        else if(!Input.GetButtonUp("DS4x") || !Input.GetKeyUp(KeyCode.Space))
        {
            //アクションを開始する前の状態に戻す
            IsActionCharging = false;
            PayedCost = 0;
            actionPush = 0;
            gaugeController.gameObject.SetActive(false);
            gaugeController.DrawGauge(0);
            InAction = false;
            if (!IsActionConfirm)
            {
                foreach (Enemy obj in followingEnemy)
                {
                    obj.CancelAction();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //ActionPointから離れた場合
        if (other.tag == "ActionPoint")
        {
            Debug.Log("範囲内から離れました");
            //アクションを開始する前の状態に戻す
            gaugeController.gameObject.SetActive(false);
            gaugeController.DrawGauge(0);
            CanAction = false;
            actionCost = 0;
            if (!IsActionConfirm)
            {
                other.gameObject.GetComponent<ActionArea>().PayedCost = 0;
                foreach (Enemy obj in followingEnemy)
                {
                    obj.CancelAction();
                    IsPileUp = false;
                }
            }
        }
    }

    private IEnumerator EnemyMoveToTargetArea()
    {
        for (int i = actionCost-1; i > -1; i--)
        {
            followingEnemy[i].actionTargetPos = pileUpPos;
            followingEnemy[i].IsFollow = false;
            followingEnemy[i].IsAction = true;
            if (IsPileUp)
            {
                followingEnemy[i].IsPileUp = true;
                if (actionCost-1 > i)
                {
                    followingEnemy[i].NeedJump = true;
                }
            }
            
            yield return new WaitForSeconds(1f);
        }
    }

    private void ClimbUp()
    {
        rb.useGravity = false;
        if (!IsClimbing && !ClimbFinish)
        {
            transform.LookAt(pileUpPos);
            transform.position += transform.forward * 0.1f;
        }

        if (!IsClimbing && !ClimbFinish && Vector3.Distance(transform.position, pileUpPos.position) <= 0.3f)
        {
            IsClimbing = true;
            transform.position = pileUpPos.position - new Vector3(0,0,0.3f);
            transform.rotation = Quaternion.Euler(0,-90,90);
        }

        if (IsClimbing && !ClimbFinish)
        {
            transform.position += Vector3.up * 0.1f;
            if (1.5f - transform.position.y <= 0.1f)
            {
                IsClimbing = false;
                ClimbFinish = true;
                transform.rotation = Quaternion.identity;
            }
        }

        if (ClimbFinish)
        {
            Debug.Log(transform.position.z);
            transform.position += transform.forward * 0.1f;
            if (1.1f - transform.localPosition.z <= 0.1f)
            {
                //仮
                foreach(Enemy obj in followingEnemy)
                {
                    obj.transform.position = this.transform.position;
                    obj.IsAction = false;
                    obj.PileUpFinish = false;
                    obj.IsFollow = false;
                }
                ClimbFinish = false;
                InAction = false;
                IsActionConfirm = false;
                IsPileUp = false;
            }
        }
    }

    private void UpdateNotice()
    {
        PayedCost = 0;
        for (int i = 0; i < actionCost; i++)
        {
            if (IsPileUp && followingEnemy[i].PileUpFinish)
            {
                PayedCost++;
            }
        }
    }

    private bool CheckIsEnemyActionFinish()
    {
        int finishedNum = 0;
        for (int i = 0; i < actionCost; i++)
        {
            if (IsPileUp && followingEnemy[i].PileUpFinish)
            {
                finishedNum++;
            }
        }

        if (finishedNum == actionCost)
        {
            IsActionCharging = false;
            IsEnemyMoving = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}