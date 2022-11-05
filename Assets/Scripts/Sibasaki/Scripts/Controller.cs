using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float speed;//移動する速さ

    [SerializeField] float jumpPower = 200f;

    [SerializeField] float rotSpeed;
    [SerializeField] float gravSpeed;

    public List<Pikmin> followingPikmins;//今現在追従しているピクミンのリスト

    [SerializeField] GameObject PikminPrefab;//ピクミンのプレハブ

    [SerializeField] Transform playerGathPos;//ピクミンが向かってくる場所

    [SerializeField] Transform onionPos;//ピクミンが帰る家の座標
    
    [SerializeField] Transform boneDestroyPos;

    [SerializeField] Transform pileUpPos;//ピクミンが積み上げる場所の取得

    [SerializeField] Rigidbody rigid;//Rigidbodyの取得
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))//もしXキーを押したら
        {
            Pikmin pik = Instantiate(PikminPrefab, onionPos.position, Quaternion.identity).GetComponent<Pikmin>();
            pik.follow = true;//Playerに追従をON
            pik.PlayerGathPos = playerGathPos;
            pik.homePos = onionPos;//帰る家をonionに設定
            pik.pileUpPos = pileUpPos;//積み上げる場所をpileUpPosに設定
            pik.BoneDestroyPos = boneDestroyPos;
            pik.controller = this;//
            followingPikmins.Add(pik);

        }

        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;

        //Spaceキーを押したらジャンプする
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(new Vector3(0, jumpPower, 0));
            //transform.position = (transform.forward * y + transform.right * x);
        }

        rigid.velocity = (transform.forward * y + Vector3.down * gravSpeed);
        //transform.Rotate(new Vector3(0, x, 0) * rotSpeed * Time.deltaTime);

        //自分の位置 + 速度
        Vector3 direction = transform.position + new Vector3(x,0,y) * speed;
        //移動した方向にPlayerの向きを変更する
        //transform.LookAt(direction);
        //速度設定
        rigid.velocity = new Vector3(x,0,y) * speed;//moveSpeedで速度の調整

    }


    
    //[SerializeField] Camera camera;
    //private void LateUpdate()
    //{
        //camera.transform.LookAt(transform.position);
    //}
}
