using UnityEngine;

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
    public class ExactTileRedDot : ExactTileBase
    {
        public bool tileIsActive = false;

        // Start is called before the first frame update
        public override void DeviceStart()
        {
            SetColor(Color.black);
            tileIsActive = false;
        }

        public override void DeviceUpdate()
        {

        }

        public override void DeviceJustConnected()
        {

        }

        public override void EventFromGameLogic(string eventName, string eventData)
        {
            if (eventName == "MakeActive")
            {
                // SetColor(new Color(0.05f,0.0f,0.0f));
                Debug.Log("Make Active tile");
                Debug.Log(deviceName.ToString());
                tileIsActive = true;
                this.SetRingColor(Color.red);
                this.PlayTone(500, 50);


            }
            else if (eventName == "MakePassive")
            {
                SetColor(Color.black);
                tileIsActive = false;
            }
        }

        public override void OnTapped()
        {
            if (tileIsActive)
            {
                SendEventToGameLogic("Tapped", "");
            }
        }
    }
}
