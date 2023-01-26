using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextFade : MonoBehaviour
{
    [SerializeField, Header("フェードを行うオブジェクト")] private Text targetText = null;
    Tweener fade;
    
    // Start is called before the first frame update
    void Start()
    {
        fade = targetText.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("DS4x") || Input.GetMouseButtonDown(0))
        {
            fade.Kill();
        }
    }
}