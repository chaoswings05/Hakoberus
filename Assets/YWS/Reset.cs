using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float DS4UD = Input.GetAxisRaw("DS4UpDown");

        if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.P))
        {
            SoundManager.Instance.StopBGM();
            SceneManager.LoadScene("TitleScene");
        }

        if (DS4UD > 0 && Input.GetButtonDown("DS4△"))
        {
            SoundManager.Instance.StopBGM();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
