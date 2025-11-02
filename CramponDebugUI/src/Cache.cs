using UnityEngine;

namespace CramponDebugUI
{
    public class Cache
    {
        public Climbing climbing     { get; private set; }
        public IceAxe iceAxes        { get; private set; }
        public StemFoot stemFoot     { get; private set; }

        /**
         * <summary>
         * Finds objects in the current scene.
         * </summary>
         */
        public void FindObjects()
        {
            climbing = GameObject.FindObjectOfType<Climbing>();
            iceAxes = GameObject.FindObjectOfType<IceAxe>();
            stemFoot = GameObject.FindObjectOfType<StemFoot>();
        }

        /**
         * <summary>
         * Checks whether all necessary objects exist.
         * </summary>
         * <returns>True if they do, false otherwise</returns>
         */
        public bool IsComplete()
        {
            return climbing != null
                && iceAxes != null
                && stemFoot != null;
        }

        /**
         * <summary>
         * Clears objects from the cache.
         * </summary>
         */
        public void Clear()
        {
            climbing = null;
            iceAxes = null;
            stemFoot = null;
        }
    }
}
