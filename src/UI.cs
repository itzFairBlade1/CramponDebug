using UnityEngine;

namespace CramponDebug
{
    public class UI
    {
        private Tracker tracker;

        /**
         * <summary>
         * Initializes the UI.
         * </summary>
         * <param name="tracker">The tracker to get timers from</param>
         */
        public UI(Tracker tracker)
        {
            this.tracker = tracker;
        }

        /**
         * <summary>
         * Renders the UI to display timers.
         * </summary>
         */
        public void Render()
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 130), GUI.skin.box);
            GUILayout.Label($"armTimerL:    {tracker.armTimerL}");
            GUILayout.Label($"armTimerR:    {tracker.armTimerR}");
            GUILayout.Label($"iceAxeTimerL: {tracker.iceAxeTimerL}");
            GUILayout.Label($"iceAxeTimerR: {tracker.iceAxeTimerR}");
            GUILayout.Label($"cramponTimer: {tracker.cramponTimer}");
            GUILayout.EndArea();
        }
    }
}
