using UILib;
using UnityEngine;
using UILib.Layouts;
using UILib.Patches;
using UILib.Components;

namespace CramponDebug
{
    public class UI
    {
        private Tracker tracker;
        private Core core;

        private static float boxWidth = 250f;
        private static float boxHeight = 170f;

        private static float screenHeight = Screen.height;
        private static float y;

        private float currentX;            // Current X position for sliding
        private static float targetX = 10f;       // Target X position gets set to onScreenX or offScreenX
        private static float onScreenX = 10f;     // X position on screen
        private static float offScreenX = -260f;  // X position off screen
        private static float animationSpeed = 5f;

        private static bool isOnScreen;

        /**
         * <summary>
         * Initializes the UI.
         * </summary>
         * <param name="tracker">The tracker to get timers from</param>
         */
        public UI(Tracker tracker, Core core)
        {
            this.tracker = tracker;
            this.core = core;
            y = Mathf.Floor(screenHeight / 2 - boxHeight / 2);
        }

        /**
         * <summary>
         * Animates the UIs by saving toggle States.
         * </summary>
         * <param name="tracker">The tracker to get timers from</param>
         */
        public static void Toggle()
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

            
         
            // GUI stylek
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 22,
                normal = { textColor = Color.white }
            };

            // Draw box
            GUI.Box(new Rect(currentX, y, boxWidth, boxHeight), "Timers");

            // Labels with manual positioning inside the box
            labelStyle.normal.textColor = armTimerL == 0 ? Color.green : Color.red;
            GUI.Label(new Rect(currentX + 10f, y + 20f, 200f, 30f), $"Left Arm:         {armTimerL:F2}", labelStyle);

            labelStyle.normal.textColor = armTimerR == 0 ? Color.green : Color.red;
            GUI.Label(new Rect(currentX + 10f, y + 50f, 200f, 30f), $"Right Arm:       {armTimerR:F2}", labelStyle);

            labelStyle.normal.textColor = iceAxeTimerL == 0 ? Color.green : Color.red;
            GUI.Label(new Rect(currentX + 10f, y + 80f, 200f, 30f), $"Left Ice Axe:    {iceAxeTimerL:F2}", labelStyle);

            labelStyle.normal.textColor = iceAxeTimerR == 0 ? Color.green : Color.red;
            GUI.Label(new Rect(currentX + 10f, y + 110f, 200f, 30f), $"Right Ice Axe:  {iceAxeTimerR:F2}", labelStyle);

            labelStyle.normal.textColor = cramponTimer == 0 ? Color.green : Color.red;
            GUI.Label(new Rect(currentX + 10f, y + 140f, 200f, 30f), $"Crampons:       {cramponTimer:F2}", labelStyle);

            

        }
    }
}
