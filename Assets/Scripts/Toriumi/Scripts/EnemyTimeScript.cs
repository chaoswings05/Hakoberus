using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimeScript : MonoBehaviour
{
    List<GameObject> enemy;

    [SerializeField]
    GameObject[] RedEnemy;

    int enemyCount;

    float playerDelay;
    // Start is called before the first frame update
    void Start()
    {
        RedEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject obj in RedEnemy)
        {
            enemy.Add(obj);
        }

        // enemyCount = RedEnemy.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlayerDelay()
    {

        if (enemyCount == 2 || enemyCount == 3)
        {
            yield return new WaitForSeconds(0.1f);
            playerDelay += 0.1f;
            Debug.Log(playerDelay);
        }
    }
}
