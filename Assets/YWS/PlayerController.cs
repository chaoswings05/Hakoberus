using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーが連れている赤ハコベロスのリスト")] public List<Enemy> followingEnemy = new List<Enemy>();
    [SerializeField] private Rigidbody rb = null; //PlayerのRigidbodyを取得

    #region 移動速度関連
    [SerializeField, Header("基礎移動速度")] private float defaultSpeed = 2.8f;
    private float speed = 2.8f; //実際の移動速度
    [SerializeField, Header("アクション中の移動速度")] private float actionSpeed = 0.05f;
    #endregion

    #region アニメーション関連
    [SerializeField] private Animator playerAnimator = null;
    private int isWalkingID = Animator.StringToHash("IsWalking");
    #endregion

    #region アクション関連
    private bool CanAction = false; //アクションを行える状態なのか
    private bool IsActionConfirm = false; //アクションを実行するかどうか
    private bool IsEnemyMoving = false; //赤ハコベロスがアクションを開始したかどうか
    private bool IsWalkingAction = false;
    [SerializeField]private bool IsEnemyLeft = false;
    //階段登りアクション
    private bool IsClimbing = false; //階段を登り始めたかどうか
    private bool ClimbFinish = false; //階段を登り切ったかどうか
    private bool IsWaiting = false; //階段を登るアクションが完全に終了したかどうか
    //橋を渡るアクション
    private int crossNum = 0; //何体目の赤ハコベロスの上を渡っているか
    private bool CrossFinish = false; //橋を完全に渡り切ったかどうか
    #endregion

    void Start()
    {
        SetSpeed();
        SetEnemyNum();
    }

    void Update()
    {
        //CanAction状態でアクションボタンを押した場合
        if (Input.GetButtonDown("DS4x") || Input.GetKeyDown(KeyCode.Space))
        {
            if (CanAction && !IsActionConfirm)
            {
                IsActionConfirm = true;
                playerAnimator.SetBool(isWalkingID, false);
            }
        }

        if (IsActionConfirm && !IsEnemyMoving)
        {
            StartCoroutine(EnemyMoveToTargetArea());
            IsEnemyMoving = true;
        }

        if (IsActionConfirm && CheckIsEnemyActionFinish())
        {
            if (!IsWalkingAction && followingEnemy.Count > GameDirector.Instance.AP.needNum)
            {
                StartCoroutine(LeftEnemyFollowPlayer());
                IsWalkingAction = true;
                return;
            }

            if (GameDirector.Instance.AP.IsPileUp)
            {
                ClimbUp();
            }
            if (GameDirector.Instance.AP.IsBuildBridge)
            {
                CrossBridge();
            }
        }

        if (IsEnemyLeft && CheckIsEnemyWalkingActionFinish())
        {
            StartCoroutine(EnemyJumpToEndPoint());
            IsEnemyLeft = false;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //Playerの移動
        if (!IsActionConfirm)
        {
            //Playerの座標を計算
            Vector3 direction = transform.position + new Vector3(x,0,z) * speed;

            if (x != 0 || z != 0)
            {
                playerAnimator.SetBool(isWalkingID, true);
            }
            else
            {
                playerAnimator.SetBool(isWalkingID, false);
            }

            //移動した方向にPlayerの向きを変更する
            transform.LookAt(direction);
            rb.velocity = (new Vector3(x,0,z) * speed);
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
        else
        {
            speed = defaultSpeed;
        }

        //Debug.Log(speed);
    }

    private void SetEnemyNum()
    {
        int actionNum = 0;
        for (int i = followingEnemy.Count-1; i > -1; i--)
        {
            followingEnemy[i].followNum = i;
            followingEnemy[i].actionNum = actionNum;
            followingEnemy[i].SetFollowPoint();
            actionNum++;
        }
    }
    
    //Colliderになにかが当たったら
    private void OnTriggerEnter(Collider other)
    {
        //PlayerがActionPointに入ったら
        if(other.CompareTag("ActionPoint"))
        {
            //Debug.Log("範囲内に入りました");
            GameDirector.Instance.AP = other.gameObject.GetComponent<ActionArea>();

            //アクションに必要な分の赤ハコベロスを連れている時だけアクションを行う許可を出す
            if (followingEnemy.Count >= GameDirector.Instance.AP.needNum)
            {
                CanAction = true;
            }
        }

        //Playerが骨と接触した場合
        if (other.CompareTag("Bone"))
        {
            //Debug.Log("骨を拾いました");

            SoundManager.Instance.PlaySE(0);

            //処理が重複しないように骨のタグを変更
            other.tag = "Untagged";
            //骨のデータを一番目の赤ハコベロスに渡し、その赤ハコベロスをリストから削除
            if (followingEnemy.Count > 0)
            {
                followingEnemy[0].bone = other.gameObject.GetComponent<Bone>();
                followingEnemy.RemoveAt(0);
                SetSpeed();
                SetEnemyNum();
            }
        }

        //Playerが赤ハコベロスを連れていない状態でゴールに到達した場合
        if (other.CompareTag("Goal") && followingEnemy.Count == 0)
        {
            //Goalしたログを出す
            //Debug.Log("Goal");
           
            //Playerを非表示にする
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //ActionPointから離れた場合
        if (other.CompareTag("ActionPoint"))
        {
            //Debug.Log("範囲内から離れました");
            CanAction = false;
        }
    }

    private IEnumerator EnemyMoveToTargetArea()
    {
        var cachedWait = new WaitForSeconds(1f);
        int standbyNum = 0;
        for (int i = followingEnemy.Count-1; i > followingEnemy.Count-GameDirector.Instance.AP.needNum-1; i--)
        {
            followingEnemy[i].IsFollow = false;
            followingEnemy[i].IsAction = true;
            if (GameDirector.Instance.AP.IsPileUp)
            {
                followingEnemy[i].actionTargetPos = GameDirector.Instance.AP.actionPoint[0];
                if (GameDirector.Instance.AP.needNum > 1 && standbyNum >= 1)
                {
                    followingEnemy[i].NeedJump = true;
                }
            }
            else if (GameDirector.Instance.AP.IsBuildBridge)
            {
                followingEnemy[i].actionTargetPos = GameDirector.Instance.AP.actionPoint[followingEnemy[i].actionNum];
            }
            standbyNum++;
            
            yield return cachedWait;
        }
    }

    private IEnumerator LeftEnemyFollowPlayer()
    {
        var cachedWait = new WaitForSeconds(0.1f);

        for (int i = 0; i < followingEnemy.Count-GameDirector.Instance.AP.needNum; i++)
        {
            followingEnemy[i].IsFollow = false;
            followingEnemy[i].IsWalkingAction = true;
            if (GameDirector.Instance.AP.IsPileUp)
            {
                followingEnemy[i].IsClimbUp = true;
            }
            else if (GameDirector.Instance.AP.IsBuildBridge)
            {
                followingEnemy[i].IsCrossBridge = true;
            }

            yield return null;
        }
    }

    private void ClimbUp()
    {
        rb.useGravity = false;
        if (!IsClimbing && !ClimbFinish && !IsWaiting)
        {
            playerAnimator.SetBool(isWalkingID, true);
            transform.LookAt(GameDirector.Instance.AP.walkPoint[0]);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[0].position) <= 0.01f)
            {
                IsClimbing = true;
                transform.position = GameDirector.Instance.AP.walkPoint[0].position;
                transform.rotation = Quaternion.Euler(-90,GameDirector.Instance.AP.forward,0);
                playerAnimator.SetBool(isWalkingID, false);
            }
        }
        else if (IsClimbing && !ClimbFinish)
        {
            playerAnimator.SetBool(isWalkingID, true);
            transform.position += Vector3.up * actionSpeed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[1].position) <= 0.01f)
            {
                IsClimbing = false;
                ClimbFinish = true;
                transform.position = GameDirector.Instance.AP.walkPoint[1].position;
                transform.rotation = Quaternion.Euler(0,GameDirector.Instance.AP.forward,0);
                playerAnimator.SetBool(isWalkingID, false);
            }
        }
        else if (ClimbFinish)
        {
            playerAnimator.SetBool(isWalkingID, true);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[2].position) <= 0.01f)
            {
                if (followingEnemy.Count > GameDirector.Instance.AP.needNum)
                {
                    IsEnemyLeft = true;
                }
                else
                {
                    StartCoroutine(EnemyJumpToEndPoint());
                }
                ClimbFinish = false;
                transform.position = GameDirector.Instance.AP.walkPoint[2].position;
                playerAnimator.SetBool(isWalkingID, false);
                IsWaiting = true;
            }
        }
    }

    private void CrossBridge()
    {
        rb.useGravity = false;
        if (!CrossFinish)
        {
            transform.LookAt(GameDirector.Instance.AP.walkPoint[crossNum]);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[crossNum].position) <= 0.01f)
            {
                transform.position = GameDirector.Instance.AP.walkPoint[crossNum].position;
                if (crossNum == GameDirector.Instance.AP.needNum)
                {
                    if (followingEnemy.Count > GameDirector.Instance.AP.needNum)
                    {
                        IsEnemyLeft = true;
                    }
                    else
                    {
                        StartCoroutine(EnemyJumpToEndPoint());
                    }
                    transform.position = GameDirector.Instance.AP.walkPoint[crossNum].position;
                    playerAnimator.SetBool(isWalkingID, false);
                    CrossFinish = true;
                    crossNum = 0;
                }
                else
                {
                    crossNum++;
                }
            }
        }
    }

    private IEnumerator EnemyJumpToEndPoint()
    {
        var cachedWait = new WaitForSeconds(1f);

        if (GameDirector.Instance.AP.IsPileUp)
        {
            for (int i = followingEnemy.Count-GameDirector.Instance.AP.needNum; i < followingEnemy.Count; i++)
            {
                followingEnemy[i].JumpToEndPoint(GameDirector.Instance.AP.walkPoint[2].position);
                
                yield return cachedWait;
            }
        }
        else if (GameDirector.Instance.AP.IsBuildBridge)
        {
            for (int i = followingEnemy.Count-1; i > followingEnemy.Count-GameDirector.Instance.AP.needNum-1; i--)
            {
                followingEnemy[i].JumpToEndPoint(GameDirector.Instance.AP.walkPoint[GameDirector.Instance.AP.needNum].position);
                
                yield return cachedWait;
            }
        }
        ResetAfterActionFinish();
    }

    private bool CheckIsEnemyActionFinish()
    {
        int finishedNum = 0;
        for (int i = followingEnemy.Count-1; i > followingEnemy.Count-GameDirector.Instance.AP.needNum-1; i--)
        {
            if (GameDirector.Instance.AP.IsPileUp && followingEnemy[i].PileUpFinish)
            {
                finishedNum++;
            }

            if (GameDirector.Instance.AP.IsBuildBridge && followingEnemy[i].BuildFinish)
            {
                finishedNum++;
            }
        }

        if (finishedNum == GameDirector.Instance.AP.needNum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckIsEnemyWalkingActionFinish()
    {
        int finishedNum = 0;
        for (int i = 0; i < followingEnemy.Count-GameDirector.Instance.AP.needNum; i++)
        {
            if (GameDirector.Instance.AP.IsPileUp && followingEnemy[i].IsWaiting)
            {
                finishedNum++;
            }

            if (GameDirector.Instance.AP.IsBuildBridge && followingEnemy[i].CrossFinish)
            {
                finishedNum++;
            }
        }

        if (finishedNum == followingEnemy.Count - GameDirector.Instance.AP.needNum)
        {
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
            obj.IsFollow = false;
            obj.IsAction = false;
            obj.IsWalkingAction = false;
            obj.PileUpFinish = false;
            obj.BuildFinish = false;
            obj.IsClimbUp = false;
            obj.IsWaiting = false;
            obj.IsCrossBridge = false;
            obj.CrossFinish = false;
            obj.transform.rotation = Quaternion.Euler(0,GameDirector.Instance.AP.forward,0);
        }
        IsActionConfirm = false;
        IsEnemyMoving = false;
        IsWalkingAction = false;
        IsWaiting = false;
        CrossFinish = false;
        rb.useGravity = true;
        transform.rotation = Quaternion.Euler(0,GameDirector.Instance.AP.forward,0);

        if (GameDirector.Instance.AP.NeedBlock)
        {
            GameDirector.Instance.AP.blockingActivate();
        }
    }
}