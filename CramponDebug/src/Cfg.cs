
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

        [Field("Console Output", "Extra information shown in the console.")]
        internal ConfigEntry<bool> ConsoleOutput;

        [Field("Wrong Timing Only", "disables the display of other console output messages")]
        internal ConfigEntry<bool> OnlyShowWrongTiming;

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
        }
    }
}
