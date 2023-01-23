using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField]
    GameObject i_stage1,
               i_stage2;
    // Start is called before the first frame update
    void Start()
    {
        i_stage1.SetActive(false);
        i_stage2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // マウスカーソルをボタンの上に置いたら画像1表示
    public void OnEnterStage1()
    {
        i_stage1.SetActive(true);
    }

    // マウスカーソルをボタンの上に置いたら画像2表示
    public void OnEnterStage2()
    {
        i_stage2.SetActive(true);
    }

    // マウスカーソルをボタンから離したら画像1を非表示
    public void OnExitStage1()
    {
        i_stage1.SetActive(false);
    }

    // マウスカーソルをボタンから離したら画像2を非表示
    public void OnExitStage2()
    {
        i_stage2.SetActive(false);
    }

    // ステージ1へ
    public void OnClickStage1()
    {
        Debug.Log("ステージ1へ");
        // SceneManager.LoadScene("Stage1");
    }

    // ステージ2へ
    public void OnClickStage2()
    {
        Debug.Log("ステージ2へ");
        // SceneManager.LoadScene("Stage2");
    }

}
