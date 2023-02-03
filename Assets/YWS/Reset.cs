using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("TitleScene");
        }

        if (z > 0 && Input.GetButtonDown("DS4△"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
