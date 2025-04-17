using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
[UnityEngine.ExecuteInEditMode]
public partial class ShowFps : MonoBehaviour
{
    public Font fpsFont;
    private Text text;
    private float updateInterval;
    private double lastInterval; // Last interval end time
    private int frames; // Frames over current interval
    public virtual void Start()
    {
        this.lastInterval = Time.realtimeSinceStartup;
        this.frames = 0;
    }

    public virtual void OnDisable()
    {
        if (this.text)
        {
            DestroyImmediate(this.text.gameObject);
        }
    }

    public virtual void Update()
    {
        ++this.frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > (this.lastInterval + this.updateInterval))
        {
            if (!this.text)
            {
                Canvas fpsCanvas = new GameObject("FPS Canvas", new Type[] { typeof(Canvas) }).GetComponent<Canvas>();
                fpsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                this.text = new GameObject("FPS Display", new System.Type[] {typeof(Text)}).GetComponent<Text>();
                this.text.transform.SetParent(fpsCanvas.transform);
                this.text.hideFlags = HideFlags.DontSave;
                this.text.color = Color.white;
                this.text.horizontalOverflow = HorizontalWrapMode.Overflow;
                this.text.verticalOverflow = VerticalWrapMode.Overflow;
                this.text.font = fpsFont;
                (this.text.transform as RectTransform).pivot = new Vector2(0, 0);
                (this.text.transform as RectTransform).anchorMin = new Vector2(0, 0);
                (this.text.transform as RectTransform).anchorMax = new Vector2(0, 0);
                (this.text.transform as RectTransform).anchoredPosition = new Vector2(5, 55);
            }
            float fps = (float) (this.frames / (timeNow - this.lastInterval));
            float ms = 1000f / Mathf.Max(fps, 1E-05f);
            this.text.text = ((ms.ToString("f1") + "ms ") + fps.ToString("f2")) + "FPS";
            this.frames = 0;
            this.lastInterval = timeNow;
        }
    }

    public ShowFps()
    {
        this.updateInterval = 1f;
    }

}