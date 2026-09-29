using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UiBlurDemo
{
    /// <summary>
    /// Keyboard driven test harness for the UI blur feature.
    /// </summary>
    public class BlurTestController : MonoBehaviour
    {
        private const float StrengthStep = 0.1f;
        private static readonly int BlurStrengthId = Shader.PropertyToID("_BlurStrength");

        public CanvasGroup menuGroup;
        public GameObject hudRoot;
        public TMP_Text hudText;

        [Tooltip("Graphics whose material strength is driven by the keyboard (a runtime copy of their material).")]
        public List<Graphic> animatedPanels = new List<Graphic>();

        public RectTransform stressParent;
        public GameObject stressTemplate;
        public int stressBatchSize = 10;

        private readonly List<GameObject> stressPanels = new List<GameObject>();

        private Material animatedMaterial;
        private float strength = 0.6f;
        private bool animateStrength;
        private bool hudDirty = true;

        private void OnEnable()
        {
            DemoMotion.Enabled = true;

            if (animatedPanels.Count > 0 && animatedPanels[0] != null && animatedPanels[0].material != null)
            {
                animatedMaterial = new Material(animatedPanels[0].material) { name = "Blur (runtime)" };
                strength = animatedMaterial.GetFloat(BlurStrengthId);
                foreach (Graphic panel in animatedPanels)
                {
                    panel.material = animatedMaterial;
                }
            }
        }

        private void OnDestroy()
        {
            if (animatedMaterial != null)
            {
                Destroy(animatedMaterial);
            }
        }

        private void Update()
        {
            HandleInput();

            if (animateStrength)
            {
                SetStrength(Mathf.PingPong(Time.time * 0.5f, 1.0f));
            }

            if (hudDirty && hudText != null)
            {
                hudDirty = false;
                hudText.SetText(
                    "UI Blur test\n" +
                    "[1] menu on-off   [Q]/[A] animated panel strength {0:2}   [Space] animate strength\n" +
                    "[M] motion   [P] +{1} stress panels ({2})   [O] clear   [H] hud",
                    strength, stressBatchSize, stressPanels.Count);
            }
        }

        private void HandleInput()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.digit1Key.wasPressedThisFrame && menuGroup != null)
            {
                menuGroup.alpha = menuGroup.alpha > 0.0f ? 0.0f : 1.0f;
            }

            if (keyboard.qKey.wasPressedThisFrame) StepStrength(StrengthStep);
            if (keyboard.aKey.wasPressedThisFrame) StepStrength(-StrengthStep);

            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                animateStrength = !animateStrength;
            }

            if (keyboard.mKey.wasPressedThisFrame)
            {
                DemoMotion.Enabled = !DemoMotion.Enabled;
            }

            if (keyboard.pKey.wasPressedThisFrame)
            {
                SpawnStressPanels();
            }

            if (keyboard.oKey.wasPressedThisFrame)
            {
                ClearStressPanels();
            }

            if (keyboard.hKey.wasPressedThisFrame && hudRoot != null)
            {
                hudRoot.SetActive(!hudRoot.activeSelf);
            }
        }

        private void StepStrength(float delta)
        {
            animateStrength = false;
            SetStrength(Mathf.Round((strength + delta) * 10.0f) / 10.0f);
        }

        public void SetStrength(float value)
        {
            strength = Mathf.Clamp01(value);
            if (animatedMaterial != null)
            {
                animatedMaterial.SetFloat(BlurStrengthId, strength);
            }

            hudDirty = true;
        }

        public void SpawnStressPanels()
        {
            if (stressTemplate == null || stressParent == null)
            {
                return;
            }

            Rect area = stressParent.rect;
            for (int index = 0; index < stressBatchSize; index++)
            {
                GameObject panel = Instantiate(stressTemplate, stressParent);
                panel.SetActive(true);
                var rectTransform = (RectTransform)panel.transform;
                rectTransform.anchoredPosition = new Vector2(
                    Random.Range(area.xMin, area.xMax) * 0.8f,
                    Random.Range(area.yMin, area.yMax) * 0.8f);
                rectTransform.sizeDelta = new Vector2(Random.Range(80.0f, 260.0f), Random.Range(80.0f, 260.0f));
                stressPanels.Add(panel);
            }

            hudDirty = true;
        }

        public void ClearStressPanels()
        {
            for (int index = 0; index < stressPanels.Count; index++)
            {
                Destroy(stressPanels[index]);
            }

            stressPanels.Clear();
            hudDirty = true;
        }
    }
}
