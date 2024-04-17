using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Net.Http.Headers;
using System.Reflection;
using UWE;
using Nautilus.Handlers;
using System.IO;
using System.Collections;

namespace WraithJet
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    [BepInDependency("com.Bobasaur.AircraftLib")]
    [BepInDependency("com.mikjaw.subnautica.vehicleframework.mod")]
    [BepInDependency("com.snmodding.nautilus")]
    public class WraithJetPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.Bobasaur.WraithJet";
        private const string PluginName = "Wraith Jet";
        private const string VersionString = "2.4.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        public static WraithConfig ModConfig { get; } = OptionsPanelHandler.RegisterModOptions<WraithConfig>();

        string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private void Awake()
        {
            Logger.LogInfo($"Will load {PluginName} version {VersionString}.");
            Harmony.PatchAll();
            Log = Logger;

            UWE.CoroutineHost.StartCoroutine(RegisterThenFragments());
        }

        private IEnumerator RegisterThenFragments()
        {
            yield return UWE.CoroutineHost.StartCoroutine(Wraith.Register());
            UWE.CoroutineHost.StartCoroutine(VehicleFramework.VoiceManager.RegisterVoice("WraithAI", modFolder));
            WraithFragments.RegisterFragments();
        }
    }
}
