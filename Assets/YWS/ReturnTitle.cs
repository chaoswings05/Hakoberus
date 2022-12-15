using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTitle : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("DS4x"))
        {
            SceneChange_GameClearToTitle();
        }
    }

    public void SceneChange_GameClearToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
