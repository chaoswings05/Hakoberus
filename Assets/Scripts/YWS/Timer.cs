using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0.0f;
    [Header("制限時間")]public float timeLimit = 300f;
    [Header("時間に応じて変化するオブジェクト"), SerializeField] private Image controlObj = null;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        elapsedTime = 0.0f;
        controlObj.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < timeLimit)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
        }

        if (elapsedTime >= 60f && elapsedTime <= 180f)
        {
            controlObj.color = Color.yellow;
        }
        else if (elapsedTime >= 180f)
        {
            controlObj.color = Color.blue;
        }
    }
}
