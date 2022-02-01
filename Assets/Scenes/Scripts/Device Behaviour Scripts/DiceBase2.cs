using UnityEngine;

using System.Collections.Generic;

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
    /*
    R1,042E58F2744C80
    R2,042156F2744C80
    R3,042A57F2744C80
    R4,041C56F2744C80
    R5,043358F2744C80
    R6,042656F2744C80
    B1,047157F2744C80
    B2,046159F2744C80
    B3,045759F2744C80
    B4,046558F2744C80
    B5,045C59F2744C80
    B6,046C57F2744C80
    Y1,045259F2744C80
    Y2,044859F2744C80
    Y3,043E59F2744C80
    Y4,043A58F2744C80
    Y5,044D59F2744C80
    Y6,044359F2744C80
    */


    public class DiceBase2 : ExactTileBase
    {

        Dictionary<string, int> yellowDice = new Dictionary<string, int>
        {
            { "04:52:59:F2:74:4C:80", 1 },
            { "04:48:59:F2:74:4C:80", 2 },
            { "04:3E:59:F2:74:4C:80", 3 },
            { "04:3A:58:F2:74:4C:80", 4 }
        };

        // Start is called before the first frame update
        public override void DeviceStart()
        {
            SetIntensity(80);
            SetColor(Color.yellow);
            StartPulse(Color.yellow, 20, 80, 6000);
            configName = "dicebase2";
            EnableRFID();
        }

        public override void DeviceUpdate()
        {

        }

        public override void DeviceJustConnected()
        {
            SetColor(Color.yellow);
        }

        public override void EventFromGameLogic(string eventName, string eventData)
        {
        }

        public override void OnTapped()
        {
        }

        public override void OnRFIDEnter(string RFIDInHex)
        {
            Debug.Log("Enter:" + RFIDInHex);
            if (yellowDice.ContainsKey(RFIDInHex))
            {
                int state = yellowDice[RFIDInHex];
                Debug.Log("State:" + state);
                SendEventToGameLogic("SetGameState", state);
                PlayTone(1000, 50);
                if (state == 1)
                {
                    SetColor(Color.yellow);
                    StartPulse(Color.yellow, 20, 80, 6000);
                }
                else if (state == 2)
                {
                    StopPulse();
                    SetIntensity(70);
                    SetColor(Color.green);
                }
                else if (state == 3)
                {
                    StopPulse();
                    SetIntensity(70);
                    SetColor(Color.green);
                }
                else if (state == 4)
                {
                    StopPulse();
                    SetIntensity(70);
                    SetColor(Color.red);
                }
            }
        }

        public override void OnRFIDLeave(string RFIDInHex)
        {
            Debug.Log("Leave:" + RFIDInHex);
        }

    }
}
