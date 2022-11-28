using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pikmin : MonoBehaviour
{
    // NavMeshAgentの取得
    [SerializeField] NavMeshAgent agent;
    
    //Controllerスクリプトの追加
    public Controller controller;

    //ObjectControllerスクリプトの取得
    public ObjectController targetObject;
    
    //ピクミンのカエル家の座標の取得
    public Transform homePos;
    
    //PlayerGathPosの位置情報の取得
    public Transform PlayerGathPos;
    
    //ピクミンを積み上げる場所の取得
    public Transform pileUpPos;
    
    //ActionPointスクリプトの取得
    public ActionPoint actionPoint;
    
    //PileUpPointスクリプトの取得
    public PileUpPoint pileUpPoint;
    
    //BoneControllerスクリプトの取得
    public BoneController Bone;
    
    //骨を持って行く場所
    public Transform BoneDestroyPos;
    
    //ピクミンが家に帰るかどうか
    public bool goHome;

    //Playerに追従するかどうか
    public  bool follow;
    //持っている骨を壊すかどうか
    public bool BoneDestroy;
    //範囲内にいるかどうか
    public bool inArea = false;
    

    void Start()
    {
       
    }

    private void Update()
    {
        //もしPlayerに追従するなら
        if (follow)
        {
            agent.SetDestination(PlayerGathPos.position);//目的地をPlayerGathPosにする
        }
        //もし家に帰るなら
        if (goHome)
        {
            agent.SetDestination(homePos.transform.position);//目的地をhomePosにする
            if(Vector3.Distance(transform.position, homePos.transform.position) <= 0.85f)//もしhomePosとの距離が0.85未満になったら
            {
                controller.followingPikmins.Add(this);
                follow = true;
                goHome = false;
                Destroy(targetObject.gameObject);
            }
            return;
        }

       //アクションポイントの範囲内に入っているなら
        //if(inArea == true)
        //{     
            //骨を消す場所に行く時
            if(BoneDestroy)
            {
                agent.SetDestination(BoneDestroyPos.transform.position);//骨を消す場所に行く
            
                if (Vector3.Distance(transform.position, BoneDestroyPos.transform.position) <= 0.3f)//もしBoneDestroyPosの距離が0.75より小さくなったら
                {
                    Destroy(Bone.gameObject);//骨を消す
                    Destroy(gameObject);//自分を消す
                }
                return;
            }

            //もしBoneになにか入っている場合
            if(Bone != null)
            {
                //agent.stoppingDistance = 0;
                agent.SetDestination(Bone.transform.position);//目的地をBoneにする
                if (Vector3.Distance(transform.position, Bone.transform.position) <= 0.4f)//もしBoneの距離が0.75より小さくなったら
                {
                    Bone.transform.position = transform.position + Vector3.forward * 0.9f;//targetObjectを上にする
                    Bone.transform.SetParent(transform);//Boneとピクミンをくっつける
                    Destroy(Bone.GetComponent<Rigidbody>());//BoneのRigidbodyをはずす
                    BoneDestroy = true;//家に向かう
                }
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
            //もしtargetObjectになにか入っている場合
            if(targetObject != null)
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(targetObject.transform.position);//目的地をtargetObjectにする
                if (Vector3.Distance(transform.position, targetObject.transform.position) <= 0.75f)//もしargetObjectの距離が0.75より小さくなったら
                {
                    //targetObject.transform.position = transform.position + Vector3.forward * 0.9f;//targetObjectを上にする
                    targetObject.transform.SetParent(transform);//targetObjectとピクミンをくっつける
                    //Destroy(targetObject.GetComponent<Rigidbody>());//targetObjectのRigidbodyをはずす
                    goHome = true;//家に向かう
                }
            }
        //}
    }

    //ピクミンを破壊するスクリプト
    public void PikminDestroy()
    {
        Destroy(this.gameObject);
    }
}
