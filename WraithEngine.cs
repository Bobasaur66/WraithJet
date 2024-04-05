using AircraftLib;
using System;
using UnityEngine;

namespace VehicleFramework.Engines
{
    public class WraithEngine : RCPlaneEngine
    {
        public static bool engineHigh = false;

        protected override float FORWARD_TOP_SPEED
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 1500f;
                }
                else
                {
                    return 4000f;
                }
            }
        }

        protected override float REVERSE_TOP_SPEED
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 1000f;
                }
                else
                {
                    return 300f;
                }
            }
        }

        protected override float STRAFE_MAX_SPEED
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 1000f;
                }
                else
                {
                    return 0f;
                }
            }
        }

        protected override float VERT_MAX_SPEED
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 750f;
                }
                else
                {
                    return 1000f;
                }
            }
        }

        protected override float FORWARD_ACCEL
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 4000f;
                }
                else
                {
                    return 400f;
                }
            }
        }

        protected override float REVERSE_ACCEL
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 4000f;
                }
                else
                {
                    return 300f;
                }
            }
        }

        protected override float STRAFE_ACCEL
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 4000f;
                }
                else
                {
                    return 0f;
                }
            }
        }

        protected override float VERT_ACCEL
        {
            get
            {
                bool flag = engineHigh == false;
                if (flag)
                {
                    return 4000f;
                }
                else
                {
                    return 2000f;
                }
            }
        }

        protected override float waterDragDecay => 4f;

        protected override float DragDecay => waterDragDecay;
    }
}