using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class EnemyActionScript : MonoBehaviour
{
    // memo
    // AnimatorのTargetMatchingを使う予定(縦積み)


    // 赤ハコベロス用リスト
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    // 赤ハコベロス用配列
    [SerializeField]
    GameObject[] RedEnemy;

    // 縦障害のブロック
    [SerializeField]
    Transform HighBrock;

    float actionPush = 0f,
          push1 = 3;

    [SerializeField]
    float enemyY = 0.5f,
          enemyZ = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
       

        // RedEnemyオブジェクトと、タグ:Enemyを紐づける
        RedEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        // RedEnemyの中身をenemyリストに入れる
        foreach(GameObject obj in RedEnemy)
        {
            enemyList.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(actionPush);
    }

    //// 赤ハコベロスが骨に当たったら消えるスクリプト
    //// ※全部消える
    //public void OnCollisionEnter(Collision collision)
    //{
    //    // タグ:Enemyをenemys配列に入れる
    //    GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

    //    // Boneというタグに当たったら
    //    if (collision.gameObject.tag == "Bone")
    //    {
    //        // enemysに入っているオブジェクトにredと名付けてから全て消す
    //        foreach(GameObject red in enemys)
    //        {
    //            Destroy(red);
    //        }
    //    }
    //}

    // 実験 積み立てまでは動いた
    public void OnTriggerStay(Collider other)
    {
        GameObject[] nav = GameObject.FindGameObjectsWithTag("Enemy");

        Debug.Log("haitta");

        if (Input.GetKey(KeyCode.Space))
        {
            // 秒数を数える
            actionPush += Time.deltaTime;

            // 3秒経ったら
            if (actionPush >= push1)
            {
                Debug.Log("3秒経過");

                StartCoroutine("EnemyHighMove");
            }

        }

        else if(!Input.GetKeyUp(KeyCode.Space))
        {
            actionPush = 0;

        }
    }

    IEnumerator EnemyHighMove()
    {
        GameObject[] nav = GameObject.FindGameObjectsWithTag("Enemy");

        // 簡単な表記にするために
        Vector3 firstPos = enemyList[0].transform.position;
        Vector3 h_brockPos = HighBrock.transform.position;

        // エネミーの移動
        enemyList[0].transform.position = new Vector3(h_brockPos.x, h_brockPos.y, h_brockPos.z - enemyZ);
        enemyList[1].transform.position = new Vector3(firstPos.x, firstPos.y + enemyY, firstPos.z);
        Debug.Log("アクション");

        // enemyのスクリプトとAIコンポーネントをfalse
        foreach (GameObject obj in nav)
        {
            obj.GetComponent<EnemyFollowScript>().enabled = false;
            obj.GetComponent<NavMeshAgent>().enabled = false;
        }

        yield return new WaitForSeconds(3f);

        // Plyerと赤ハコベロスの障害物上に移動
        this.transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);
        enemyList[0].transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);
        enemyList[1].transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);

        yield return new WaitForSeconds(1f);

        // 外したコンポーネントを再度ON
        foreach (GameObject obj in nav)
        {
            obj.GetComponent<EnemyFollowScript>().enabled = true;
            obj.GetComponent<NavMeshAgent>().enabled = true;

        }
    }



}
