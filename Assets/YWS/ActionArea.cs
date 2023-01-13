using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionArea : MonoBehaviour
{
    [SerializeField, Header("アクションを行う場所")] public Transform targetPoint = null;
    [SerializeField, Header("アクションが終了する場所")] public Transform endPoint = null;
    [Header("行うアクション")]
    public bool IsPileUp = false;
    public bool IsBuildBridge = false;
    [SerializeField, Header("必要赤ハコベロス数")] public int needNum = 2;
    [SerializeField] private GameObject blockingWall = null;

    // Start is called before the first frame update
    void Start()
    {
        if (blockingWall != null)
        {
            blockingWall.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActionFinish()
    {
        StartCoroutine(blockingActivate());
    }

    private IEnumerator blockingActivate()
    {
        yield return new WaitForSeconds(0.5f);

        blockingWall.SetActive(true);
    }
}
