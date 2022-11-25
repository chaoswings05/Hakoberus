using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class EnemyActionScript : MonoBehaviour
{
    // memo
    // Animator��TargetMatching���g���\��(�c�ς�)


    // �ԃn�R�x���X�p���X�g
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    // �ԃn�R�x���X�p�z��
    [SerializeField]
    GameObject[] RedEnemy;

    // �c��Q�̃u���b�N
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
       

        // RedEnemy�I�u�W�F�N�g�ƁA�^�O:Enemy��R�Â���
        RedEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        // RedEnemy�̒��g��enemy���X�g�ɓ����
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

    //// �ԃn�R�x���X�����ɓ��������������X�N���v�g
    //// ���S��������
    //public void OnCollisionEnter(Collision collision)
    //{
    //    // �^�O:Enemy��enemys�z��ɓ����
    //    GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

    //    // Bone�Ƃ����^�O�ɓ���������
    //    if (collision.gameObject.tag == "Bone")
    //    {
    //        // enemys�ɓ����Ă���I�u�W�F�N�g��red�Ɩ��t���Ă���S�ď���
    //        foreach(GameObject red in enemys)
    //        {
    //            Destroy(red);
    //        }
    //    }
    //}

    // ���� �ςݗ��Ă܂ł͓�����
    public void OnTriggerStay(Collider other)
    {
        GameObject[] nav = GameObject.FindGameObjectsWithTag("Enemy");

        Debug.Log("haitta");

        if (Input.GetKey(KeyCode.Space))
        {
            // �b���𐔂���
            actionPush += Time.deltaTime;

            // 3�b�o������
            if (actionPush >= push1)
            {
                Debug.Log("3�b�o��");

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

        // �ȒP�ȕ\�L�ɂ��邽�߂�
        Vector3 firstPos = enemyList[0].transform.position;
        Vector3 h_brockPos = HighBrock.transform.position;

        // �G�l�~�[�̈ړ�
        enemyList[0].transform.position = new Vector3(h_brockPos.x, h_brockPos.y, h_brockPos.z - enemyZ);
        enemyList[1].transform.position = new Vector3(firstPos.x, firstPos.y + enemyY, firstPos.z);
        Debug.Log("�A�N�V����");

        // enemy�̃X�N���v�g��AI�R���|�[�l���g��false
        foreach (GameObject obj in nav)
        {
            obj.GetComponent<EnemyFollowScript>().enabled = false;
            obj.GetComponent<NavMeshAgent>().enabled = false;
        }

        yield return new WaitForSeconds(3f);

        // Plyer�Ɛԃn�R�x���X�̏�Q����Ɉړ�
        this.transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);
        enemyList[0].transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);
        enemyList[1].transform.position = new Vector3(h_brockPos.x, h_brockPos.y + 1f, h_brockPos.z);

        yield return new WaitForSeconds(1f);

        // �O�����R���|�[�l���g���ēxON
        foreach (GameObject obj in nav)
        {
            obj.GetComponent<EnemyFollowScript>().enabled = true;
            obj.GetComponent<NavMeshAgent>().enabled = true;

        }
    }



}
