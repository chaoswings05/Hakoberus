using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowScript : MonoBehaviour
{
    [SerializeField]
    GameObject player;          // TargetObject

    [SerializeField]
    float StopDis = 1f,         // �v���C���[�Ɍ��E�܂ŋ߂Â��鋗��
          AgeSpeed = 1f;        // �ԃn�R�x���X�̑��x

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
        // player�܂ł̋�����ݒ�
        age.SetDestination(player.transform.position);

        // ���̋����܂ŋ߂Â�����~�܂�
        age.stoppingDistance = StopDis;
    }

    public void OnTriggerEnter(Collider other)
    {
        transform.parent = GameObject.Find("EnemyManager").transform;
    }
}
