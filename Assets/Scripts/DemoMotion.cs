using UnityEngine;

namespace UiBlurDemo
{
    /// <summary>
    /// Simple deterministic motion used by the demo scenes.
    /// </summary>
    public class DemoMotion : MonoBehaviour
    {
        /// <summary>Pauses every DemoMotion, for still screenshots and frame comparisons.</summary>
        public static bool Enabled { get; set; } = true;

        public enum MotionMode
        {
            Rotate,
            OscillateWorld,
            OscillateUi
        }

        public MotionMode mode = MotionMode.Rotate;
        public Vector3 axis = Vector3.up;
        public float speed = 45.0f;
        public float amplitude = 1.0f;
        public float phase;

        private Vector3 startLocalPosition;
        private Vector2 startAnchoredPosition;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
            startLocalPosition = transform.localPosition;
            if (rectTransform != null)
            {
                startAnchoredPosition = rectTransform.anchoredPosition;
            }
        }

        private void Update()
        {
            if (!Enabled)
            {
                return;
            }

            float time = Time.time * speed + phase;

            switch (mode)
            {
                case MotionMode.Rotate:
                    transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
                    break;
                case MotionMode.OscillateWorld:
                    transform.localPosition = startLocalPosition + axis * (Mathf.Sin(time) * amplitude);
                    break;
                case MotionMode.OscillateUi:
                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = startAnchoredPosition + (Vector2)axis * (Mathf.Sin(time) * amplitude);
                    }

                    break;
            }
        }
    }
}
