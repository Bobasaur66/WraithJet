using Nautilus.Extensions;
using Nautilus.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;
using UWE;
using VehicleFramework;
using VehicleFramework.Engines;
using VehicleFramework.VehicleParts;
using VehicleFramework.VehicleTypes;
using WorldStreaming;
using static GameInput;
using AircraftLib;
using AircraftLib.Managers;
using AircraftLib.VehicleTypes;

namespace WraithJet
{
    public class Wraith : PlaneVehicle
    {
        public static GameObject model;

        public static Atlas.Sprite pingSprite;

        public static Atlas.Sprite crafterSprite;

        public static Vehicle playerVehicle;

        public static float playerVehicleSpeed;

        protected float _maxSpeed = 60f;
        public override float maxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
            }
        }

        protected float _takeoffSpeed = 35f;
        public override float takeoffSpeed
        {
            get
            {
                return _takeoffSpeed;
            }
            set
            {
                _takeoffSpeed = value;
            }
        }

        public override void Start()
        {
            base.Start();

            voice.voice = VehicleFramework.VoiceManager.GetVoice("WraithAI");
            voice.balance = 1.3f;
            voice.blockVoiceChange = true;
        }

        public override void Update()
        {
            base.Update();

            if (GetKeyDown(WraithJetPlugin.ModConfig.engineToggleKey))
            {
                WraithEngine.engineHigh = !WraithEngine.engineHigh;
            }

            FlightManager.CheckLandingGear(this);
        }

        public static void GetAssets()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(directory, "assets/wraith"));
            bool flag = assetBundle == null;
            if (flag)
            {
                Debug.Log("[Wraith]: Failure loading assetbundle");
            }
            else
            {
                object[] array = assetBundle.LoadAllAssets();
                object[] array2 = array;
                foreach (object obj in array2)
                {
                    bool flag2 = obj.ToString().Contains("SpriteAtlas");
                    if (flag2)
                    {
                        SpriteAtlas spriteAtlas = (SpriteAtlas)obj;
                        Sprite sprite = spriteAtlas.GetSprite("WraithPingSprite");
                        Wraith.pingSprite = new Atlas.Sprite(sprite, false);
                        Sprite sprite2 = spriteAtlas.GetSprite("WraithCrafterSprite");
                        Wraith.crafterSprite = new Atlas.Sprite(sprite2, false);
                    }
                    else
                    {
                        bool flag3 = obj.ToString().Contains("vehicle");
                        if (flag3)
                        {
                            Wraith.model = (GameObject)obj;
                        }
                    }
                }
            }
        }
        public static IEnumerator Register()
        {
            Wraith.GetAssets();
            ModVehicle wraith = Radical.EnsureComponent<Wraith>(Wraith.model);
            wraith.name = "Wraith";
            yield return CoroutineHost.StartCoroutine(VehicleRegistrar.RegisterVehicle(wraith));
            yield break;
        }

        public override Dictionary<TechType, int> Recipe
        {
            get
            {
                return new Dictionary<TechType, int>
                {
                    {
                        TechType.PlasteelIngot, 1
                    },
                    {
                        TechType.AdvancedWiringKit, 2
                    },
                    {
                        TechType.Lubricant, 1
                    },
                    {
                        TechType.EnameledGlass, 2
                    },
                    {
                        TechType.PowerCell, 2
                    },
                    {
                        TechType.AramidFibers, 2
                    },
                    {
                        TechType.Aerogel, 2
                    }
                };
            }
        }

        public override string vehicleDefaultName
        {
            get
            {
                Language main = Language.main;
                bool flag = !(main != null);
                string result;
                if (flag)
                {
                    result = "WRAITH";
                }
                else
                {
                    result = main.Get("WraithDefaultName");
                }
                return result;
            }
        }

        public override string Description
        {
            get
            {
                return "A single-seater aircraft capable of great speeds, but of great underwater agility as well.";
            }
        }

        public override string EncyclopediaEntry
		{
			get
			{
				string str = "The Alterra Wraith is a newly developed single-seater aircraft, that can also function as a submersible. ";
                str += "It is the first Alterra aircraft to ever be usable as a submersible. ";
                str += "It is also currently the most used single-seater general aviation aircraft developed by Alterra.\n";
                str += "\nIt features:\n";
                str += "- Two-speed flight controls which double as submersible controls\n";
                str += "- Variable airfoil to ensure stability in any fluid\n";
                str += "- Automatic landing gear functionality\n";
                str += "- VTOL functionality\n";
                str += "- Onboard AI\n";
                str += "\nAdvice for pilots:\n";
                str += "- Low thrust engine mode will enable agile movements when not flying\n";
                str += "- High thrust engine mode is optimized for flight, and will not allow you to 'strafe' sideways\n";
                str += "- Base package is not designed for deep sea use; upgrade modules are required for that\n";
                str += "\nRatings:\n";
                str += "- Top Speed Low Thrust: 13m/s\n";
                str += "- Top Speed High Thrust: 60/s\n";
                str += "- Max Acceleration: 6m/s/s\n";
                str += "- Power: Two replaceable power cells in each wing\n";
                str += "- Dimensions: 4m x 3m x 1.5m\n";
                str += "- Persons: 1\n";
                return str + "\n\'The Alterra Wraith: Faster than a bullet leviathan.\'\n";
			}
		}

        public override GameObject VehicleModel
        {
            get
            {
                return Wraith.model;
            }
        }

        public override GameObject StorageRootObject
        {
            get
            {
                return base.transform.Find("StorageRoot").gameObject;
            }
        }

        public override GameObject ModulesRootObject
        {
            get
            {
                return base.transform.Find("ModulesRoot").gameObject;
            }
        }

        public override VehiclePilotSeat PilotSeat
        {
            get
            {
                VehiclePilotSeat result = default(VehiclePilotSeat);
                Transform transform = base.transform.Find("Pilotseat");
                result.Seat = transform.gameObject;
                result.SitLocation = transform.Find("SitLocation").gameObject;
                result.LeftHandLocation = transform;
                result.RightHandLocation = transform;
                return result;
            }
        }

        public override List<VehicleHatchStruct> Hatches
        {
            get
            {
                List<VehicleHatchStruct> list = new List<VehicleHatchStruct>();
                VehicleHatchStruct hatch = default(VehicleHatchStruct);
                hatch.Hatch = transform.Find("Collider").gameObject;
                hatch.ExitLocation = transform.Find("Hatch/ExitPosition");
                hatch.SurfaceExitLocation = transform.Find("Hatch/ExitPosition");
                list.Add(hatch);
                return list;
            }
        }

        public override List<VehicleStorage> InnateStorages
        {
            get
            {
                return new List<VehicleStorage>();
            }
        }

        public override List<VehicleUpgrades> Upgrades
        {
            get
            {
                List<VehicleUpgrades> list = new List<VehicleUpgrades>();
                VehicleUpgrades vehicleUpgrades = default(VehicleUpgrades);
                vehicleUpgrades.Interface = base.transform.Find("Upgrades").gameObject;
                vehicleUpgrades.Flap = vehicleUpgrades.Interface;
                list.Add(vehicleUpgrades);
                return list;
            }
        }

        public override List<VehicleBattery> Batteries
        {
            get
            {
                List<VehicleBattery> list = new List<VehicleBattery>();
                VehicleBattery item = default(VehicleBattery);
                item.BatterySlot = base.transform.Find("Batteries/Battery1").gameObject;
                list.Add(item);
                VehicleBattery item2 = default(VehicleBattery);
                item2.BatterySlot = base.transform.Find("Batteries/Battery2").gameObject;
                list.Add(item2);
                return list;
            }
        }

        public override List<VehicleBattery> BackupBatteries
        {
            get
            {
                return null;
            }
        }

        public override List<VehicleFloodLight> HeadLights
        {
            get
            {
                List<VehicleFloodLight> list = new List<VehicleFloodLight>();
                List<VehicleFloodLight> list2 = list;
                VehicleFloodLight item = default(VehicleFloodLight);
                item.Light = base.transform.Find("lights_parent/headlights/L").gameObject;
                item.Angle = 70f;
                item.Color = Color.white;
                item.Intensity = 0.6f;
                item.Range = 400f;
                list2.Add(item);
                List<VehicleFloodLight> list3 = list;
                item = default(VehicleFloodLight);
                item.Light = base.transform.Find("lights_parent/headlights/R").gameObject;
                item.Angle = 70f;
                item.Color = Color.white;
                item.Intensity = 0.6f;
                item.Range = 400f;
                list3.Add(item);
                return list;
            }
        }

        public override List<GameObject> WaterClipProxies
        {
            get
            {
                List<GameObject> list = new List<GameObject>();
                foreach (object obj in base.transform.Find("WaterClipProxies"))
                {
                    Transform transform = (Transform)obj;
                    list.Add(transform.gameObject);
                }
                return list;
            }
        }

        public override List<GameObject> CanopyWindows
        {
            get
            {
                return new List<GameObject>
                {
                    base.transform.Find("Model/InnerCanopy").gameObject,
                    base.transform.Find("Model/OuterCanopy").gameObject
                };
            }
        }

        public override GameObject BoundingBox
        {
            get
            {
                return base.transform.Find("BoundingBox").gameObject;
            }
        }

        public override GameObject CollisionModel
        {
            get
            {
                return base.transform.Find("Collider").gameObject;
            }
        }

        public override ModVehicleEngine Engine
        {
            get
            {
                return Radical.EnsureComponent<WraithEngine>(base.gameObject);
            }
        }

        public override Atlas.Sprite PingSprite
        {
            get
            {
                return Wraith.pingSprite;
            }
        }

        public override int BaseCrushDepth
        {
            get
            {
                return 100;
            }
        }

        public override int CrushDepthUpgrade1
        {
            get
            {
                return 300;
            }
        }

        public override int CrushDepthUpgrade2
        {
            get
            {
                return 600;
            }
        }

        public override int CrushDepthUpgrade3
        {
            get
            {
                return 1000;
            }
        }

        public override int MaxHealth
        {
            get
            {
                return 400;
            }
        }

        public override int Mass
        {
            get
            {
                return 1000;
            }
        }

        public override int NumModules
        {
            get
            {
                return 6;
            }
        }

        public override bool HasArms
        {
            get
            {
                return false;
            }
        }

        public override Atlas.Sprite CraftingSprite
        {
            get
            {
                return Wraith.crafterSprite;
            }
        }

        public override List<VehicleStorage> ModularStorages
        {
            get
            {
                return null;
            }
        }

        public override float TimeToConstruct => 6f;

        public override bool CanLeviathanGrab => true;

        public override bool CanMoonpoolDock => true;

        
    }
}
