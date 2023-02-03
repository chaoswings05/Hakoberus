using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("移動速度")] private float speed = 0.05f;

    #region プレイヤー追従関連
    [SerializeField, Header("追従するプレイヤー")] private Transform playerObj = null;
    [SerializeField, Header("赤ハコベロスの陣形の位置リスト")] private GameObject[] followPosArray = new GameObject[5];
    [Header("この赤ハコベロスの追従番号")] public int followNum = 0;
    [Header("この赤ハコベロスのアクション番号")] public int actionNum = 0;
    private GameObject followPos = null; //この赤ハコベロスの配属するポイント
    [Header("プレイヤーに追随するかどうか")] public bool IsFollow = true;
    #endregion

    #region アニメーション関連
    [SerializeField] private Animator enemyAnimator = null;
    private int isWalkingID = Animator.StringToHash("IsWalking");
    #endregion

    #region 骨関連
    [Header("保持している骨オブジェクト")] public Bone bone = null;
    [Header("骨を持っていく場所")] public Transform BoneDestroyPos = null;
    [Header("骨を破壊するアクションを行うかどうか")] public bool IsBoneDestroy = false;
    #endregion

    #region アクション関連
    [Header("アクションを行う場所")] public Transform actionTargetPos = null;
    [Header("アクション中かどうか")] public bool IsAction = false;
    public bool IsWalkingAction = false;
    //階段積み上げアクション
    [Header("ジャンプをする必要があるかどうか")]public bool NeedJump = false;
    private bool IsJumping = false; //ジャンプ中かどうか
    //橋掛けアクション
    [Header("積み上げアクションが終了したかどうか")] public bool PileUpFinish = false;
    [Header("橋掛けアクションが終了したかどうか")] public bool BuildFinish = false;
    //階段登りアクション
    public bool IsClimbUp = false;
    private bool IsClimbing = false; //階段を登り始めたかどうか
    private bool ClimbFinish = false; //階段を登り切ったかどうか
    public bool IsWaiting = false; //階段を登るアクションが完全に終了したかどうか
    //橋を渡るアクション
    public bool IsCrossBridge = false;
    private int crossNum = 0; //何体目の赤ハコベロスの上を渡っているか
    public bool CrossFinish = false; //橋を完全に渡り切ったかどうか
    #endregion

    void Update()
    {
        //骨を加えている状態
        if(bone != null)
        {
            bone.transform.position = transform.position + Vector3.forward * 0.5f;//targetObjectを上にする
            bone.transform.SetParent(transform); //Boneと自分をくっつける
            BoneDestroyPos = bone.boneDestroyPos;
            bone.gameObject.layer = 7;
            IsBoneDestroy = true; //家に向かう
            IsFollow = false;
        }
        //骨を消す場所に行く時
        if(IsBoneDestroy)
        {
            BoneDestroy();
            return;
        }
        //アクション中および骨を咥えている状態でない時
        if (!IsAction && !IsWalkingAction && !IsBoneDestroy)
        {
            //プレイヤーから一定距離離れると、追従モードに入る
            if (Vector3.Distance(transform.position, followPos.transform.position) <= 0.1f)
            {
                IsFollow = false;
                enemyAnimator.SetBool(isWalkingID, false);
            }
            else
            {
                IsFollow = true;
            }
        }
        //もしPlayerに追従するなら
        if (IsFollow)
        {
            enemyAnimator.SetBool(isWalkingID, true);
            transform.LookAt(followPos.transform);
            transform.position += transform.forward * speed;
            transform.rotation = playerObj.transform.rotation;
        }
        //階段を積むアクション
        if (IsAction && GameDirector.Instance.AP.IsPileUp)
        {
            PileUp();
            return;
        }
        //橋を掛けるアクション
        if (IsAction && GameDirector.Instance.AP.IsBuildBridge)
        {
            BuildBridge();
            return;
        }
        //階段を登るアクション
        if (IsWalkingAction && IsClimbUp)
        {
            ClimbUp();
            return;
        }
        //橋を渡るアクション
        if (IsWalkingAction && IsCrossBridge)
        {
            CrossBridge();
            return;
        }
    }

    /// <summary>
    /// 隊列内の自分が着くべきポイントを設定する
    /// </summary>
    public void SetFollowPoint()
    {
        followPos = followPosArray[followNum];
    }

    /// <summary>
    /// 骨を持ってフィールドを離れる処理
    /// </summary>
    private void BoneDestroy()
    {
        enemyAnimator.SetBool(isWalkingID, true);
        //離脱ポイントに向かう
        transform.LookAt(BoneDestroyPos.transform.position);
        transform.position += transform.forward * speed;

        //離脱ポイントと一定距離になったら、自身と所持している骨を削除する
        if (Vector3.Distance(transform.position, BoneDestroyPos.transform.position) <= 1f)
        {
            Destroy(bone.gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 階段になる処理
    /// </summary>
    private void PileUp()
    {
        if (!PileUpFinish && !IsJumping)
        {
            enemyAnimator.SetBool(isWalkingID, true);
            transform.LookAt(actionTargetPos.position);
            transform.position += transform.forward * speed;
        }

        if (!PileUpFinish && !NeedJump && Vector3.SqrMagnitude(transform.position - actionTargetPos.position) <= 0.01f)
        {
            enemyAnimator.SetBool(isWalkingID, false);
            transform.position = actionTargetPos.position;
            transform.rotation = Quaternion.Euler(0,GameDirector.Instance.AP.forward,0);
            PileUpFinish = true;
        }
        else if (!IsJumping && NeedJump && Vector3.SqrMagnitude(transform.position - actionTargetPos.position) <= 1f)
        {
            enemyAnimator.SetBool(isWalkingID, false);
            actionTargetPos = GameDirector.Instance.AP.actionPoint[actionNum];
            if (!IsJumping)
            {
                transform.DOJump(actionTargetPos.position, 0.5f, 1, 0.5f).OnComplete(() => JumpFinish());
            }
            IsJumping = true;
        }
    }

    /// <summary>
    /// 階段積みの際にジャンプをした場合、リセットする関数
    /// </summary>
    private void JumpFinish()
    {
        NeedJump = false;
        transform.position = actionTargetPos.position;
        transform.rotation = Quaternion.Euler(0,GameDirector.Instance.AP.forward,0);
        IsJumping = false;
        PileUpFinish = true;
    }

    /// <summary>
    /// 橋になる処理
    /// </summary>
    private void BuildBridge()
    {
        if (!BuildFinish)
        {
            enemyAnimator.SetBool(isWalkingID, true);
            transform.LookAt(actionTargetPos);
            transform.position += transform.forward * speed;
        }

        if (Vector3.SqrMagnitude(transform.position - actionTargetPos.position) <= 0.01f)
        {
            enemyAnimator.SetBool(isWalkingID, false);
            transform.position = actionTargetPos.position;
            transform.rotation = Quaternion.Euler(0,GameDirector.Instance.AP.forward,0);
            BuildFinish = true;
        }
    }

    /// <summary>
    /// アクション終了後に、ジャンプして帰還する関数
    /// </summary>
    /// <param name="actionEndPos">目標ポイント</param>
    public void JumpToEndPoint(Vector3 actionEndPos)
    {
        transform.DOJump(actionEndPos, 0.5f, 1, 0.5f);
    }

    /// <summary>
    /// 階段を登るアクション
    /// </summary>
    private void ClimbUp()
    {
        if (!IsClimbing && !ClimbFinish && !IsWaiting)
        {
            enemyAnimator.SetBool(isWalkingID, true);
            transform.LookAt(GameDirector.Instance.AP.walkPoint[0]);
            transform.position += transform.forward * speed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[0].position) <= 0.01f)
            {
                IsClimbing = true;
                transform.position = GameDirector.Instance.AP.walkPoint[0].position;
                transform.rotation = Quaternion.Euler(-90,GameDirector.Instance.AP.forward,0);
                enemyAnimator.SetBool(isWalkingID, false);
            }
        }
        else if (IsClimbing && !ClimbFinish)
        {
            enemyAnimator.SetBool(isWalkingID, true);
            transform.position += Vector3.up * speed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[1].position) <= 0.01f)
            {
                IsClimbing = false;
                ClimbFinish = true;
                transform.position = GameDirector.Instance.AP.walkPoint[1].position;
                transform.rotation = Quaternion.Euler(0,GameDirector.Instance.AP.forward,0);
                enemyAnimator.SetBool(isWalkingID, false);
            }
        }
        else if (ClimbFinish)
        {
            enemyAnimator.SetBool(isWalkingID, true);
            transform.position += transform.forward * speed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[2].position) <= 0.01f)
            {
                ClimbFinish = false;
                transform.position = GameDirector.Instance.AP.walkPoint[2].position;
                enemyAnimator.SetBool(isWalkingID, false);
                IsWaiting = true;
            }
        }
    }

    /// <summary>
    /// 橋を渡るアクション
    /// </summary>
    private void CrossBridge()
    {
        if (!CrossFinish)
        {
            transform.LookAt(GameDirector.Instance.AP.walkPoint[crossNum]);
            transform.position += transform.forward * speed;

            if (Vector3.SqrMagnitude(transform.position - GameDirector.Instance.AP.walkPoint[crossNum].position) <= 0.01f)
            {
                transform.position = GameDirector.Instance.AP.walkPoint[crossNum].position;
                if (crossNum == GameDirector.Instance.AP.needNum)
                {
                    transform.position = GameDirector.Instance.AP.walkPoint[crossNum].position;
                    enemyAnimator.SetBool(isWalkingID, false);
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
}