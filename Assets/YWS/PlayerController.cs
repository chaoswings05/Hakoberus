using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("基礎移動速度")] private float defaultSpeed = 1f;
    private float speed = 1f;
    [SerializeField, Header("アクション中の移動速度")] private float actionSpeed = 0.1f;
    public List<Enemy> followingEnemy = new List<Enemy>(); //プレイヤーが連れている赤ハコベロスのリスト
    [SerializeField] private Rigidbody rb = null; //PlayerのRigidbodyを取得
    [SerializeField] private Animator playerAnimator = null;
    private bool CanAction = false; //アクションを行える状態なのか
    private int actionCost = 0; //アクションに必要な赤ハコベロスの数
    private bool IsActionConfirm = false;
    private bool IsEnemyMoving = false;
    private Transform actionPos = null; //赤ハコベロスがアクションを行う場所
    private Transform actionEndPos = null;
    private bool IsPileUp = false;
    private bool IsClimbing = false;
    private bool ClimbFinish = false;
    private bool IsBuildBridge = false;
    private int crossedNum = 0;
    private bool arrivalCentral = false;

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
                playerAnimator.SetBool("IsWalking", false);
            }
        }

        if (IsActionConfirm && !IsEnemyMoving)
        {
            StartCoroutine(EnemyMoveToTargetArea());
            IsEnemyMoving = true;
        }

        if (IsActionConfirm && CheckIsEnemyActionFinish())
        {
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
        if (!IsActionConfirm)
        {
            //Playerの座標を計算
            Vector3 direction = transform.position + new Vector3(x,0,z) * speed;

            if (x != 0 || z != 0)
            {
                playerAnimator.SetBool("IsWalking", true);
            }
            else
            {
                playerAnimator.SetBool("IsWalking", false);
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
                SetSpeed();
                SetEnemyNum();
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

    private void OnTriggerExit(Collider other)
    {
        //ActionPointから離れた場合
        if (other.tag == "ActionPoint")
        {
            Debug.Log("範囲内から離れました");
            CanAction = false;
            if (IsActionConfirm)
            {
                other.gameObject.GetComponent<ActionArea>().ActionFinish();
            }
        }
    }

    private IEnumerator EnemyMoveToTargetArea()
    {
        for (int i = followingEnemy.Count-1; i > followingEnemy.Count-actionCost-1; i--)
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
            playerAnimator.SetBool("IsWalking", true);
            transform.LookAt(actionPos);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.Distance(transform.position, actionPos.position) <= 0.3f)
            {
                IsClimbing = true;
                transform.position = actionPos.position - new Vector3(0,0,0.3f);
                transform.rotation = Quaternion.Euler(-90,0,0);
                playerAnimator.SetBool("IsWalking", false);
            }
        }

        if (IsClimbing && !ClimbFinish)
        {
            playerAnimator.SetBool("IsWalking", true);
            transform.position += Vector3.up * actionSpeed;

            if (actionEndPos.position.y - transform.position.y <= 0.1f)
            {
                IsClimbing = false;
                ClimbFinish = true;
                transform.position += new Vector3(0,0.1f,0);
                transform.rotation = Quaternion.identity;
                playerAnimator.SetBool("IsWalking", false);
            }
        }

        if (ClimbFinish)
        {
            playerAnimator.SetBool("IsWalking", true);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.Distance(transform.position, actionEndPos.position) <= 0.1f)
            {
                StartCoroutine(EnemyJumpToEndPoint());
                ClimbFinish = false;
                IsPileUp = false;
                transform.position = actionEndPos.position;
                playerAnimator.SetBool("IsWalking", false);
            }
        }
    }

    private void CrossBridge()
    {
        Vector3 CentralLocation = new Vector3(actionPos.position.x, this.transform.position.y, actionPos.position.z + 0.75f * crossedNum);

        if (!arrivalCentral)
        {
            transform.LookAt(CentralLocation);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.Distance(transform.position, CentralLocation) <= 0.1f)
            {
                transform.position = CentralLocation;
                crossedNum++;
                if (crossedNum == actionCost)
                {
                    arrivalCentral = true;
                    crossedNum = 0;
                }
            }
        }

        if (arrivalCentral)
        {
            transform.LookAt(actionEndPos.position);
            transform.position += transform.forward * actionSpeed;

            if (Vector3.Distance(transform.position, actionEndPos.position) <= 0.1f)
            {
                transform.position = actionEndPos.position;
                StartCoroutine(EnemyJumpToEndPoint());
                arrivalCentral = false;
                IsBuildBridge = false;
            }
        }
    }

    private IEnumerator EnemyJumpToEndPoint()
    {
        if (IsPileUp)
        {
            for (int i = 0; i < actionCost; i++)
            {
                followingEnemy[i].JumpToEndPoint(actionEndPos.position);
                
                yield return new WaitForSeconds(1f);
            }
        }
        else if (IsBuildBridge)
        {
            for (int i = followingEnemy.Count-1; i > followingEnemy.Count-actionCost-1; i--)
            {
                followingEnemy[i].JumpToEndPoint(actionEndPos.position);
                
                yield return new WaitForSeconds(1f);
            }
        }
        ResetAfterActionFinish();
    }

    private bool CheckIsEnemyActionFinish()
    {
        int finishedNum = 0;
        for (int i = followingEnemy.Count-1; i > followingEnemy.Count-actionCost-1; i--)
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
        rb.useGravity = true;
        transform.rotation = Quaternion.identity;
    }
}