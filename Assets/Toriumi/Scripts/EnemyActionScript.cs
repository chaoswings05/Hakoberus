using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class EnemyActionScript : MonoBehaviour
{
    // �ԃn�R�x���X�p���X�g
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    // �ԃn�R�x���X�p�z��
    [SerializeField]
    GameObject[] RedEnemy;

    //[SerializeField]
    //float targetDiff,       // ���ɂ���Ƃ��ɏd�Ȃ�Ȃ��悤�Ɉ������
    //      moveSpeed;        // ���ɂȂ�Ƃ��̃X�s�[�h

    //[SerializeField]
    //GameObject Bridge;        // ���̓����u���b�N


    private Animator anim;
    void Start()
    {
        //Bridge.SetActive(false);

        anim = GetComponent<Animator>();

        // RedEnemy�I�u�W�F�N�g�ƁA�^�O:Enemy��R�Â���
        RedEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        // RedEnemy�̒��g��enemy���X�g�ɓ����
        foreach(GameObject obj in RedEnemy)
        {
            enemyList.Add(obj);
        }

    }

    void Update()
    {

    }

    // ���� �A�j���[�V�����Ŗ�����蓮����
    
    public void BridgeAction()
    {
        StartCoroutine("EnemyBridge");
    }

   
    IEnumerator EnemyBridge()
    {
        GameObject[] nav = GameObject.FindGameObjectsWithTag("Enemy");

        //Bridge.SetActive(true);

        // ���X�g���t��(��납�瓮���̂�)
        //enemyList.Reverse();

        // �ז��Ȃ̂ł�������O���Ƃ�
        foreach (GameObject obj in nav)
        {
            obj.GetComponent<EnemyFollowScript>().enabled = false;
            obj.GetComponent<NavMeshAgent>().enabled = false;

        }

        anim.SetBool("Enemy1", true);

        yield return new WaitForSeconds(1.5f);


        //// �G�l�~�[�̈ړ�
        //Debug.Log("�A�N�V�����J�n");

        //enemyList[0].transform.position = Vector3.MoveTowards(enemyList[0].transform.position, Bridge.transform.position, moveSpeed);


        //enemyList[1].transform.position = Vector3.MoveTowards(enemyList[1].transform.position, Bridge2.transform.position, moveSpeed);

        //yield return new WaitForSeconds(3f);

        //// Plyer�Ɛԃn�R�x���X�̏�Q����Ɉړ�

        //yield return new WaitForSeconds(1f);

        //Bridge.SetActive(false);
        //// �O�����R���|�[�l���g���ēxON
        //foreach (GameObject obj in nav)
        //{
        //    obj.GetComponent<EnemyFollowScript>().enabled = true;
        //    obj.GetComponent<NavMeshAgent>().enabled = true;

        //}
    }
}
