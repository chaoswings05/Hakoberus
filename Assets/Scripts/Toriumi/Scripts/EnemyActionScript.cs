using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionScript : MonoBehaviour
{
    // 赤ハコベロス用リスト
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    // 赤ハコベロス用配列
    [SerializeField]
    GameObject[] RedEnemy;

    int enemyCount;

    float actionPush = 0f,
          push1 = 3;


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

        enemyCount = enemyList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(actionPush);
    }

    // 赤ハコベロスが骨に当たったら消えるスクリプト
    // ※全部消える
    public void OnCollisionEnter(Collision collision)
    {
        // タグ:Enemyをenemys配列に入れる
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        // Boneというタグに当たったら
        if (collision.gameObject.tag == "Bone")
        {
            // enemysに入っているオブジェクトにredと名付けてから全て消す
            foreach(GameObject red in enemys)
            {
                Destroy(red);
            }
        }
    }

    // 実験
    public void OnTriggerStay(Collider other)
    {
        Debug.Log("haitta");

        if (Input.GetKey(KeyCode.Space))
        {
            actionPush += Time.deltaTime;
        }

        if (actionPush >= push1)
        {
            Debug.Log("長押し完了");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            actionPush = 0;
        }

    }


}
