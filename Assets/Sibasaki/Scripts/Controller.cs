using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //移動する速さ
    [SerializeField] float speed;

    [SerializeField] float jumpPower;

    //重力量
    [SerializeField] float gravSpeed;

    //今現在追従しているピクミンのリスト
    public List<Pikmin> followingPikmins;

    //ピクミンのプレハブ
    [SerializeField] GameObject PikminPrefab;

    //ピクミンが向かってくる場所
    [SerializeField] Transform playerGathPos;

    //ピクミンが帰る家の座標
    [SerializeField] Transform onionPos;

    //Playerが次のギミックに行ったらピクミンが生成される場所
    [SerializeField] Transform onionPos2;

    
    //骨をピクミンが持っていく場所
    [SerializeField] Transform boneDestroyPos;

    //ピクミンが積み上げる場所の取得
    [SerializeField] Transform pileUpPos;

    //Rigidbodyの取得
    [SerializeField] Rigidbody rigid;

    [SerializeField] Pikmin pikmin;

    public LayerMask groundLayers;
    public Transform groungCheckPoint;
    public Transform boxDog_01;
    
    void Start()
    {
        
    }
    void Update()
    {
        
        //もしXキーを押したら
        if (Input.GetKeyDown(KeyCode.X))
        {
            Pikmin pik = Instantiate(PikminPrefab, onionPos.position, Quaternion.identity).GetComponent<Pikmin>();
            pik.follow = true;//Playerに追従をON
            pik.PlayerGathPos = playerGathPos;
            pik.homePos = onionPos;//帰る家をonionに設定
            pik.pileUpPos = pileUpPos;//積み上げる場所をpileUpPosに設定
            pik.BoneDestroyPos = boneDestroyPos;//骨を捨てに行く場所の設定
            pik.controller = this;//
            pik.inArea = false;
            followingPikmins.Add(pik);
            
        }

        float x = Input.GetAxisRaw("Horizontal") * speed;//横
        float z = Input.GetAxisRaw("Vertical") * speed;//前

//Playerの移動------------- \
        
        //Spaceキーを押したらジャンプする
        if(IsGround() && Input.GetKeyDown(KeyCode.Space))
        {
            //ジャンプ
            rigid.AddForce(new Vector3(0, jumpPower, speed = 2), ForceMode.Impulse);  
            gravSpeed = 1;
        }
        else if(IsGround())
        {
            speed = 2;
        } 

        if(followingPikmins.Count == 0)//追従しているピクミンがいない時//ピクミンが多いほど足が遅くなる処理
        {
            
        }
        else if(followingPikmins.Count >= 1 && followingPikmins.Count < 2)//追従しているピクミンが1匹の時
        {
            speed = 1.6f;
        }
        else if(followingPikmins.Count >= 2 && followingPikmins.Count < 3)//追従しているピクミンが2匹の時
        {
            speed = 1.2f;
            //gravSpeed = 0.6f; 
        }
        else if(followingPikmins.Count >= 3 && followingPikmins.Count < 4)//追従しているピクミンが1匹の時
        {
            speed = 1f;
            //gravSpeed = 0.3f;
        }
    
        


        //Playerの座標を計算
        Vector3 direction = transform.position + new Vector3(x,0,z) * speed;        
        //移動下方向にPlayerの向きを変更する
        transform.LookAt(direction);
        
        
        rigid.velocity = (new Vector3(x,0,z) * speed + Vector3.down * gravSpeed);
//-------------
        
    }
    
    //Colliderになにか当たったら
    public void OnTriggerEnter(Collider other)
    {
        Pikmin pikmin = GetComponent<Pikmin>();
        //Playerがあたったら
        if(other.tag == "ActionPoint")
        {
            Debug.Log("範囲内に入りました");
            //pikmin.inArea = true;
        }
        if(other.tag == "ActionPoint2")
        {
            Debug.Log("次の場所に入りました");
            Destroy(PikminPrefab);
            Pikmin pik = Instantiate(PikminPrefab, onionPos2.position, Quaternion.identity).GetComponent<Pikmin>();
            pik.follow = true;//Playerに追従をON
            pik.PlayerGathPos = playerGathPos;
            //pik.homePos = onionPos;//帰る家をonionに設定
            //pik.pileUpPos = pileUpPos;//積み上げる場所をpileUpPosに設定
            pik.BoneDestroyPos = boneDestroyPos;//骨を捨てに行く場所の設定
            pik.controller = this;//
            //pik.inArea = false;
            followingPikmins.Add(pik);
        }
    }

    //地面判定用
    public bool IsGround()
    {
        return Physics.Raycast(groungCheckPoint.position, Vector3.down, 0.25f, groundLayers);
    }
    
    //[SerializeField] Camera camera;
    //private void LateUpdate()
    //{
        //camera.transform.LookAt(transform.position);
    //}
}
