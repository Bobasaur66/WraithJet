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

        public override void ControlRotation()
        {
            // if underwater
            if (FlightManager.checkUnderwaterActual(this.mv) == true)
            {
                mousePosition = new Vector2(0f, 0f);
                isControllingRC = false;
                canvasClone.SetActive(false);

                FlightManager.StabilizeRoll(this.mv, true);

                float pitchFactor = 5f;
                float yawFactor = 5f;
                Vector2 mouseDir = GameInput.GetLookDelta();
                float xRot = mouseDir.x;
                float yRot = mouseDir.y;
                rb.AddTorque(mv.transform.up * xRot * yawFactor * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddTorque(mv.transform.right * yRot * -pitchFactor * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                isControllingRC = true;
                canvasClone.SetActive(true);

                setRCCrosshairPosition();

                // input
                inputYawValue = 0f;
                if (GameInput.GetKey(AircraftLibPlugin.ModConfig.yawLeftBind))
                {
                    inputYawValue += 1f;
                }
                if (GameInput.GetKey(AircraftLibPlugin.ModConfig.yawRightBind))
                {
                    inputYawValue -= 1f;
                }

                // pitch
                this.rb.AddTorque(this.mv.transform.right * Time.deltaTime * pitchSensitivity * GetCurrentPercentOfVehicleTopSpeed() * mousePosition.y / 18, (ForceMode)2);

                // destablize roll
                AircraftLib.FlightManager.StabilizeRoll(this.mv, false);

                // do roll with mouse horizontalness
                this.rb.AddTorque(this.mv.transform.forward * Time.deltaTime * rollSensitivity * GetCurrentPercentOfVehicleTopSpeed() * mousePosition.x / 18, (ForceMode)2);

                // do yaw with bank angle
                rollAngle = this.mv.transform.localEulerAngles.z;
                rollAngle = FlightManager.GetNormalizedAngle(rollAngle);
                rollDirection = rollAngle / Mathf.Abs(rollAngle);

                if (Mathf.Abs(rollAngle) > 90)
                {
                    yawValue = Mathf.Lerp(1f, 0f,
                    Mathf.InverseLerp(90 * rollDirection, 180 * rollDirection, rollAngle)
                    );
                }
                else
                {
                    yawValue = Mathf.Lerp(0f, 1f,
                    Mathf.InverseLerp(0 * rollDirection, 90 * rollDirection, rollAngle)
                    );
                }

                if (inputYawValue != 0f)
                {
                    this.rb.AddTorque(Vector3.up * Time.deltaTime * yawSensitivity * inputYawValue * -3, (ForceMode)2);
                }
                else
                {
                    this.rb.AddTorque(Vector3.up * Time.deltaTime * yawSensitivity * yawValue * rollDirection * -3, (ForceMode)2);
                }
            }
        }
    }
}