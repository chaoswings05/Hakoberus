using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoint : MonoBehaviour
{
    Controller controller;
    
    private void Start()
    {
        controller = GameObject.Find("Player").GetComponent<Controller>();
    }

    //Colliderになにか当たったら
    public void OnTriggerEnter(Collider other)
    {
        //Playerがあたったら
        if(other.tag == "Player")
        {
            Debug.Log("Playerが当たりました");
        }
    }
}
