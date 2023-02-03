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
    private bool IsWalkingAction = false; //歩きアクション中かどうか
    private bool IsEnemyLeft = false; //赤ハコベロスが余っているかどうか
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
                //プレイヤーの入力を切る
                IsActionConfirm = true;
                playerAnimator.SetBool(isWalkingID, false);
            }
        }

        //アクションが開始した場合、アクション担当の赤ハコベロスを派遣させる
        if (IsActionConfirm && !IsEnemyMoving)
        {
            StartCoroutine(EnemyMoveToTargetArea());
            IsEnemyMoving = true;
        }

        //アクション担当の赤ハコベロスがアクションを終了し、余った赤ハコベロスが存在する場合、先に余った赤ハコベロスがついて来る準備をする
        if (IsActionConfirm && CheckIsEnemyActionFinish())
        {
            if (!IsWalkingAction && followingEnemy.Count > GameDirector.Instance.AP.needNum)
            {
                StartCoroutine(LeftEnemyFollowPlayer());
                IsWalkingAction = true;
                //ここでプレイヤーの移動を一旦遅らせる
                return;
            }

            //階段登り
            if (GameDirector.Instance.AP.IsPileUp)
            {
                ClimbUp();
            }
            //橋渡り
            if (GameDirector.Instance.AP.IsBuildBridge)
            {
                CrossBridge();
            }
        }

        //赤ハコベロスが余って、それらがすべてアクション終了位置に着いたら、アクション担当の赤ハコベロスを帰還させる
        if (IsEnemyLeft && CheckIsEnemyWalkingActionFinish())
        {
            StartCoroutine(EnemyJumpToEndPoint());
            IsEnemyLeft = false;
        }

        float x = Input.GetAxisRaw("Horizontal") * speed;
        float z = Input.GetAxisRaw("Vertical") * speed;

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
            rb.velocity = new Vector3(x, rb.velocity.y, z);
        }
    }

    /// <summary>
    /// 連れている赤ハコベロスの数によって移動速度が変わるやつ
    /// </summary>
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
    }

    /// <summary>
    /// 赤ハコベロスに番号を振り分ける
    /// </summary>
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
            //触れたActionPointを登録しておく
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
            //Playerを非表示にする
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //ActionPointから離れた場合
        if (other.CompareTag("ActionPoint"))
        {
            CanAction = false;
        }
    }

    /// <summary>
    /// 赤ハコベロスをアクション所定位置に配置する処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyMoveToTargetArea()
    {
        var cachedWait = new WaitForSeconds(1f);
        int standbyNum = 0; //何番目の赤ハコベロスか

        //後ろから順で数える
        for (int i = followingEnemy.Count-1; i > followingEnemy.Count-GameDirector.Instance.AP.needNum-1; i--)
        {
            //赤ハコベロスのプレイヤー追従を切る
            followingEnemy[i].IsFollow = false;
            followingEnemy[i].IsAction = true;
            //赤ハコベロスに所定位置を与える
            if (GameDirector.Instance.AP.IsPileUp)
            {
                followingEnemy[i].actionTargetPos = GameDirector.Instance.AP.actionPoint[0];
                //階段積み上げアクションの場合、二体目以降はジャンプの必要がある
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

    /// <summary>
    /// 赤ハコベロスが余った時、余ったハコベロスをプレイヤーと同じ動きをさせる
    /// </summary>
    /// <returns></returns>
    private IEnumerator LeftEnemyFollowPlayer()
    {
        var cachedWait = new WaitForSeconds(0.1f);

        //前から順で数える
        for (int i = 0; i < followingEnemy.Count-GameDirector.Instance.AP.needNum; i++)
        {
            //必要なフラグを立たせる
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

    /// <summary>
    /// 階段登りアクション
    /// </summary>
    private void ClimbUp()
    {
        //一旦重力を外す
        rb.useGravity = false;
        //ポイントを順番に辿っていき、一定距離内になったら次のポイントを目標に変更する
        if (!IsClimbing && !ClimbFinish && !IsWaiting)
        {
            playerAnimator.SetBool(isWalkingID, true);
            transform.LookAt(GameDirector.Instance.AP.walkPoint[0]);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[0].position) <= 0.01f)
            {
                IsClimbing = true;
                transform.position = GameDirector.Instance.AP.walkPoint[0].position;
                //ここから登り始めるので、上に向かせる
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
                //ここで登りきるので、元の向きに戻す
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
                //ここでプレイヤー側のアクションが終了する
                if (followingEnemy.Count > GameDirector.Instance.AP.needNum)
                {
                    //余った赤ハコベロスが存在する場合、それらがアクションを終了するまで待つ
                    IsEnemyLeft = true;
                }
                else
                {
                    //余った赤ハコベロスがいない場合、階段担当の赤ハコベロスをジャンプで帰還させる
                    StartCoroutine(EnemyJumpToEndPoint());
                }
                ClimbFinish = false;
                transform.position = GameDirector.Instance.AP.walkPoint[2].position;
                playerAnimator.SetBool(isWalkingID, false);
                IsWaiting = true;
            }
        }
    }

    /// <summary>
    /// 橋を渡るアクション
    /// </summary>
    private void CrossBridge()
    {
        //一旦重力を外す
        rb.useGravity = false;
        //ポイントを順番に辿っていき、一定距離内になったら次のポイントを目標に変更する
        if (!CrossFinish)
        {
            transform.LookAt(GameDirector.Instance.AP.walkPoint[crossNum]);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[crossNum].position) <= 0.01f)
            {
                //ここでプレイヤー側のアクションが終了する
                transform.position = GameDirector.Instance.AP.walkPoint[crossNum].position;
                if (crossNum == GameDirector.Instance.AP.needNum)
                {
                    if (followingEnemy.Count > GameDirector.Instance.AP.needNum)
                    {
                        //余った赤ハコベロスが存在する場合、それらがアクションを終了するまで待つ
                        IsEnemyLeft = true;
                    }
                    else
                    {
                        //余った赤ハコベロスがいない場合、階段担当の赤ハコベロスをジャンプで帰還させる
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

    /// <summary>
    /// アクションを担当した赤ハコベロスがジャンプして帰還する処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyJumpToEndPoint()
    {
        var cachedWait = new WaitForSeconds(1f);

        //階段掛けアクションの場合、一番上の段の赤ハコベロスから戻ってくる
        if (GameDirector.Instance.AP.IsPileUp)
        {
            for (int i = followingEnemy.Count-GameDirector.Instance.AP.needNum; i < followingEnemy.Count; i++)
            {
                followingEnemy[i].JumpToEndPoint(GameDirector.Instance.AP.walkPoint[2].position);
                
                yield return cachedWait;
            }
        }
        //橋掛けアクションの場合、一番最初に橋になった赤ハコベロスから戻ってくる
        else if (GameDirector.Instance.AP.IsBuildBridge)
        {
            for (int i = followingEnemy.Count-1; i > followingEnemy.Count-GameDirector.Instance.AP.needNum-1; i--)
            {
                followingEnemy[i].JumpToEndPoint(GameDirector.Instance.AP.walkPoint[GameDirector.Instance.AP.needNum].position);
                
                yield return cachedWait;
            }
        }
        //赤ハコベロスが全員戻ってきたら、アクション用のフラグ類をリセットする
        ResetAfterActionFinish();
    }

    /// <summary>
    /// アクションを行う赤ハコベロスがすべてアクションを終了したかどうか調べる関数
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// アクションに参加しない余った赤ハコベロスがすべて、アクション終了地点までたどり着いたかどうか調べる関数
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// アクション後に色々リセットする関数
    /// </summary>
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
    }
}