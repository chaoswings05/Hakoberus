using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalMessageDisplay : MonoBehaviour
{
    [SerializeField] private Text displayText = null;
    [SerializeField] private PlayerController player = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.followingEnemy.Count == 0)
        {
            displayText.gameObject.SetActive(true);
        }
    }
}
