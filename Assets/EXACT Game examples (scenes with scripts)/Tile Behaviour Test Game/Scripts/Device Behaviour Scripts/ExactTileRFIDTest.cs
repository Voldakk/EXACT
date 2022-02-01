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
    public class ExactTileRFIDTest : ExactTileBase
    {

        // Start is called before the first frame update
        public override void DeviceStart()
        {

        }

        public override void DeviceUpdate()
        {

        }

        public override void DeviceJustConnected()
        {

        }

        public override void EventFromGameLogic(string eventName, string eventData)
        {  /*
            if (eventName == "DoSomething")
            {
            } 
            */
        }

        public override void OnTapped()
        {
        }

        public override void OnRFIDEnter(string RFIDInHex)
        {
            SetColor(new Color(1, 1, 0));
        }

        public override void OnRFIDLeave(string RFIDInHex)
        {
            SetColor(new Color(0, 1, 1));
        }
    }
}
