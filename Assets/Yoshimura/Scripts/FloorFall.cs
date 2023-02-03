using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFall : MonoBehaviour
{
    [SerializeField, Header("����ԃn�R�x���X�̏��")] int fallcount = 0;
    [SerializeField] private PlayerController playerObj = null;
    [SerializeField, Header("�v���C���[�̕����n�_")] private Transform respawnPoint = null;
    [SerializeField] private Rigidbody rb = null;
    private Vector3 thisPos = Vector3.zero;
    [SerializeField, Header("���̕��󏰂���������܂ł̎���")] private float respawnCountDownTime = 0f;
    private float elapsedTime = 0f;
    private bool IsRespawnCountDownStart = false;

    void Start()
    {
        thisPos = this.transform.position;   
    }

    void Update()
    {
        if (IsRespawnCountDownStart)
        {      
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= respawnCountDownTime)
            {
                transform.position = thisPos;
                rb.isKinematic = true;
                playerObj.transform.position = respawnPoint.position;
                for (int i = 0; i < playerObj.followingEnemy.Count; i++)
                {
                    playerObj.followingEnemy[i].transform.position = respawnPoint.position;
                }
                elapsedTime = 0f;
                IsRespawnCountDownStart = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && playerObj.followingEnemy.Count > fallcount)
        {
            rb.isKinematic = false;
            IsRespawnCountDownStart = true;
        }
    }
}