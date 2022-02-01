using UnityEngine;
using UnityEditor;

using System.Text;

namespace ExactFramework
{
    ///<summary>
    ///Digital representation of a device wuth a ring light, time of flight distance sensor, tone player, and an inertial measurement unit.
    ///</summary>
    public class ExactTileBase : TwinObject
    {
        protected LedRing ledRing;
        protected TonePlayer tonePlayer;
        protected IMU imu;
        protected RFIDReader rfidReader;

        public Color ringInUnityColor = Color.black;
        public float ringIntensity = 1.0f;
        SpriteRenderer ledRingSprite = null;


        public ExactGameLogicBase exactGameLogic = null;

        struct fadeDataType
        {
            public bool fading;
            public float intensity;
            public float fromIntensity;
            public float toIntensity;
            public float increment;
            public float beta;   // Helper variable for calculating intensity.
            public long startMillis;
            public long pulseLengthMs;
            public Color fadeColor;
        };

        fadeDataType fadeData;

        // Start is called before the first frame update
        protected override void TwinStart()
        {
            Debug.Log("Twin start");

            configName = "exactredtile";
            useDeviceName = true;
            useConfigName = true;

            ledRing = AddDeviceComponent<LedRing>("ledring");
            tonePlayer = AddDeviceComponent<TonePlayer>("toneplayer");
            imu = AddDeviceComponent<IMU>("imu");
            AddEventListener("imu.tapped", OnTapped);

            rfidReader = AddDeviceComponent<RFIDReader>("rfid_reader");
            AddEventListener("rfid_reader.rfid_enter", RFIDEnterCallback);
            AddEventListener("rfid_reader.rfid_leave", RFIDLeaveCallback);

            foreach (Transform child in transform)
            {
                if (child.name == "LEDRing")
                {
                    ledRingSprite = child.GetComponent<SpriteRenderer>();
                }
            }
            if (ledRing != null)
            {
                ringInUnityColor = ledRingSprite.color;
                ringIntensity = ringInUnityColor.a;
            }

            ExactGameLogicBase[] gameObjects = GameObject.FindObjectsOfType<ExactGameLogicBase>();
            foreach (ExactGameLogicBase to in gameObjects)
            {
                exactGameLogic = to;  // Allow for more game logics later;
            }


            DeviceStart();
        }

        public virtual void DeviceStart()
        {

        }

        // Update is called once per frame
        protected override void TwinUpdate()
        {
            if (fadeData.fading)
            {
                long now = (long)(Time.time * 1000.0f);         // now: timestamp
                long elapsed = now - fadeData.startMillis;
                fadeData.intensity = fadeData.fromIntensity + (((float)elapsed) * fadeData.beta);
                if (fadeData.increment > 0.0f)
                {
                    if (fadeData.intensity > fadeData.toIntensity)
                    {
                        SetLedRingSpriteIntensity(fadeData.toIntensity);
                        fadeData.fading = false;
                    }
                    else
                    {
                        SetLedRingSpriteIntensity(fadeData.intensity);
                    }
                }
                else
                {
                    if (fadeData.intensity < fadeData.toIntensity)
                    {
                        SetLedRingSpriteIntensity(fadeData.toIntensity);
                        fadeData.fading = false;
                    }
                    else
                    {
                        SetLedRingSpriteIntensity(fadeData.intensity);
                    }
                }
            }

            DeviceUpdate();
        }

        public virtual void DeviceUpdate()
        {

        }

        public override void DeviceIsConnected()
        {
            //    Debug.Log("connected now");
            ledRing.SetColor(ringInUnityColor);
            DeviceJustConnected();
        }

        public virtual void DeviceJustConnected()
        {

        }

        public virtual void OnTapped()
        {
        }


        public void OnMouseDown()
        {
            OnTapped();
        }

        public virtual void EventFromGameLogic(string eventName, string eventData)
        {

        }

        public virtual void EventFromGameLogic(string eventName, int eventData)
        {

        }

        public virtual void EventFromGameLogic(string eventName, Color c)
        {

        }

        public void RFIDEnterCallback()
        {
            byte[] lastId = rfidReader.GetLastReadID();
            string rfidString = Encoding.UTF8.GetString(lastId, 0, lastId.Length);
            OnRFIDEnter(rfidString);
        }

        public void RFIDLeaveCallback()
        {
            byte[] lastId = rfidReader.GetLastReadID();
            string rfidString = Encoding.UTF8.GetString(lastId, 0, lastId.Length);
            OnRFIDLeave(rfidString);
        }

