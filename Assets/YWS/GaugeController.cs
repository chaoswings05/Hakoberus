using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gauge = null;
    private const float maxCount = 3f;
    public Vector2 defaultSize = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        defaultSize = gauge.size;
        DrawGauge(0);
        this.gameObject.SetActive(false);
    }

    public void DrawGauge(float timeCount)
    {
        float ratio = timeCount / maxCount;
        gauge.size = new Vector2(defaultSize.x * ratio, defaultSize.y);
    }
}
