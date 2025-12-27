using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModMenu;
using UILib;
using CramponDebug.src;

namespace CramponDebug
{
    [BepInPlugin("com.github.itzFairBlade1.CramponDebug", "Crampon Debug", PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.github.Kaden5480.poy-ui-lib")]
    [BepInDependency("com.github.Kaden5480.poy-mod-menu")]
    public class Core : BaseUnityPlugin
    {
        // Cache for accessing objects in the current scene
        private Cache cache;

        // Tracks timings
        private Tracker tracker;

        // Displays timings
        private UI ui;

        // Mod Menu config
        private Cfg config;

        // Extra Console output
        private ConsoleOutput consoleOutput;

        public static bool createdUI = false;

        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        public void Awake()
        {
            // Initialize classes
            config = new Cfg(this.Config);
            cache = new Cache();
            tracker = new Tracker(cache);
            ui = new UI(tracker, this, cache, config);
            consoleOutput = new ConsoleOutput(this, tracker, cache, config);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            // Register Mod with Mod Menu
            ModInfo info = ModManager.Register(this);
            info.description = "A debug UI for displaying crampon and climbing tool cooldowns.";
            info.license = "GPL-3.0";
            info.thumbnailUrl = "https://raw.githubusercontent.com/itzFairBlade1/CramponDebug/main/images/10-Point_Crampons.png";
            info.Add(config);
        }

        public void Start()
        {
            // Creates the UI toggle shortcut
            Shortcut shortcut = new Shortcut(new[] { config.UIToggleBind });
            UIRoot.AddShortcut(shortcut);
            shortcut.onTrigger.AddListener(UI.Toggle);
        }

        public void Update()
        {
            if (config.ConsoleOutput.Value)
                consoleOutput.LogTimers();
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

            // Create UI elements
            if (cache.IsComplete() && !createdUI)
            {
                ui.CreateElements();
                createdUI = true;
            }

            // Make UI visible only if values can change
            if (!cache.IsComplete() && createdUI) ui.ToggleVisibility(false);
            else if (cache.IsComplete() && createdUI) ui.ToggleVisibility(true);
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

        public void Log(string message, bool isError = false)
        {
            if (!isError) Logger.LogInfo(message);
            else Logger.LogError(message);
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
