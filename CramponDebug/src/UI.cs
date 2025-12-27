using UILib;
using UILib.Components;
using UILib.Layouts;
using UILib.Patches;
using UnityEngine;

namespace CramponDebug
{
    public class UI
    {
        private Tracker tracker;
        private Core core;
        private Cache cache;
        private Cfg config;
        private static UI instance;

        private const float BOX_WIDTH = 350f;
        private const float BOX_HEIGHT = 170f;
        private const float LABEL_WIDTH = 300f;
        private const float LABEL_HEIGHT = 30f;
        private const int FONT_SIZE = 33;
        private const int ELEMENT_SPACING = 7;

        private  float currentX;            // Current X position for sliding
        private  float currentOffset;      // Current offset X position for sliding
        private  float targetX = 0f;       // Target X position gets set to onScreenX or offScreenX
        private  float onScreenX = 0f;     // X position on screen
        private  float offScreenX = -360f;  // X position off screen
        private  float animationSpeed = 5f;

        // UI Elements
        private Overlay overlay;
        private Label armTimerLLabel;
        private Label armTimerRLabel;
        private Label iceAxeTimerLLabel;
        private Label iceAxeTimerRLabel;
        private Label cramponTimerLabel;

        private bool isOnScreen;

        /**
         * <summary>
         * Initializes the UI.
         * </summary>
         * <param name="tracker">The tracker to get timers from</param>
         */
        internal UI(Tracker tracker, Core core, Cache cache, Cfg config)
        {
            this.tracker = tracker;
            this.core = core;
            this.cache = cache;
            this.config = config;
            instance = this;
        }

        public void ToggleVisibility(bool visibility)
        {
            bool currentlyVisible = overlay.isVisible;

            if (!visibility && currentlyVisible) overlay.ToggleVisibility();
            else if (visibility && !currentlyVisible) overlay.ToggleVisibility();
        }

        /**
         * <summary>
         * Animates the UIs by saving toggle States.
         * </summary>
         * <param name="tracker">The tracker to get timers from</param>
         */
        public static void Toggle()
        {
            instance.ToggleInternal();
        }

        public void ToggleInternal()
        {
            if (!isOnScreen)
            {
                targetX = onScreenX;
                isOnScreen = true;
            }

            else
            {
                targetX = offScreenX;
                isOnScreen = false;
            }
        }

        public static void ReCreateElements(float value)
        {
            instance.ReCreateElementsInternal();
        }

        private void ReCreateElementsInternal()
        {
            // In game force create UI
            if (cache.IsComplete() && Core.createdUI)
            {
                overlay.Destroy();
                CreateElements();
            }

            // Out of game create UI upon scene load
            else if (!cache.IsComplete() && Core.createdUI)
            {
                overlay.Destroy();
                Core.createdUI = false;
            }
        }

        /**
         * <summary>
         * Creates the UI upon first load.
         * </summary>
         * <param name="tracker">The tracker to get timers from</param>
         */
        public void CreateElements()
        {
            Theme theme = Theme.GetTheme();
            Color bgColor = theme.background;
            bgColor.a = config.BackgroundOpacity.Value;
            Image background = new Image(bgColor);
            background.SetFill(FillType.All);
            background.SetContentLayout(LayoutType.Vertical);
            background.SetElementSpacing(ELEMENT_SPACING);

            overlay = new Overlay(BOX_WIDTH, BOX_HEIGHT);
            overlay.SetContentLayout(LayoutType.Vertical);

            currentOffset = (Screen.width / 2 - BOX_WIDTH / 2) * -1;
            overlay.SetOffset(currentOffset + currentX, 0f);
            overlay.SetElementSpacing(ELEMENT_SPACING);
            overlay.SetLockMode(LockMode.None);

            armTimerLLabel = new Label($"Left Arm:         0", FONT_SIZE);
            armTimerLLabel.SetAlignment(AnchorType.MiddleLeft);
            armTimerLLabel.SetSize(LABEL_WIDTH, LABEL_HEIGHT);
            background.Add(armTimerLLabel);

            armTimerRLabel = new Label($"Right Arm:       0", FONT_SIZE);
            armTimerRLabel.SetAlignment(AnchorType.MiddleLeft);
            armTimerRLabel.SetSize(LABEL_WIDTH, LABEL_HEIGHT);
            background.Add(armTimerRLabel);

            iceAxeTimerLLabel = new Label($"Left Ice Axe:    0", FONT_SIZE);
            iceAxeTimerLLabel.SetAlignment(AnchorType.MiddleLeft);
            iceAxeTimerLLabel.SetSize(LABEL_WIDTH, LABEL_HEIGHT);
            background.Add(iceAxeTimerLLabel);

            iceAxeTimerRLabel = new Label($"Right Ice Axe:  0", FONT_SIZE);
            iceAxeTimerRLabel.SetAlignment(AnchorType.MiddleLeft);
            iceAxeTimerRLabel.SetSize(LABEL_WIDTH, LABEL_HEIGHT);
            background.Add(iceAxeTimerRLabel);

            cramponTimerLabel = new Label($"Crampons:       0", FONT_SIZE);
            cramponTimerLabel.SetAlignment(AnchorType.MiddleLeft);
            cramponTimerLabel.SetSize(LABEL_WIDTH, LABEL_HEIGHT);
            background.Add(cramponTimerLabel);

            overlay.Add(background);

            overlay.ToggleVisibility(); // Start visible
        }

        /**
         * <summary>
         * Renders the UI to display timers.
         * </summary>
         */
        public void Render()
        {
            // Animate currentX toward targetX
            currentX = Mathf.Lerp(currentX, targetX, Time.deltaTime * animationSpeed);

            // Clamp timer values
            float armTimerL = Mathf.Max(0f, tracker.armTimerL);
            float armTimerR = Mathf.Max(0f, tracker.armTimerR);
            float iceAxeTimerL = Mathf.Max(0f, tracker.iceAxeTimerL);
            float iceAxeTimerR = Mathf.Max(0f, tracker.iceAxeTimerR);
            float cramponTimer = Mathf.Max(0f, tracker.cramponTimer);

            // Sets offsets and text for UILib elements
            overlay.SetOffset(currentOffset + currentX, 0f);

            armTimerLLabel.SetText($"Left Arm:     {armTimerL:F2}");
            armTimerLLabel.SetColor(armTimerL == 0 ? Color.green : Color.red);

            armTimerRLabel.SetText($"Right Arm:    {armTimerR:F2}");
            armTimerRLabel.SetColor(armTimerR == 0 ? Color.green : Color.red);

            iceAxeTimerLLabel.SetText($"Left Ice Axe:  {iceAxeTimerL:F2}");
            iceAxeTimerLLabel.SetColor(iceAxeTimerL == 0 ? Color.green : Color.red);

            iceAxeTimerRLabel.SetText($"Right Ice Axe: {iceAxeTimerR:F2}");
            iceAxeTimerRLabel.SetColor(iceAxeTimerR == 0 ? Color.green : Color.red);

            cramponTimerLabel.SetText($"Crampons:     {cramponTimer:F2}");
            cramponTimerLabel.SetColor(cramponTimer == 0 ? Color.green : Color.red);

        }
    }
}
