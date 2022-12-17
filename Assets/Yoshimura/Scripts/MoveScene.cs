using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("DS4x"))
        {
            OnClickStartButton();
        }
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Stage1");
    }
}
