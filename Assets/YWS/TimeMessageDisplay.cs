using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMessageDisplay : MonoBehaviour
{
    [SerializeField] private Text displayText = null;
    [SerializeField, Header("メッセージが表示されるまでの秒数")] private float waitTime = 8f;
    private bool IsCountDownStart = false;
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCountDownStart)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= waitTime)
            {
                displayText.gameObject.SetActive(true);
                elapsedTime = 0f;
                IsCountDownStart = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsCountDownStart = true;
        }    
    }
}
