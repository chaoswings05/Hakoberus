using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentativeBridgeAction_Enemy : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    // 実験 アニメーションで無理やり動かす
    public void BridgeAction()
    {
        StartCoroutine("EnemyBridge");
    }
   
    IEnumerator EnemyBridge()
    {
        // 邪魔なのでいったん外しとく
        this.GetComponent<Enemy>().IsFollow = false;
        anim.enabled = true;

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        if (anim != null)
        {
            anim.SetBool("Bridge1", true);
        }

        yield return new WaitForSeconds(12.0f);

        // 外したコンポーネントを再度ON
        this.GetComponent<Enemy>().IsFollow = true;
        Destroy(anim);
    }
}
