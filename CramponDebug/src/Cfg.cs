
using BepInEx.Configuration;
using ModMenu.Config;
using UnityEngine;

namespace CramponDebug
{
    internal class Cfg
    {
        [Field("Toggle UI Keybind", "Toggles the Crampon Debug UI on or off.")]
        internal ConfigEntry<KeyCode> UIToggleBind;

        [Listener(typeof(UI), nameof(UI.ReCreateElements))]
        [Field(FieldType.Slider, min = 0f, max = 1f, description = "The alpha value of the black screen behind the text", name = "Background Opacity")]
        internal ConfigEntry<float> BackgroundOpacity;

        [Field("Default On Screen", "If the UI should be shown when entering the game for the first time.")]
        internal ConfigEntry<bool> DefaultOnScreen;

        [Listener(typeof(UI), nameof(UI.ReCreateElements))]
        [Field(name = "Animation Speed", description = "The speed of showing/hiding the UI with the UIToggleBind.", fieldType = FieldType.Slider, min = 0.5f, max = 20f)]
        internal ConfigEntry<float> animationSpeed;

        [Field("Console Output", "Extra information shown in the console.")]
        internal ConfigEntry<bool> ConsoleOutput;

        [Field("Wrong Timing Only", "disables the display of other console output messages")]
        internal ConfigEntry<bool> OnlyShowWrongTiming;

        // we are not making this mistake again
        internal Cfg(ConfigFile config)
        {
            UIToggleBind = config.Bind<KeyCode>(
                "Keybinds",
                "Toggle UI Keybind",
                KeyCode.M,
                "Toggles the Crampon Debug UI on or off."
            );

            ConsoleOutput = config.Bind<bool>(
                "Console Output",
                "Console Output",
                false,
                "Extra information shown in the console."
            );

            OnlyShowWrongTiming = config.Bind<bool>(
                "Console Output",
                "Wrong Timing Only",
                false,
                "disables the display of other console output messages"
            );

            BackgroundOpacity = config.Bind<float>(
                "UI Settings",
                "Background Opacity",
                0.6f,
                "The alpha value of the black screen behind the text"
            );

            DefaultOnScreen = config.Bind<bool>(
                "UI Settings",
                "Default On Screen",
                true,
                "If the UI should be shown when entering the game for the first time."
            );

            animationSpeed = config.Bind<float>(
                "UI Settings",
                "Animation Speed",
                4f,
                "The speed of showing/hiding the UI with the UIToggleBind."
            );
        }
    }
}
