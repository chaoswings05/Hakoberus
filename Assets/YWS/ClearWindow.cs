using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ClearWindow : MonoBehaviour
{
    [SerializeField] private GameObject window = null;
    [SerializeField] private GameObject[] cursorArray = new GameObject[2];
    private int selectNum = 0;
    private bool IsWindowStandby = false;

    // Start is called before the first frame update
    void Start()
    {
        IsWindowStandby = false;
        window.SetActive(false);
        cursorArray[1].SetActive(false);
        cursorArray[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        float DS4LR = Input.GetAxisRaw("Horizontal");

        if (selectNum == 0)
        {
            if (DS4LR > 0)
            {
                SelectStageSelectButton();
            }

            if (Input.GetButtonDown("DS4x"))
            {
                TitleButtonOnClick();
            }
        }

        if (selectNum == 1)
        {
            if (DS4LR < 0)
            {
                SelectTitleButton();
            }

            if (Input.GetButtonDown("DS4x"))
            {
                StageSelectButtonOnClick();
            }
        }
    }

    public void WindowDeployment()
    {
        window.SetActive(true);
        window.transform.DOScale(Vector3.one, 1f).OnComplete(() => IsWindowStandby = true);
    }

    public void SelectTitleButton()
    {
        if (IsWindowStandby)
        {
            selectNum = 0;
            cursorArray[1].SetActive(false);
            cursorArray[0].SetActive(true);
        }
    }

    public void SelectStageSelectButton()
    {
        if (IsWindowStandby)
        {
            selectNum = 1;
            cursorArray[0].SetActive(false);
            cursorArray[1].SetActive(true);
        }
    }

    public void TitleButtonOnClick()
    {
        if (IsWindowStandby)
        {
            SoundManager.Instance.StopBGM();
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void StageSelectButtonOnClick()
    {
        if (IsWindowStandby)
        {
            SoundManager.Instance.StopBGM();
            SceneManager.LoadScene("StageSelectScene");
            SoundManager.Instance.PlayBGM(0);
        }
    }
}
