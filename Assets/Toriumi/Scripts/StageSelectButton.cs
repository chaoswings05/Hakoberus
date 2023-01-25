using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W�C���[�W")]
    GameObject i_stage1;

    [SerializeField]
    GameObject i_stage2;

    [SerializeField, Header("�J�[�\���C���[�W")]
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
        // �ŏ�����{�^����I�����Ă�����
        _button.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �}�E�X�J�[�\�����{�^���̏�ɒu������摜1�\��
    public void OnEnterStage1()
    {
        i_stage1.SetActive(true);
        i_cursor1.SetActive(true);

    }

    // �}�E�X�J�[�\�����{�^���̏�ɒu������摜2�\��
    public void OnEnterStage2()
    {
        i_stage2.SetActive(true);
        i_cursor2.SetActive(true);

    }

    // �}�E�X�J�[�\�����{�^�����痣������摜1���\��
    public void OnExitStage1()
    {
        i_stage1.SetActive(false);
        i_cursor1.SetActive(false);
    }

    // �}�E�X�J�[�\�����{�^�����痣������摜2���\��
    public void OnExitStage2()
    {
        i_stage2.SetActive(false);
        i_cursor2.SetActive(false);
    }

    // �X�e�[�W1��
    public void OnClickStage1()
    {
        Debug.Log("�X�e�[�W1��");
        // SceneManager.LoadScene("Stage1");
    }

    // �X�e�[�W2��
    public void OnClickStage2()
    {
        Debug.Log("�X�e�[�W2��");
        // SceneManager.LoadScene("Stage2");
    }

}
