using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField, Header("ステージ1イメージ")] private GameObject i_stage1 = null;
    [SerializeField, Header("ステージ2イメージ")] private GameObject i_stage2 = null;
    [SerializeField, Header("ステージ1カーソル")] private GameObject i_cursor1 = null;
    [SerializeField, Header("ステージ2カーソル")] private GameObject i_cursor2 = null;
    [SerializeField, Header("ステージ1ボタン")] private Button _button = null;

    // Start is called before the first frame update
    void Start()
    {
        i_stage1.SetActive(false);
        i_stage2.SetActive(false);
        i_cursor1.SetActive(false);
        i_cursor2.SetActive(false);

        // 最初からボタンを選択している状態
        _button.Select();
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
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Stage1");
    }

    // ステージ2へ
    public void OnClickStage2()
    {
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Stage2");
    }
}
