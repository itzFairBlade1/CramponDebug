using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CramponDebugUI
{
    [BepInPlugin("com.github.itzFairBlade1.CramponDebugUI", "Crampon Debug UI", PluginInfo.PLUGIN_VERSION)]
    public class Core : BaseUnityPlugin
    {
        // Cache for accessing objects in the current scene
        private Cache cache;

        // Tracks timings
        private Tracker tracker;

        // Displays timings
        private UI ui;

        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        public void Awake()
        {
            // Create cache and tracker
            cache = new Cache();
            tracker = new Tracker(cache);
            ui = new UI(tracker, this);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /**
         * <summary>
         * Executes per Frame.
         * </summary>
         */
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Logger.LogInfo("pressed M");
                ui.Switch();
            }
        }

        /**
         * <summary>
         * Executes when the plugin is destroyed.
         * </summary>
         */
        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /**
         * <summary>
         * Executes when a scene is loaded.
         * </summary>
         * <param name="scene">The scene which loaded</param>
         * <param name="mode">The mode the scene loaded with</param>
         */
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Update the cache
            cache.FindObjects();
        }

        /**
         * <summary>
         * Executes when a scene is unloaded.
         * </summary>
         * <param name="scene">The scene which unloaded</param>
         */
        public void OnSceneUnloaded(Scene scene)
        {
            // Clear the cache
            cache.Clear();
        }

        /**
         * <summary>
         * Updates the UI
         * </summary>
         */
        public void OnGUI()
        {
            // Add null check before reading tracker values
            if (cache.IsComplete())
                ui.Render();
        }
    }
}
