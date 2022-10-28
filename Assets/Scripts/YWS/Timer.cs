using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private int phase = 0;
    private float elapsedTime = 0.0f;
    private Color initialSkyTint = new Color(128/255f, 128/255f, 128/255f);
    [Header("制限時間")] private float timeLimit = 300f;
    [SerializeField] private AnimationCurve Curve_R, Curve_G, Curve_B;
    [SerializeField] private Text display = null;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        phase = 0;
        elapsedTime = 0.0f;
        RenderSettings.skybox.SetColor("_SkyTint", initialSkyTint);
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < timeLimit)
        {
            elapsedTime += Time.deltaTime;

            if (phase != 1 && elapsedTime >= 60f && elapsedTime <= 180f)
            {
                phase = 1;
                RenderSettings.skybox.SetColor("_SkyTint", new Color (253/255f, 126/255f, 0f));
                Debug.Log("Phase 0 to Phase 1");
            }
            else if (phase != 2 && elapsedTime >= 180f)
            {
                phase = 2;
                RenderSettings.skybox.SetColor("_SkyTint", new Color(0f, 103/255f, 192/255f));
                Debug.Log("Phase 1 to Phase 2");
            }

            Color col = new Color(Curve_R.Evaluate(elapsedTime), Curve_G.Evaluate(elapsedTime), Curve_B.Evaluate(elapsedTime));
            RenderSettings.skybox.SetColor("_SkyTint", col);
        }
        else if (elapsedTime >= timeLimit)
        {
            Debug.Log("Game End");
        }

        display.text = elapsedTime.ToString();
    }
}
