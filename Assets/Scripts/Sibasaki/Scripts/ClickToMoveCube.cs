using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// AI 関連のライブラリを使用するための宣言
using UnityEngine.AI;
 
public class ClickToMoveCube : MonoBehaviour
{
    // NavMeshAgent コンポーネントを格納する変数
    NavMeshAgent agent;
 
    //ゲーム開始時の処理
    void Start()
    {
        // 変数 agemt に NavMeshAgent コンポーネントを格納する
        agent = GetComponent<NavMeshAgent>();
    }
 
    //ゲーム実行中の処理
    void Update()
    { 
        // マウス左クリックされたとき
        if (Input.GetMouseButtonDown(0))
        {
            // カメラから発射したレイが衝突したオブジェクトを格納する変数 hit を宣言
            RaycastHit hit;
            // カメラからマウスでクリックした方向にレイを発射して ray に格納
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // もし発射したレイがコライダーと衝突したら（衝突検知最大距離 100）
            if (Physics.Raycast(ray, out hit, 100))
            {
                // 変数 hit の座標を NavMeshAgent の目標点に渡す
                agent.destination = hit.point;
            }
        }
    }
}