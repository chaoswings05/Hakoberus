using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField, Header("ウインドウ本体")] private GameObject window = null;
    [SerializeField, Header("ウインドウの背景画像")] private Image windowBG = null;
    [SerializeField, Header("ウインドウのテキスト")] private Text text = null;
    public bool IsGameStart = false;
    private bool IsFadeStart = false;
    [SerializeField, Header("ウインドウが消えるまでの時間")] private float fadeTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMNum);
        IsGameStart = false;
        IsFadeStart = false;
    }

    void Update()
    {
        if (!IsFadeStart)
        {
            if (Input.GetButtonDown("DS4x") || Input.GetKeyDown(KeyCode.Space))
            {
                text.DOFade(0f, fadeTime);
                windowBG.DOFade(0f, fadeTime).OnComplete(()=> DeleteWindow());
                IsFadeStart = true;
            }
        }
    }

    private void DeleteWindow()
    {
        Destroy(window);
        IsGameStart = true;
    }
}
