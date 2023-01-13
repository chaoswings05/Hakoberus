using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("基礎移動速度")] private float defaultSpeed = 1f;
    private float speed = 1f;
    public List<Enemy> followingEnemy = new List<Enemy>(); //プレイヤーが連れている赤ハコベロスのリスト
    [SerializeField] private Rigidbody rb = null; //PlayerのRigidbodyを取得
    private bool CanAction = false; //アクションを行える状態なのか
    [SerializeField, Header("アクションの必要時間")] private float actionTime = 3f;
    private float actionPush = 0f; //アクションボタンを長押した時間
    private int actionCost = 0; //アクションに必要な赤ハコベロスの数
    private bool InAction = false;
    private bool IsActionCharging = false;
    private bool IsActionConfirm = false;
    private bool IsEnemyMoving = false;
    private Transform actionPos = null; //赤ハコベロスがアクションを行う場所
    private Transform actionEndPos = null;
    private bool IsPileUp = false;
    private bool IsClimbing = false;
    private bool ClimbFinish = false;
    private bool IsBuildBridge = false;
    private bool arrivalCentral = false;
    private bool CrossBridgeFinish = false;
    [SerializeField] private GaugeController gaugeController = null;

    void Update()
    {
        SetSpeed();
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
            if (IsBuildBridge)
            {
                CrossBridge();
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
            transform.LookAt(SetLookDirection(x, z));
            rb.velocity = (new Vector3(x,0,z) * speed);

            for (int i = 0; i < followingEnemy.Count; i++)
            {
                followingEnemy[i].transform.localRotation = this.transform.localRotation;
            }
        }

        if (IsActionCharging && !IsEnemyMoving)
        {
            StartCoroutine(EnemyMoveToTargetArea());
            IsEnemyMoving = true;
        }
    }

    private void SetSpeed()
    {
        if (followingEnemy.Count == 2 || followingEnemy.Count == 3)
        {
            speed = defaultSpeed - 0.1f;
        }
        else if (followingEnemy.Count == 4 || followingEnemy.Count == 5)
        {
            speed = defaultSpeed - 0.2f;
        }
        else if (followingEnemy.Count == 6 || followingEnemy.Count == 7 || followingEnemy.Count == 8)
        {
            speed = defaultSpeed - 0.3f;
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

    private Vector3 SetLookDirection(float x, float z)
    {
        Vector3 lookDirection = transform.position + new Vector3(-z,0,x) * speed;

        return lookDirection;
    }
    
    //Colliderになにかが当たったら
    private void OnTriggerEnter(Collider other)
    {
        //PlayerがActionPointに入ったら
        if(other.tag == "ActionPoint")
        {
            Debug.Log("範囲内に入りました");
            //アクションを行う目標地点とアクションを行うための赤ハコベロスの数を取得
            actionPos = other.gameObject.GetComponent<ActionArea>().targetPoint;
            actionCost = other.gameObject.GetComponent<ActionArea>().needNum;
            if (other.gameObject.GetComponent<ActionArea>().IsPileUp)
            {
                IsPileUp = true;
            }
            if (other.gameObject.GetComponent<ActionArea>().IsBuildBridge)
            {
                IsBuildBridge = true;
            }
            actionEndPos = other.gameObject.GetComponent<ActionArea>().endPoint;

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
            other.gameObject.GetComponent<ActionArea>().PayedCost = UpdateNotice();
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
                gaugeController.ShowGauge();
                gaugeController.DrawGauge(actionPush);

                // 3秒経ったら
                if (actionPush >= actionTime && !InAction)
                {
                    Debug.Log("3秒経過");
                    IsActionConfirm = true;
                    //アクションゲージを消す
                    gaugeController.HideGauge();
                    gaugeController.DrawGauge(0);
                }
            }
        }
        //アクションボタンを離した場合
        else if(!Input.GetButtonUp("DS4x") || !Input.GetKeyUp(KeyCode.Space))
        {
            //アクションを開始する前の状態に戻す
            actionPush = 0;
            gaugeController.HideGauge();
            gaugeController.DrawGauge(0);
            InAction = false;
            if (!IsActionConfirm)
            {
                foreach (Enemy obj in followingEnemy)
                {
                    obj.CancelAction();
                    if (IsActionCharging)
                    {
                        obj.WrapToFollowPoint();
                    }
                }
            }
            IsActionCharging = false;
            IsEnemyMoving = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //ActionPointから離れた場合
        if (other.tag == "ActionPoint")
        {
            Debug.Log("範囲内から離れました");
            //アクションを開始する前の状態に戻す
            gaugeController.HideGauge();
            gaugeController.DrawGauge(0);
            CanAction = false;
            if (!IsActionConfirm)
            {
                actionCost = 0;
                other.gameObject.GetComponent<ActionArea>().PayedCost = 0;
                foreach (Enemy obj in followingEnemy)
                {
                    obj.CancelAction();
                    if (IsActionCharging)
                    {
                        obj.WrapToFollowPoint();
                    }
                    IsPileUp = false;
                }
            }
            else
            {
                other.gameObject.GetComponent<ActionArea>().ActionFinish();
            }
            IsActionCharging = false;
            IsEnemyMoving = false;
        }
    }

    private IEnumerator EnemyMoveToTargetArea()
    {
        for (int i = actionCost-1; i > -1; i--)
        {
            followingEnemy[i].IsFollow = false;
            followingEnemy[i].IsAction = true;
            if (IsPileUp)
            {
                followingEnemy[i].actionTargetPos = actionPos;
                followingEnemy[i].IsPileUp = true;
                if (actionCost-1 > i)
                {
                    followingEnemy[i].NeedJump = true;
                }
            }
            if (IsBuildBridge)
            {
                followingEnemy[i].actionTargetPos = actionPos;
                followingEnemy[i].IsBuildBridge = true;
            }
            
            yield return new WaitForSeconds(1f);
        }
    }

    private void ClimbUp()
    {
        rb.useGravity = false;
        if (!IsClimbing && !ClimbFinish)
        {
            transform.LookAt(actionPos);
            transform.position += transform.forward * 0.1f;

            if (Vector3.Distance(transform.position, actionPos.position) <= 0.3f)
            {
                IsClimbing = true;
                transform.position = actionPos.position - new Vector3(0,0,0.3f);
                transform.rotation = Quaternion.Euler(0,-90,90);
            }
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
            transform.position += transform.forward * 0.1f;

            if (1.1f - transform.localPosition.z <= 0.1f)
            {
                StartCoroutine(EnemyJumpToEndPoint());
                ClimbFinish = false;
                IsPileUp = false;
            }
        }
    }

    private void CrossBridge()
    {
        Vector3 CentralLocation = new Vector3(actionPos.position.x, this.transform.position.y, actionPos.position.z);

        if (!arrivalCentral && !CrossBridgeFinish)
        {
            transform.LookAt(CentralLocation);
            transform.position += transform.forward * 0.1f;

            if (Vector3.Distance(transform.position, CentralLocation) <= 0.1f)
            {
                arrivalCentral = true;
                transform.position = CentralLocation;
            }
        }

        if (arrivalCentral)
        {
            transform.LookAt(actionEndPos.position);
            transform.position += transform.forward * 0.1f;

            if (Vector3.Distance(transform.position, actionEndPos.position) <= 0.1f)
            {
                transform.position = actionEndPos.position;
                arrivalCentral = false;
                CrossBridgeFinish = true;
            }
        }

        if (CrossBridgeFinish)
        {
            StartCoroutine(EnemyJumpToEndPoint());
            IsBuildBridge = false;
            CrossBridgeFinish = false;
        }
    }

    private IEnumerator EnemyJumpToEndPoint()
    {
        for (int i = 0; i < actionCost; i++)
        {
            followingEnemy[i].JumpToEndPoint(actionEndPos.position);
            
            yield return new WaitForSeconds(1f);
        }
        ResetAfterActionFinish();
    }

    private int UpdateNotice()
    {
        int PayedCost = 0;
        for (int i = 0; i < actionCost; i++)
        {
            if (IsPileUp && followingEnemy[i].PileUpFinish)
            {
                PayedCost++;
            }

            if (IsBuildBridge && followingEnemy[i].BuildFinish)
            {
                PayedCost++;
            }
        }
        return PayedCost;
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

            if (IsBuildBridge && followingEnemy[i].BuildFinish)
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

    private void ResetAfterActionFinish()
    {
        foreach(Enemy obj in followingEnemy)
        {
            obj.tag = "Enemy";
            obj.gameObject.layer = 7;
            obj.IsAction = false;
            obj.PileUpFinish = false;
            obj.BuildFinish = false;
            obj.IsFollow = false;
        }
        InAction = false;
        IsActionConfirm = false;
        rb.useGravity = true;
    }
}