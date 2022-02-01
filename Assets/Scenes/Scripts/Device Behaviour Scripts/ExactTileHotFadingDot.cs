using UnityEngine;
using UnityEditor;

namespace ExactFramework
{
    ///<summary>
    ///Digital representation of a device
    ///</summary>
    ///

    // Available class methods:
    //   public void SetColor(Color c)
    //   public List<TwinObject> GetTwinObjectList()
    //   public void SendEventToGameLogic(string eventName, string eventData)
    //
    //
    public class ExactTileHotFadingDot : ExactTileBase
    {
        public int imuSensitivity = 10;    // mg/ms  1..1600;  1 is most sensitive. default=30
        public bool tileIsActive = false;
        public bool RFID_Enabled = false;

        // Start is called before the first frame update
        public override void DeviceStart()
        {
            SetColor(Color.black);
            SetSensitivity(imuSensitivity);  // mg/ms  1..1600;  1 is most sensitive. default=30.
            tileIsActive = false;
        }

        public override void DeviceUpdate()
        {

        }

        // public override void DeviceJustConnected()
        // {
        //
        // }

        public override void DeviceJustDisconnected()
        {
            if (tileIsActive)
            {
                if (RFID_Enabled)
                {
                    SendEventToGameLogic("RFID_Enter", "Disconnected");
                }
                else
                {
                    SendEventToGameLogic("Tapped", "Disconnected");  // Fake a tapped event..
                }

            }
        }


        public override void EventFromGameLogic(string eventName, int eventData)
        {
            if (eventName == "MakeActive")
            {
                tileIsActive = true;
                int intensity = GetIntensity();
                // this.PlayTone(500, 50);
                //EditorApplication.Beep();
                intensity = 5;
                StartFading(GetColor(), intensity, 60, eventData);

            }
            else if (eventName == "MakePassive")
            {
                int intensity = GetIntensity();
                this.PlayTone(500, 50);
                EditorApplication.Beep();
                StartFading(GetColor(), 80, 0, eventData);
                //  StartFading(Color.green, intensity, 0, 3000);
                tileIsActive = false;
            }
            else if (eventName == "Enable_RFID")
            {
                EnableRFID();
                RFID_Enabled = true;
            }
            else if (eventName == "Disable_RFID")
            {
                DisableRFID();
                RFID_Enabled = true;
            }
        }

        public override void OnTapped()
        {
            if (tileIsActive)
            {
                SendEventToGameLogic("Tapped", "");
            }
        }

        public override void OnRFIDEnter(string RFIDInHex)
        {
            if (tileIsActive)
            {
                SendEventToGameLogic("RFID_Enter", RFIDInHex);
            }
        }
    }
}