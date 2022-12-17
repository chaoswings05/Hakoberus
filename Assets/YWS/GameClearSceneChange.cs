using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearSceneChange : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("DS4x"))
        {
            ReturnTitle();
        }
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
