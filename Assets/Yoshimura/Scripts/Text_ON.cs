using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_ON : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.gameObject.SetActive(false);
        }
    }
}