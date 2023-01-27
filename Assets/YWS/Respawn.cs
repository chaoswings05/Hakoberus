using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField, Header("復活地点")] private Transform respawnPoint = null;
    private PlayerController respawnTarget = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            respawnTarget = other.GetComponent<PlayerController>();
            respawnTarget.transform.position = respawnPoint.position;
            for (int i = 0; i < respawnTarget.followingEnemy.Count; i++)
            {
                respawnTarget.followingEnemy[i].transform.position = respawnPoint.position;
            }
            respawnTarget = null;
        }        
    }
}