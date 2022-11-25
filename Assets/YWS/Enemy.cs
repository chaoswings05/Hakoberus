using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // NavMeshAgentの取得
    [SerializeField] NavMeshAgent agent = null;
    //Controllerスクリプトの追加
    public PlayerController controller = null;
    //PlayerGathPosの位置情報の取得
    public Transform PlayerGathPos = null;
    //ピクミンを積み上げる場所の取得
    public Transform pileUpPos = null;
    //ActionPointスクリプトの取得
    public ActionPoint actionPoint = null;
    //PileUpPointスクリプトの取得
    public PileUpPoint pileUpPoint = null;
    //BoneControllerスクリプトの取得
    public Bone bone = null;
    //骨を持って行く場所
    public Transform BoneDestroyPos = null;
    //Playerに追従するかどうか
    public bool follow = true;
    //持っている骨を壊すかどうか
    public bool BoneDestroy = false;

    private void Start()
    {

    }

    private void Update()
    {
        //もしPlayerに追従するなら
        if (follow)
        {
            agent.SetDestination(PlayerGathPos.position); //目的地をPlayerGathPosにする
        }

        //骨を消す場所に行く時
        if(BoneDestroy)
        {
            agent.SetDestination(BoneDestroyPos.transform.position);//骨を消す場所に行く
            
            if (Vector3.Distance(transform.position, BoneDestroyPos.transform.position) <= 0.3f)//もしBoneDestroyPosの距離が0.75より小さくなったら
            {
                Destroy(bone.gameObject);//骨を消す
                Destroy(gameObject);//自分を消す
            }
            return;
        }

        //もしBoneになにか入っている場合
        if(bone != null)
        {
            //agent.stoppingDistance = 0;
            //agent.SetDestination(bone.transform.position);//目的地をBoneにする
            //if (Vector3.Distance(transform.position, bone.transform.position) <= 0.4f)//もしBoneの距離が0.75より小さくなったら
            //{
                bone.transform.position = transform.position + Vector3.forward * 0.9f;//targetObjectを上にする
                bone.transform.SetParent(transform);//Boneとピクミンをくっつける
                Destroy(bone.GetComponent<Rigidbody>());//BoneのRigidbodyをはずす
                BoneDestroyPos = bone.boneDestroyPos;
                BoneDestroy = true;//家に向かう
            //}
        }

        //もし積み上げるなら(ピクミンはHomePosには戻らない)
        if(pileUpPoint != null)
        {
            agent.SetDestination(pileUpPos.position);//ピクミンの目的地を積み上げる場所にする
            if(Vector3.Distance(transform.position, pileUpPos.transform.position) <= 0.2f)//もしピクミンがくっついたら
            {
                agent.speed = 0f;
                //pileUpPoint.transform.SetParent(transform);//pileUpPointとピクミンをくっつける
            }
        }
    }
}
