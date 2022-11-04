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
    [SerializeField] private AnimationCurve Curve_R = new AnimationCurve(new Keyframe(0f, 179/255f), new Keyframe(120f, 1f), new Keyframe(240f, 0f));
    [SerializeField] private AnimationCurve Curve_G = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(120f, 169/255f), new Keyframe(240f, 17/255f));
    [SerializeField] private AnimationCurve Curve_B = new AnimationCurve(new Keyframe(0f, 254/255f), new Keyframe(120f, 67/255f), new Keyframe(240f, 142/255f));
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

            if (phase != 1 && elapsedTime >= 120f && elapsedTime <= 240f)
            {
                phase = 1;
                //RenderSettings.skybox.SetColor("_SkyTint", new Color (253/255f, 126/255f, 0f));
                Debug.Log("Phase 0 to Phase 1");
            }
            else if (phase != 2 && elapsedTime >= 240f)
            {
                phase = 2;
                //RenderSettings.skybox.SetColor("_SkyTint", new Color(46/255f, 75/255f, 113/255f));
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
