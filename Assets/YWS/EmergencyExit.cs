using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmergencyExit : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
