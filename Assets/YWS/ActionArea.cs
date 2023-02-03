using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionArea : MonoBehaviour
{
    [Header("アクションを行う場所")] public List<Transform> actionPoint = new List<Transform>();
    [Header("歩いて通る場所")] public List<Transform> walkPoint = new List<Transform>();
    [Header("行うアクション")]
    public bool IsPileUp = false;
    public bool IsBuildBridge = false;
    [Header("アクションを行う時の正面は")] public float forward = 0f;
    [Header("必要赤ハコベロス数")] public int needNum = 2;
    public bool NeedBlock = false;
    [SerializeField] private GameObject blockingWall = null;

    // Start is called before the first frame update
    void Start()
    {
        if (blockingWall != null)
        {
            blockingWall.SetActive(false);
        }
    }

    public void blockingActivate()
    {
        blockingWall.SetActive(true);
    }
}
