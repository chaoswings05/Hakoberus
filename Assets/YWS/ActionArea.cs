using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionArea : MonoBehaviour
{
    [SerializeField, Header("アクションを行う場所")] public Transform targetPoint = null;
    [Header("行うアクション")]
    [SerializeField] public bool IsPileUp = false;
    [SerializeField, Header("必要赤ハコベロス数")] public int needNum = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
