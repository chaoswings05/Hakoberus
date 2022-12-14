using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestScript : MonoBehaviour
{
    [SerializeField]
    float _speed = 1.0f;

    private Rigidbody rb;

    private Vector3 playerPos;

    void Start()
    {
        playerPos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // ‘O
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector3(0, 0, _speed);
        }

        // Œã
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector3(0, 0, -_speed);
        }

        // ‰E
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector3(_speed, 0, 0);

        }

        // ¶
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector3(-_speed, 0, 0);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) ||
        Input.GetKeyUp(KeyCode.DownArrow) ||
        Input.GetKeyUp(KeyCode.RightArrow) ||
        Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.zero;
        }

        Vector3 diff = transform.position - playerPos;

        if(diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff);
        }

        playerPos = transform.position;
    }
}



