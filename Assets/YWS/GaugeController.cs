using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private GameObject player = null;
    [SerializeField] private SpriteRenderer frame = null;
    [SerializeField] private SpriteRenderer gauge = null;
    private const float maxCount = 3f;
    public Vector2 defaultSize = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        defaultSize = gauge.size;
        DrawGauge(0);
        HideGauge();
    }

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 0.6f, 0);
    }

    public void DrawGauge(float timeCount)
    {
        float ratio = timeCount / maxCount;
        gauge.size = new Vector2(defaultSize.x * ratio, defaultSize.y);
    }

    public void ShowGauge()
    {
        frame.color = new Color(255,255,255,1);
        gauge.color = new Color(255,255,255,1);
    }

    public void HideGauge()
    {
        frame.color = new Color(255,255,255,0);
        frame.color = new Color(255,255,255,0);
    }
}
