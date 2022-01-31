using System.Collections;
using System.Collections.Generic;
using ExactFramework;
using UnityEngine;
using System;


namespace ExactFramework {
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
    public class ExactTileFadingDot : ExactTileBase
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
                tileIsActive = true;
                this.PlayTone(500, 50);
				StartFading(Color.red, 0, 70, 3000);

            }
            else if (eventName == "MakePassive")
            {
                StartFading(Color.red, 70, 0, 500);
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
