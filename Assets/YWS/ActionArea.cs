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
    [SerializeField] private Text NeedNumDisplay = null;
    public int PayedCost = 0;
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
        NeedNumDisplay.text = PayedCost.ToString() + " / " + needNum.ToString();
    }

    public void ActionFinish()
    {
        StartCoroutine(ResetPayedCost());
    }

    private IEnumerator ResetPayedCost()
    {
        yield return new WaitForSeconds(0.5f);

        PayedCost = 0;
        blockingActivate();
    }

    private void blockingActivate()
    {
        blockingWall.SetActive(true);
    }
}
