using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionScript : MonoBehaviour
{
    // �ԃn�R�x���X�p���X�g
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    // �ԃn�R�x���X�p�z��
    [SerializeField]
    GameObject[] RedEnemy;

    int enemyCount;

    float actionPush = 0f,
          push1 = 3;


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

        enemyCount = enemyList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(actionPush);
    }

    // �ԃn�R�x���X�����ɓ��������������X�N���v�g
    // ���S��������
    public void OnCollisionEnter(Collision collision)
    {
        // �^�O:Enemy��enemys�z��ɓ����
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        // Bone�Ƃ����^�O�ɓ���������
        if (collision.gameObject.tag == "Bone")
        {
            // enemys�ɓ����Ă���I�u�W�F�N�g��red�Ɩ��t���Ă���S�ď���
            foreach(GameObject red in enemys)
            {
                Destroy(red);
            }
        }
    }

    // ����
    public void OnTriggerStay(Collider other)
    {
        Debug.Log("haitta");

        if (Input.GetKey(KeyCode.Space))
        {
            actionPush += Time.deltaTime;
        }

        if (actionPush >= push1)
        {
            Debug.Log("����������");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            actionPush = 0;
        }

    }


}
