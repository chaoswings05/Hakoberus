using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class EnemyActionScript : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
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
        // �ז��Ȃ̂ł�������O���Ƃ�
        this.GetComponent<EnemyFollowScript>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;
        anim.enabled = true;

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        if (anim != null)
        {
            anim.SetBool("Bridge1", true);
        }

        yield return new WaitForSeconds(12.0f);

        // �O�����R���|�[�l���g���ēxON
        this.GetComponent<EnemyFollowScript>().enabled = true;
        this.GetComponent<NavMeshAgent>().enabled = true;
        Destroy(anim);
    }
}
