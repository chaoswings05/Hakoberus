using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileUpPoint : MonoBehaviour
{
    public bool selected;//カーソルで押されたかどうか
    Controller controller;
    private void Start()
    {
        controller = GameObject.Find("Player").GetComponent<Controller>();
    }
    public void Selected()
    {
        if(selected == true)
        {
            Debug.Log("押されました");
        }
        if(!selected && controller.followingPikmins.Count >= 1)//クリックされていてなおかつ、現在追従されているピクミンの数が１よりも多ければ
        {
            controller.followingPikmins[0].pileUpPoint = this;
            controller.followingPikmins.RemoveAt(0);
            selected = true;
        }
    }

}
