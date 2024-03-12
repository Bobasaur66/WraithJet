using System;
using Nautilus.Options.Attributes;
using Nautilus.Json;
using UnityEngine;

namespace WraithJet
{
    [Menu("Wraith Configuration")]
    public class WraithConfig : ConfigFile
    {
        [Keybind("Key to toggle engine low/high")]
        public static KeyCode engineToggleKey = KeyCode.F;
    }
}
