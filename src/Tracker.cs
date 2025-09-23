using System.Reflection;

using HarmonyLib;

namespace CramponDebug
{
    public class Tracker
    {
        // The object cache
        private Cache cache;

        // Important fields
        private FieldInfo infoArmForceTimerL = AccessTools.Field(typeof(Climbing), "armForceTimerL");
        private FieldInfo infoArmForceTimerR = AccessTools.Field(typeof(Climbing), "armForceTimerR");
        private FieldInfo infoForceCooldownL = AccessTools.Field(typeof(IceAxe), "forceCooldownL");
        private FieldInfo infoForceCooldownR = AccessTools.Field(typeof(IceAxe), "forceCooldownR");
        private FieldInfo infoWallkickCooldown = AccessTools.Field(typeof(StemFoot), "wallkickCooldown");

        // Nicer ways to access fields
        public float armTimerL    { get => (float) infoArmForceTimerL.GetValue(cache.climbing); }
        public float armTimerR    { get => (float) infoArmForceTimerR.GetValue(cache.climbing); }
        public float iceAxeTimerL { get => (float) infoForceCooldownL.GetValue(cache.iceAxes); }
        public float iceAxeTimerR { get => (float) infoForceCooldownR.GetValue(cache.iceAxes); }
        public float cramponTimer { get => (float) infoWallkickCooldown.GetValue(cache.stemFoot); }

        /**
         * <summary>
         * Initializes a Tracker.
         * </summary>
         * <param name="cache">The cache to read objects from</param>
         */
        public Tracker(Cache cache)
        {
            this.cache = cache;
        }
    }
}
