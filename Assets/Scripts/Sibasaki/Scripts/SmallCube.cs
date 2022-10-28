using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SmallCube : MonoBehaviour
{
    public Transform target;//追跡するPlayer

    NavMeshAgent agent;//AIを実装するEnemy
    //Animator animator;//アニメーターを取得

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;//destination=目的地
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;//destination=目的地
        //animator.SetFloat("Distance", agent.remainingDistance);
    }
}