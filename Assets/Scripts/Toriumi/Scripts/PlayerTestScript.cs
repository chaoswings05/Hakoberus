using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestScript : MonoBehaviour
{
    [SerializeField]
    float speed =1.0f;

    Rigidbody rb;

    Vector3 player_Pos;

    // Start is called before the first frame update
    void Start()
    {
        player_Pos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // ëO
        if(Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector3(0, 0, speed);
        }

        // å„
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector3(0, 0, -speed);
        }

        // âE
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector3(speed, 0, 0);

        }

        // ç∂
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector3(-speed, 0, 0);
        }

        if(Input.GetKeyUp(KeyCode.UpArrow) ||
           Input.GetKeyUp(KeyCode.DownArrow) ||
           Input.GetKeyUp(KeyCode.RightArrow) ||
           Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.zero;
        }
    }
}

