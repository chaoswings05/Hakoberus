using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChangerScript : MonoBehaviour
{
    [SerializeField]
    Texture2D i_cursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(i_cursor, Vector2.zero, CursorMode.Auto);
    }
}
