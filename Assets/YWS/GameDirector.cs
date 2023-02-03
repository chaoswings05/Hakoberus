using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    protected static GameDirector instance;
    public static GameDirector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameDirector)FindObjectOfType(typeof(GameDirector));

                if (instance == null)
                {
                    Debug.LogError("GameDirector Instance Error");
                }
            }

            return instance;
        }
    }

    [SerializeField, Header("流すBGMの番号")] private int BGMNum = 0;
    public ActionArea AP = null;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMNum);
    }

    void Update()
    {
        
    }
}
