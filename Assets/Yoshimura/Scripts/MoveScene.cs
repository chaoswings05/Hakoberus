using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM(0);    
    }

    void Update()
    {
        if (Input.GetButtonDown("DS4x") || Input.GetMouseButtonDown(0))
        {
            OnClickStartButton();
        }
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
