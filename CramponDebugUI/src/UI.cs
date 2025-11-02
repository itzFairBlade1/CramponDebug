using UnityEngine;

namespace CramponDebugUI
{
    public class UI
    {
        private Tracker tracker;
        private Core core;

        private float currentX;            // Current X position for sliding
        private float targetX = 10f;       // Final X position (on-screen)
        private float onScreenX = 10f;
        private float offScreenX = -260f;  
        private float animationSpeed = 5f;

        private float boxWidth = 250f;
        private float boxHeight = 170f;

        private bool isOnScreen;

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
        }

        public void Switch()
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
            float screenHeight = Screen.height;

            // Animate currentX toward targetX
            currentX = Mathf.Lerp(currentX, targetX, Time.deltaTime * animationSpeed);

            // Middle-left Y position
            float y = Mathf.Floor(screenHeight/2 - boxHeight/2);
            

            // Clamp timer values
            float armTimerL = Mathf.Max(0f, tracker.armTimerL);
            float armTimerR = Mathf.Max(0f, tracker.armTimerR);
            float iceAxeTimerL = Mathf.Max(0f, tracker.iceAxeTimerL);
            float iceAxeTimerR = Mathf.Max(0f, tracker.iceAxeTimerR);
            float cramponTimer = Mathf.Max(0f, tracker.cramponTimer);

            // GUI style
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
