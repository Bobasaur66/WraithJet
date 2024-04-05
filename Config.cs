using System;
using Nautilus.Options.Attributes;
using Nautilus.Json;
using UnityEngine;

namespace WraithJet
{
    [Menu("Wraith Configuration")]
    public class WraithConfig : ConfigFile
    {
        [Keybind("Toggle Engine Mode")]
        public KeyCode engineToggleKey = KeyCode.F;
    }
}
