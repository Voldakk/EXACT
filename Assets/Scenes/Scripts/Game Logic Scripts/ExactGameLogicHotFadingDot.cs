using ExactFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ExactFramework
{
    // public void SendEventToDevice(ExactTileBase device, string eventName, string eventData)
    // public List<ExactTileBase> GetDeviceList()
    //
    public class ExactGameLogicHotFadingDot : ExactGameLogicBase
    {

        List<Color> RFID_colors = new List<Color>()
        {
            Color.red,    // Color one
            Color.green   // Color two
        };

        Dictionary<string, int> RFID_tags = new Dictionary<string, int>
        {
            { "04:2E:58:F2:74:4C:80", 0 },  // Color one tag
            { "04:21:56:F2:74:4C:80", 0 },  // Color one tag
            { "04:2A:57:F2:74:4C:80", 1 },  // Color two tag
            { "04:1C:56:F2:74:4C:80", 1 },  // Color two tag
            { "Disconnected", 0 }  // Do not touch...
        };

        private List<ExactTileBase> deviceList = null;
        private ExactTileBase[] deviceArray = new ExactTileBase[0];
        private ExactTileBase activeTile = null;

        private Color dotColor = Color.white;

        public int gameState = 1; // 1=off, 2=dot, 3=rfid
        public int speed = 1;
        public int makeActiveSpeed = 1000;
        public int makePassiveSpeed = 100;

        public int RFID_color = 0;   //



        public List<ExactTileBase> GetDotTileList()
        {
            List<ExactTileBase> allDevices = new List<ExactTileBase>(GetDeviceList()); // clone
            List<ExactTileBase> returnList = new List<ExactTileBase>();
            foreach (ExactTileBase tile in allDevices)
            {
               if ((tile.configNameDoNotChange == "exactredtile")  && (tile.linked || debugMode))
               {
                    returnList.Add(tile);
               }
            }
            return returnList;
        }

        public void StartDotGame()
        {
            deviceList = GetDotTileList();
            if (deviceList != null)
            {
                deviceArray = deviceList.ToArray();
                debugArray(deviceArray);
                Debug.Log(deviceArray.Length.ToString());
                if (deviceArray.Length > 0)
                {
                    Debug.Log("Make Active");
                    activeTile = deviceArray[0];
                    activeTile.SetColor(dotColor);
                    SendEventToDevice(activeTile, "MakeActive", makeActiveSpeed);
                }
            }
        }


        public void StartRFIDGame()
        {
            deviceList = GetDotTileList();
            if (deviceList != null)
            {
                deviceArray = deviceList.ToArray();
                debugArray(deviceArray);
                Debug.Log(deviceArray.Length.ToString());
                if (deviceArray.Length > 0)
                {
                    Debug.Log("Make Active");
                    activeTile = deviceArray[0];
                    activeTile.SetColor(RFID_colors[RFID_color]);
                    SendEventToDevice(activeTile, "MakeActive", makeActiveSpeed);
                    SendEventToDevice(activeTile, "Enable_RFID", 0);
                }
            }
        }


        public void StartTwoColorGame()
        {
            deviceList = GetDotTileList();
            if (deviceList != null)
            {
                deviceArray = deviceList.ToArray();
                debugArray(deviceArray);
                Debug.Log(deviceArray.Length.ToString());
                if (deviceArray.Length > 0)
                {
                    Debug.Log("Make Active");
                    activeTile = deviceArray[0];
                    activeTile.SetColor(RFID_colors[RFID_color]);
                    SendEventToDevice(activeTile, "MakeActive", makeActiveSpeed);
                }
            }
        }

        public override void GameLogicStart()
        {
        }


        public override void GameLogicUpdate()
        {
        }

        public void EndGame()
        {
            if (activeTile != null)
            {
                SendEventToDevice(activeTile, "Disable_RFID", 0);
                SendEventToDevice(activeTile, "MakePassive", makePassiveSpeed);
                activeTile = null;
            }
        }


        public override void EventFromDevice(ExactTileBase exactTile, string eventName, string eventData)
        {
            if ((eventName == "Tapped") && (gameState == 2))
            {
                List<ExactTileBase> clone = new List<ExactTileBase>(GetDotTileList()); // Make a clone
                if (clone.Contains(exactTile))
                {
                    clone.Remove(exactTile);
                    ExactTileBase[] passiveDevices = clone.ToArray();
                    debugArray(passiveDevices);
                    if (passiveDevices.Length > 0)
                    {
                        System.Random r = new System.Random();
                        int nextActive = r.Next(0, passiveDevices.Length);
                        Debug.Log(nextActive);
                        ExactTileBase nextActiveDevice = passiveDevices[nextActive];
                        nextActiveDevice.SetColor(dotColor);
                        SendEventToDevice(exactTile, "MakePassive", makePassiveSpeed);
                        SendEventToDevice(nextActiveDevice, "MakeActive", makeActiveSpeed);
                        activeTile = nextActiveDevice;
                    }
                }
            }
            else if ((eventName == "RFID_Enter") && (gameState == 4))
            {
              if (RFID_tags.ContainsKey(eventData)) {
               if((RFID_tags[eventData] == RFID_color) || (eventData == "Disconnected")) {
                List<ExactTileBase> clone = new List<ExactTileBase>(GetDotTileList()); // Make a clone
                if (clone.Contains(exactTile))
                {
                    clone.Remove(exactTile);
                    ExactTileBase[] passiveDevices = clone.ToArray();
                    debugArray(passiveDevices);
                    if (passiveDevices.Length > 0)
                    {
                        System.Random r = new System.Random();
                        int nextActive = r.Next(0, passiveDevices.Length);
                        Debug.Log(nextActive);
                        ExactTileBase nextActiveDevice = passiveDevices[nextActive];

                        System.Random r2 = new System.Random();
                        RFID_color = r2.Next(0, 2);  // 0..1
                        Debug.Log("Random:" + RFID_color.ToString());
                        nextActiveDevice.SetColor(RFID_colors[RFID_color]);
                        SendEventToDevice(exactTile, "Disable_RFID", 0);
                        SendEventToDevice(exactTile, "MakePassive", makePassiveSpeed);
                        SendEventToDevice(nextActiveDevice, "MakeActive", makeActiveSpeed);
                        SendEventToDevice(nextActiveDevice, "Enable_RFID", 0);
                        activeTile = nextActiveDevice;
                    }
                }
               }
              }
            }

            else if ((eventName == "Tapped") && (gameState == 3))
            {

                        List<ExactTileBase> clone = new List<ExactTileBase>(GetDotTileList()); // Make a clone
                        if (clone.Contains(exactTile))
                        {
                            clone.Remove(exactTile);
                            ExactTileBase[] passiveDevices = clone.ToArray();
                            debugArray(passiveDevices);
                            if (passiveDevices.Length > 0)
                            {
                                System.Random r = new System.Random();
                                int nextActive = r.Next(0, passiveDevices.Length);
                                Debug.Log(nextActive);
                                ExactTileBase nextActiveDevice = passiveDevices[nextActive];

                                System.Random r2 = new System.Random();
                                RFID_color = r2.Next(0, 2);  // 0..1
                                Debug.Log("Random:" + RFID_color.ToString());
                                nextActiveDevice.SetColor(RFID_colors[RFID_color]);
                                SendEventToDevice(exactTile, "Disable_RFID", 0);
                                SendEventToDevice(exactTile, "MakePassive", makePassiveSpeed);
                                SendEventToDevice(nextActiveDevice, "MakeActive", makeActiveSpeed);
                                activeTile = nextActiveDevice;
                            }
                        }
            }



        }

        public override void EventFromDevice(ExactTileBase exactTile, string eventName, int eventData)
        {
            if (eventName == "SetSpeed")
            {
                Debug.Log("Speed:" + eventData.ToString());
                speed = eventData;
                int invSpeed = -speed + 7;
                int time = 1 + 3 * (invSpeed - 1);
                makeActiveSpeed = time * 1000;
                makePassiveSpeed = time * 100;
            } else if (eventName == "SetGameState")
            {
                Debug.Log("Game State:" + eventData.ToString());
                if (eventData != gameState)
                {
                    if(eventData == 1)
                    {
                        EndGame();
                    }
                    else if (eventData == 2)
                    {
                        EndGame();
                        StartDotGame();

                    }
                    else if (eventData == 3)
                    {
                        EndGame();
                        StartTwoColorGame();

                    }
                    else if (eventData == 4)
                    {
                        EndGame();
                        StartRFIDGame();
                    }
                }
                gameState = eventData;
            }
        }

        private void debugArray(ExactTileBase[] a)
        {
            Debug.Log("all - active:");
            Debug.Log(a.Length.ToString());
            for (int i = 0; i < a.Length; i++)
            {
                Debug.Log(a[i].deviceName.ToString());
            }
        }

    }
}
