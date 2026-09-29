using TMPro;
using UnityEngine;

namespace UiBlurDemo
{
    /// <summary>
    /// Minimal frame rate label, updated twice a second without allocations.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class FpsCounter : MonoBehaviour
    {
        private const float Interval = 0.5f;

        private TMP_Text label;
        private float elapsed;
        private int frames;

        private void Awake()
        {
            label = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            frames++;

            if (elapsed < Interval)
            {
                return;
            }

            float fps = frames / elapsed;
            label.SetText("{0:0} FPS  {1:1} ms", fps, 1000.0f / fps);
            elapsed = 0.0f;
            frames = 0;
        }
    }
}
