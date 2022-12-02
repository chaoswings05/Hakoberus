using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class EnemyActionScript : MonoBehaviour
{
    // 赤ハコベロス用リスト
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    // 赤ハコベロス用配列
    [SerializeField]
    GameObject[] RedEnemy;

    //[SerializeField]
    //float targetDiff,       // 橋にするときに重ならないように引くやつ
    //      moveSpeed;        // 橋になるときのスピード

    //[SerializeField]
    //GameObject Bridge;        // 橋の透明ブロック


    private Animator anim;
    void Start()
    {
        //Bridge.SetActive(false);

        anim = GetComponent<Animator>();

        // RedEnemyオブジェクトと、タグ:Enemyを紐づける
        RedEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        // RedEnemyの中身をenemyリストに入れる
        foreach(GameObject obj in RedEnemy)
        {
            enemyList.Add(obj);
        }

    }

    void Update()
    {

    }

    // 実験 アニメーションで無理やり動かす
    
    public void BridgeAction()
    {
        StartCoroutine("EnemyBridge");
    }

   
    IEnumerator EnemyBridge()
    {
        GameObject[] nav = GameObject.FindGameObjectsWithTag("Enemy");

        //Bridge.SetActive(true);

        // リストを逆順(後ろから動くので)
        //enemyList.Reverse();

        // 邪魔なのでいったん外しとく
        foreach (GameObject obj in nav)
        {
            obj.GetComponent<EnemyFollowScript>().enabled = false;
            obj.GetComponent<NavMeshAgent>().enabled = false;

        }

        anim.SetBool("Enemy1", true);

        yield return new WaitForSeconds(1.5f);


        //// エネミーの移動
        //Debug.Log("アクション開始");

        //enemyList[0].transform.position = Vector3.MoveTowards(enemyList[0].transform.position, Bridge.transform.position, moveSpeed);


        //enemyList[1].transform.position = Vector3.MoveTowards(enemyList[1].transform.position, Bridge2.transform.position, moveSpeed);

        //yield return new WaitForSeconds(3f);

        //// Plyerと赤ハコベロスの障害物上に移動

        //yield return new WaitForSeconds(1f);

        //Bridge.SetActive(false);
        //// 外したコンポーネントを再度ON
        //foreach (GameObject obj in nav)
        //{
        //    obj.GetComponent<EnemyFollowScript>().enabled = true;
        //    obj.GetComponent<NavMeshAgent>().enabled = true;

        //}
    }
}
