using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField, Header("流すBGMの番号")] private int BGMNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMNum);
    }
}
