using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("基礎移動速度")] private float defaultSpeed = 1f;
    private float speed = 1f;
    [SerializeField, Header("アクション中の移動速度")] private float actionSpeed = 0.1f;
    public List<Enemy> followingEnemy = new List<Enemy>(); //プレイヤーが連れている赤ハコベロスのリスト
    [SerializeField] private Rigidbody rb = null; //PlayerのRigidbodyを取得
    [SerializeField] private Animator playerAnimator = null;
    private int isWalkingID = Animator.StringToHash("IsWalking");
    private bool CanAction = false; //アクションを行える状態なのか
    private bool IsActionConfirm = false;
    private bool IsEnemyMoving = false;
    private bool IsWaiting = false;
    private bool IsClimbing = false;
    private bool ClimbFinish = false;
    private int crossNum = 0;
    private bool CrossFinish = false;

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
            if (GameDirector.Instance.AP.IsPileUp)
            {
                ClimbUp();
            }
            if (GameDirector.Instance.AP.IsBuildBridge)
            {
                CrossBridge();
            }
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

        Debug.Log(speed);
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
            Debug.Log("範囲内に入りました");
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
            Debug.Log("骨を拾いました");
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
            Debug.Log("Goal");
           
            //Playerを非表示にする
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //ActionPointから離れた場合
        if (other.CompareTag("ActionPoint"))
        {
            Debug.Log("範囲内から離れました");
            CanAction = false;
        }
    }

    private IEnumerator EnemyMoveToTargetArea()
    {
        var cachedWait = new WaitForSeconds(1f);
        for (int i = followingEnemy.Count-1; i > followingEnemy.Count-GameDirector.Instance.AP.needNum-1; i--)
        {
            followingEnemy[i].IsFollow = false;
            followingEnemy[i].IsAction = true;
            if (GameDirector.Instance.AP.IsPileUp)
            {
                followingEnemy[i].actionTargetPos = GameDirector.Instance.AP.actionPoint[0];
                if (GameDirector.Instance.AP.needNum-1 > i)
                {
                    followingEnemy[i].NeedJump = true;
                }
            }
            else if (GameDirector.Instance.AP.IsBuildBridge)
            {
                followingEnemy[i].actionTargetPos = GameDirector.Instance.AP.actionPoint[followingEnemy[i].actionNum];
            }
            
            yield return cachedWait;
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
                transform.rotation = Quaternion.Euler(-90,0,0);
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
                transform.rotation = Quaternion.identity;
                playerAnimator.SetBool(isWalkingID, false);
            }
        }
        else if (ClimbFinish)
        {
            playerAnimator.SetBool(isWalkingID, true);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[2].position) <= 0.01f)
            {
                StartCoroutine(EnemyJumpToEndPoint());
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
                    transform.position = GameDirector.Instance.AP.walkPoint[crossNum].position;
                    StartCoroutine(EnemyJumpToEndPoint());
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
            for (int i = 0; i < GameDirector.Instance.AP.needNum; i++)
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

    private void ResetAfterActionFinish()
    {
        foreach(Enemy obj in followingEnemy)
        {
            obj.IsAction = false;
            obj.PileUpFinish = false;
            obj.BuildFinish = false;
            obj.IsFollow = false;
            obj.transform.rotation = Quaternion.identity;
        }
        IsActionConfirm = false;
        IsEnemyMoving = false;
        IsWaiting = false;
        CrossFinish = false;
        rb.useGravity = true;
        transform.rotation = Quaternion.identity;

        if (GameDirector.Instance.AP.NeedBlock)
        {
            GameDirector.Instance.AP.blockingActivate();
        }
    }
}