        // Callable from subclass:
        // -------------------------------
        public void SendEventToGameLogic(string eventName, string eventData)
        {
            if (exactGameLogic != null)
            {
                exactGameLogic.EventFromDevice(this, eventName, eventData);
            }
        }

        public void SendEventToGameLogic(string eventName, int eventData)
        {
            if (exactGameLogic != null)
            {
                exactGameLogic.EventFromDevice(this, eventName, eventData);
            }
        }

        public void SendEventToGameLogic(string eventName, Color eventData)
        {
            if (exactGameLogic != null)
            {
                exactGameLogic.EventFromDevice(this, eventName, eventData);
            }
        }

        public void SetColor(Color c)
        {
            ringInUnityColor.r = c.r;
            ringInUnityColor.g = c.g;
            ringInUnityColor.b = c.b;
            if (ledRing != null)
            {
                ledRing.SetColor(c);
            }
            if (ledRingSprite != null)
            {
                ledRingSprite.color = ringInUnityColor;
            }
        }

        public Color GetColor()
        {
            return ringInUnityColor;
        }

        public void SetIndividualColor(Color color, int i)
        {
            if (ledRing != null)
            {
                ledRing.SetIndividualColor(color, i);
            }
        }

        public void SetRingColor(Color c)
        {
            //    Debug.Log("Set Ring color");
            //    Debug.Log(c.ToString());
            ringInUnityColor.r = c.r;
            ringInUnityColor.g = c.g;
            ringInUnityColor.b = c.b;
            if (ledRing != null)
            {
                ledRing.SetColor(c);
            }
            if (ledRingSprite != null)
            {
                //      Debug.Log("Set sprite color");
                //      Debug.Log(ringInUnityColor.ToString());
                ledRingSprite.color = ringInUnityColor;
            }
        }
        public void SetIntensity(int i)
        {
            if (ledRing != null)
            {
                ledRing.SetIntensity(i);
            }
            SetLedRingSpriteIntensity(((float)i) / 100.0f);
        }

        public int GetIntensity()
        {
            return (int)(ringIntensity * 100.0f);
        }

        private void SetLedRingSpriteIntensity(float intensity)
        {
            if (ledRingSprite != null)
            {
                ringInUnityColor.a = intensity;
                ledRingSprite.color = ringInUnityColor;
            }
            ringIntensity = intensity;
        }

        public void StartFading(Color fadeColor, int from, int to, long pulseLengthMs)
        {
            Debug.Log("Tilebase fade");
            Debug.Log(fadeColor.ToString());
            if (ledRing != null)
            {
                ledRing.StartFading(fadeColor, from, to, pulseLengthMs);
            }
            fadeData.fadeColor = fadeColor;
            float fromIntensity = ((float)from) / 100.0f;
            float toIntensity = ((float)to) / 100.0f;
            fadeData.pulseLengthMs = pulseLengthMs;
            fadeData.fromIntensity = fromIntensity; // 0.0 .. 1.0
            fadeData.toIntensity = toIntensity;  // 0.0 .. 1.0
            fadeData.intensity = fromIntensity;
            fadeData.increment = (toIntensity - fromIntensity);
            fadeData.beta = (fadeData.toIntensity - fadeData.fromIntensity) / ((float)fadeData.pulseLengthMs);
            ringInUnityColor.r = fadeColor.r;
            ringInUnityColor.g = fadeColor.g;
            ringInUnityColor.b = fadeColor.b;
            SetLedRingSpriteIntensity(fadeData.fromIntensity);
            fadeData.fading = true;
            fadeData.startMillis = (long)(Time.time * 1000.0f);
        }


        public void StartPulse(Color fadeColor, int from, int to, long pulseLengthMs)
        {

            ledRing.StartPulse(fadeColor, from, to, pulseLengthMs);

        }

        public void StopPulse()
        {

            ledRing.StopPulse();

        }

        public void PlayTone(int frequency, int duration)
        {
            EditorApplication.Beep();  // As for now..
            tonePlayer.PlayTone(frequency, duration);
        }

        public void EnableRFID()
        {
            rfidReader.EnableRFID();
        }

        public void DisableRFID()
        {
            rfidReader.DisableRFID();
        }


        public void SetSensitivity(int sensitivity)
        {
            imu.SetSensitivity(sensitivity);
        }

        // Can be redefined in subclass:
        // ------------------------------
        public virtual void OnRFIDEnter(string RFIDInHex)
        {
            Debug.Log("Enter:" + RFIDInHex);
        }

        public virtual void OnRFIDLeave(string RFIDInHex)
        {
            Debug.Log("Leave:" + RFIDInHex);
        }

    }
}
