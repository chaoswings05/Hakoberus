using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //移動する速さ
    [SerializeField] float speed;

    //回転度
    [SerializeField] float rotSpeed;

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
    
    //骨をピクミンが持っていく場所
    [SerializeField] Transform boneDestroyPos;

    //ピクミンが積み上げる場所の取得
    [SerializeField] Transform pileUpPos;

    //Rigidbodyの取得
    [SerializeField] Rigidbody rigid;
    
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
            followingPikmins.Add(pik);
        }

        float x = Input.GetAxisRaw("Horizontal") * speed;
        float z = Input.GetAxisRaw("Vertical") * speed;

        //Spaceキーを押したらジャンプする
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //ジャンプ
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }

//Playerの移動-------------
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
        //Playerがあたったら
        if(other.tag == "ActionPoint")
        {
            Debug.Log("範囲内に入りました");
        }
    }

    
    
    //[SerializeField] Camera camera;
    //private void LateUpdate()
    //{
        //camera.transform.LookAt(transform.position);
    //}
}
