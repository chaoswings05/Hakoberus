using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField, Header("ステージイメージ")]
    GameObject i_stage1;

    [SerializeField]
    GameObject i_stage2;

    [SerializeField, Header("カーソルイメージ")]
    GameObject i_cursor1;

    [SerializeField]
    GameObject i_cursor2;

    Button _button;
    // Start is called before the first frame update
    void Start()
    {
        i_stage1.SetActive(false);
        i_stage2.SetActive(false);
        i_cursor1.SetActive(false);
        i_cursor2.SetActive(false);

        _button = GameObject.Find("Stage1Button").GetComponent<Button>();
        // 最初からボタンを選択している状態
        _button.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // マウスカーソルをボタンの上に置いたら画像1表示
    public void OnEnterStage1()
    {
        i_stage1.SetActive(true);
        i_cursor1.SetActive(true);

    }

    // マウスカーソルをボタンの上に置いたら画像2表示
    public void OnEnterStage2()
    {
        i_stage2.SetActive(true);
        i_cursor2.SetActive(true);

    }

    // マウスカーソルをボタンから離したら画像1を非表示
    public void OnExitStage1()
    {
        i_stage1.SetActive(false);
        i_cursor1.SetActive(false);
    }

    // マウスカーソルをボタンから離したら画像2を非表示
    public void OnExitStage2()
    {
        i_stage2.SetActive(false);
        i_cursor2.SetActive(false);
    }

    // ステージ1へ
    public void OnClickStage1()
    {
        Debug.Log("ステージ1へ");
        SceneManager.LoadScene("Stage1");
    }

    // ステージ2へ
    public void OnClickStage2()
    {
        Debug.Log("ステージ2へ");
        SceneManager.LoadScene("Stage2");
    }

}
