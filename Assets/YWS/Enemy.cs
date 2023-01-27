using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject playerObj = null;
    [SerializeField] private GameObject[] followPosArray = new GameObject[5];
    private GameObject followPos = null;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private Animator enemyAnimator = null;
    public int followNum = 0;
    public int actionNum = 0;
    public Transform actionTargetPos = null; //アクションを行う場所
    public Bone bone = null;
    public Transform BoneDestroyPos = null; //骨を持っていく場所
    public bool IsFollow = true; //プレイヤーに追随するかどうか
    public bool IsBoneDestroy = false; //骨を破壊するアクションを行うかどうか
    public bool IsAction = false; //アクション中かどうか
    public bool IsPileUp = false; //階段積み上げアクションを行うかどうか
    public bool NeedJump = false; //アクションを行う時、ジャンプをする必要があるかどうか
    private bool IsJumping = false;
    public bool PileUpFinish = false;
    public bool IsBuildBridge = false;
    public bool BuildFinish = false;

    void Start()
    {
        
    }

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
        if (!IsAction && !IsBoneDestroy)
        {
            //プレイヤーから一定距離離れると、追従モードに入る
            if (Vector3.Distance(transform.position, followPos.transform.position) <= 0.1f)
            {
                IsFollow = false;
                enemyAnimator.SetBool("IsWalking", false);
            }
            else
            {
                IsFollow = true;
            }
        }
        //もしPlayerに追従するなら
        if (IsFollow)
        {
            enemyAnimator.SetBool("IsWalking", true);
            transform.LookAt(followPos.transform);
            transform.position += transform.forward * speed;
            transform.rotation = playerObj.transform.rotation;
        }
        //階段を積むアクション
        if (IsAction && IsPileUp)
        {
            PileUp();
            return;
        }
        //橋を掛けるアクション
        if (IsAction && IsBuildBridge)
        {
            BuildBridge();
            return;
        }
    }

    public void SetFollowPoint()
    {
        followPos = followPosArray[followNum];
    }

    private void BoneDestroy()
    {
        enemyAnimator.SetBool("IsWalking", true);
        transform.LookAt(BoneDestroyPos.transform.position); //骨を消す場所に行く
        transform.position += transform.forward * speed;

        if (Vector3.Distance(transform.position, BoneDestroyPos.transform.position) <= 1f) //もしBoneDestroyPosの距離が1より小さくなったら
        {
            Destroy(bone.gameObject); //骨を消す
            Destroy(gameObject); //自分を消す
        }
    }

    private void PileUp()
    {
        if (!IsJumping)
        {
            enemyAnimator.SetBool("IsWalking", true);
            transform.LookAt(actionTargetPos.position);
            transform.position += transform.forward * speed;
        }

        if (!NeedJump && Vector3.Distance(transform.position, actionTargetPos.position) <= 0.1f)
        {
            enemyAnimator.SetBool("IsWalking", false);
            transform.position = actionTargetPos.position;
            transform.rotation = Quaternion.identity;
            IsPileUp = false;
            PileUpFinish = true;
        }
        else if (NeedJump && Vector3.Distance(transform.position, actionTargetPos.position) <= 1f)
        {
            enemyAnimator.SetBool("IsWalking", false);
            if (!IsJumping)
            {
                transform.DOJump(actionTargetPos.position + new Vector3(0,actionNum * 0.5f,0), 0.5f, 1, 0.5f).OnComplete(() => JumpFinish());
            }
            IsJumping = true;
        }
    }

    private void JumpFinish()
    {
        NeedJump = false;
        transform.position = actionTargetPos.position;
        transform.position += new Vector3(0,actionNum * 0.5f,0);
        transform.rotation = Quaternion.identity;
        IsPileUp = false;
        IsJumping = false;
        PileUpFinish = true;
    }

    private void BuildBridge()
    {
        Vector3 targetPos = actionTargetPos.position + new Vector3(0,0,0.75f * actionNum);
        if (!BuildFinish)
        {
            enemyAnimator.SetBool("IsWalking", true);
            transform.LookAt(targetPos);
            transform.position += transform.forward * speed;
        }

        if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
        {
            enemyAnimator.SetBool("IsWalking", false);
            transform.position = targetPos;
            transform.rotation = Quaternion.identity;
            IsBuildBridge = false;
            BuildFinish = true;
        }
    }

    public void JumpToEndPoint(Vector3 actionEndPos)
    {
        transform.DOJump(actionEndPos, 0.5f, 1, 0.5f);
    }
}