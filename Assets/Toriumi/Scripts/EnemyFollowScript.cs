using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowScript : MonoBehaviour
{
    [SerializeField]
    GameObject player;          // PlayerTarget

    [SerializeField]
    float arrivedDis = 1.5f,    // �v���C���[�ɓ�����������

          followDis = 1f;      // �v���C���[�Ɨ��ꂽ���ɒǂ��n�߂鋗��

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

        // player�Ƃ̋����܂ł̐ݒ�
        age.SetDestination(player.transform.position);

        // player�Ƃ̋������߂�������~�܂�
        if(age.remainingDistance < arrivedDis)
        {
            age.isStopped = true;
        }

        // �����łȂ���Γ���
        else if(age.remainingDistance > followDis)
        {
            age.isStopped = false;
        }
    }
}
