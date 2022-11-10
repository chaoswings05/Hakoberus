using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pikmin : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;// NavMeshAgentの取得
    

    public Controller controller;//Controllerスクリプトの追加
    public ObjectController targetObject;//ObjectControllerスクリプトの取得
    public Transform homePos;//ピクミンのカエル家の座標の取得
    public Transform PlayerGathPos;//PlayerGathPosの位置情報の取得
    public Transform pileUpPos;//ピクミンを積み上げる場所の取得
    public ActionPoint actionPoint;//ActionPointスクリプトの取得
    public PileUpPoint pileUpPoint;//PileUpPointスクリプトの取得

    public BoneController Bone;//BoneControllerスクリプトの取得
    
    public Transform BoneDestroyPos;//骨を持って行く場所
    
    public bool goHome;//ピクミンが家に帰るかどうか

    public  bool follow;//Playerに追従するかどうか

    public bool BoneDestroy;//持っている骨を壊すかどうか
    



    private void Update()
    {
        if (follow)//もしPlayerに追従するなら
        {
            agent.SetDestination(PlayerGathPos.position);//目的地をPlayerGathPosにする
        }

        if (goHome)//もし家に帰るなら
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

        if(BoneDestroy)//骨を消す場所に行く時
        {
            agent.SetDestination(BoneDestroyPos.transform.position);//骨を消す場所に行く
            
            if (Vector3.Distance(transform.position, BoneDestroyPos.transform.position) <= 0.3f)//もしBoneDestroyPosの距離が0.75より小さくなったら
            {
                Destroy(Bone.gameObject);//骨を消す
                Destroy(gameObject);//自分を消す
            }
            return;
        }


        if(Bone != null)//もしBoneになにか入っている場合
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

        if(pileUpPoint != null)//もし積み上げるなら(ピクミンはHomePosには戻らない)
        {
            agent.SetDestination(pileUpPos.position);//ピクミンの目的地を積み上げる場所にする
            if(Vector3.Distance(transform.position, pileUpPos.transform.position) <= 0.2f)//もしピクミンがくっついたら
            {
                agent.speed = 0f;
                //pileUpPoint.transform.SetParent(transform);//pileUpPointとピクミンをくっつける
            }
            
        }

        if(targetObject != null)//もしtargetObjectになにか入っている場合
        {
            agent.stoppingDistance = 0;
            agent.SetDestination(targetObject.transform.position);//目的地をtargetObjectにする
            if (Vector3.Distance(transform.position, targetObject.transform.position) <= 0.75f)//もしargetObjectの距離が0.75より小さくなったら
            {
                targetObject.transform.position = transform.position + Vector3.forward * 0.9f;//targetObjectを上にする
                targetObject.transform.SetParent(transform);//targetObjectとピクミンをくっつける
                Destroy(targetObject.GetComponent<Rigidbody>());//targetObjectのRigidbodyをはずす
                goHome = true;//家に向かう
            }
        }
    }
}
