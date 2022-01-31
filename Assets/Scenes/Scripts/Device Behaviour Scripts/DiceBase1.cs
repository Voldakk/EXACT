using System.Collections;
using System.Collections.Generic;
using ExactFramework;
using UnityEngine;
using System;
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


    public class DiceBase1 : ExactTileBase
    {

        Dictionary<string, int> blueDice = new Dictionary<string, int>
        { 
            { "04:71:57:F2:74:4C:80", 1 },
            { "04:61:59:F2:74:4C:80", 2 },
            { "04:57:59:F2:74:4C:80", 3 },
            { "04:65:58:F2:74:4C:80", 4 },
            { "04:5C:59:F2:74:4C:80", 5 },
            { "04:6C:57:F2:74:4C:80", 6 }
        };

        // Start is called before the first frame update
        public override void DeviceStart()
        {
            SetIntensity(80);
            SetColor(Color.blue);
            configName = "dicebase1";
            EnableRFID();
        }

        public override void DeviceUpdate()
        {

        }

        public override void DeviceJustConnected()
        {
            SetColor(Color.blue);
        }

        public override void EventFromGameLogic(string eventName, string eventData)
        {  
        }

        public override void OnTapped()
        {
        }

        public void IndicateSpeed(int speed)
        {

            /*
            SetColor(Color.grey);
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j < speed; j++)
                {
                    SetIndividualColor(Color.blue, j + 6 * i);
                };
            //    for (int j = speed; j < 6; j++)
            //    {
            //        SetIndividualColor(Color.black, j + 6 * i);
            //    };
            } */
          //  SetIntensity(1);
          //  SetColor(Color.black);
            SetIntensity(20 + speed * 10);
            SetColor(Color.blue);
        }

        public override void OnRFIDEnter(string RFIDInHex)
        {
            Debug.Log("Enter:" + RFIDInHex);
            if (blueDice.ContainsKey(RFIDInHex))
            {
                int speed = blueDice[RFIDInHex];
                SendEventToGameLogic("SetSpeed", speed);
                PlayTone(1000, 50);
                IndicateSpeed(speed);
            }
        }

        public override void OnRFIDLeave(string RFIDInHex)
        {
            Debug.Log("Leave:" + RFIDInHex);
        }

    }
}
