using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowScript : MonoBehaviour
{
    [SerializeField]
    GameObject player;          // PlayerTarget

    [SerializeField]
    float arrivedDis = 1.5f,    // プレイヤーに到着した距離

          followDis = 1f;      // プレイヤーと離れた時に追い始める距離

    private NavMeshAgent age;

    // Start is called before the first frame update
    void Start()
    {
        // distance = transform.position - player.transform.position;

        age = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        // transform.position = Vector3.Lerp(transform.position, player.transform.position + distance, speed * Time.deltaTime);

        // playerとの距離までの設定
        age.SetDestination(player.transform.position);

        // playerとの距離が近すぎたら止まる
        if(age.remainingDistance < arrivedDis)
        {
            age.isStopped = true;
        }

        // そうでなければ動く
        else if(age.remainingDistance > followDis)
        {
            age.isStopped = false;
        }
    }
}
