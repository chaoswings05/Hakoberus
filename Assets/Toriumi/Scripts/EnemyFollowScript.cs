using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowScript : MonoBehaviour
{
    [SerializeField]
    GameObject player;          // TargetObject

    [SerializeField]
    float StopDis = 1f,         // プレイヤーに限界まで近づける距離
          AgeSpeed = 1f;        // 赤ハコベロスの速度

    private NavMeshAgent age;

    // Start is called before the first frame update
    void Start()
    {
        age = GetComponent<NavMeshAgent>();
        age.speed = AgeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        // playerまでの距離を設定
        age.SetDestination(player.transform.position);

        // この距離まで近づいたら止まる
        age.stoppingDistance = StopDis;
    }

    public void OnTriggerEnter(Collider other)
    {
        transform.parent = GameObject.Find("EnemyManager").transform;
    }
}
