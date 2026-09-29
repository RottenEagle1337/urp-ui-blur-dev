using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UiBlurDemo
{
    /// <summary>
    /// Showcase: Esc opens a pause menu over the HUD, Settings opens a popup over the menu.
    /// Opening fades the group alpha and animates _BlurStrength of a runtime material instance, so every panel
    /// of a group shares one material and one draw call batch.
    /// </summary>
    public class ShowcaseController : MonoBehaviour
    {
        private static readonly int BlurStrengthId = Shader.PropertyToID("_BlurStrength");

        [Header("Menu")]
        public CanvasGroup menuGroup;
        public List<Graphic> menuPanels = new List<Graphic>();
        [Range(0.0f, 1.0f)] public float menuStrength = 0.6f;

        [Header("Settings popup")]
        public CanvasGroup popupGroup;
        public List<Graphic> popupPanels = new List<Graphic>();
        [Range(0.0f, 1.0f)] public float popupStrength = 1.0f;

        [Header("Controls")]
        public Button resumeButton;
        public Button settingsButton;
        public Button quitButton;
        public Button closeButton;
        public Slider strengthSlider;
        public TMP_Text strengthLabel;

        [Min(0.01f)] public float fadeDuration = 0.35f;

        private Material menuMaterial;
        private Material popupMaterial;
        private float menuVisibility;
        private float popupVisibility;
        private bool menuOpen = true;
        private bool popupOpen;
        private float labelStrength = -1.0f;

        private void Awake()
        {
            menuMaterial = CreateInstance(menuPanels, "Menu Blur (runtime)");
            popupMaterial = CreateInstance(popupPanels, "Popup Blur (runtime)");

            resumeButton.onClick.AddListener(() => SetMenuOpen(false));
            settingsButton.onClick.AddListener(() => SetPopupOpen(true));
            quitButton.onClick.AddListener(Quit);
            closeButton.onClick.AddListener(() => SetPopupOpen(false));

            strengthSlider.SetValueWithoutNotify(menuStrength);
            strengthSlider.onValueChanged.AddListener(SetMenuStrength);

            menuVisibility = menuOpen ? 1.0f : 0.0f;
            popupVisibility = 0.0f;
            Apply();
        }

        private void OnDestroy()
        {
            Destroy(menuMaterial);
            Destroy(popupMaterial);
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                if (popupOpen)
                {
                    popupOpen = false;
                }
                else
                {
                    SetMenuOpen(!menuOpen);
                }
            }

            float step = Time.deltaTime / fadeDuration;
            menuVisibility = Mathf.MoveTowards(menuVisibility, menuOpen ? 1.0f : 0.0f, step);
            popupVisibility = Mathf.MoveTowards(popupVisibility, popupOpen && menuOpen ? 1.0f : 0.0f, step);
            Apply();
        }

        public void SetPopupOpen(bool open)
        {
            popupOpen = open;
        }

        public void SetMenuStrength(float value)
        {
            menuStrength = Mathf.Clamp01(value);
            strengthSlider.SetValueWithoutNotify(menuStrength);
        }

        public void SetMenuOpen(bool open)
        {
            menuOpen = open;
            if (!open)
            {
                popupOpen = false;
            }
        }

        private void Apply()
        {
            float menu = Smooth(menuVisibility);
            float popup = Smooth(popupVisibility);

            SetGroup(menuGroup, menu);
            SetGroup(popupGroup, popup);

            // Blur fades in together with the panels: strength 0 is an almost sharp background.
            if (menuMaterial != null)
            {
                menuMaterial.SetFloat(BlurStrengthId, menuStrength * menu);
            }

            if (popupMaterial != null)
            {
                popupMaterial.SetFloat(BlurStrengthId, popupStrength * popup);
            }

            if (!Mathf.Approximately(labelStrength, menuStrength))
            {
                labelStrength = menuStrength;
                strengthLabel.SetText("Menu blur strength  {0:2}", menuStrength);
            }
        }

        private static void SetGroup(CanvasGroup group, float visibility)
        {
            group.alpha = visibility;
            bool interactive = visibility > 0.99f;
            group.interactable = interactive;
            group.blocksRaycasts = interactive;
        }

        private static float Smooth(float value)
        {
            return value * value * (3.0f - 2.0f * value);
        }

        private static Material CreateInstance(List<Graphic> panels, string name)
        {
            if (panels.Count == 0 || panels[0] == null)
            {
                return null;
            }

            var instance = new Material(panels[0].material) { name = name };
            foreach (Graphic panel in panels)
            {
                panel.material = instance;
            }

            return instance;
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
