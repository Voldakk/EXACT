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
    public class ExactGameLogicRedDot : ExactGameLogicBase
    {
        private List<ExactTileBase> deviceList = null;
        private ExactTileBase[] deviceArray = new ExactTileBase[0];

        public override void GameLogicStart()
        {
            deviceList = GetDeviceList();
            if (deviceList != null)
            {
                deviceArray = deviceList.ToArray();
                debugArray(deviceArray);
                Debug.Log(deviceArray.Length.ToString());
                if (deviceArray.Length > 0)
                {
                    Debug.Log("Make Active");
                    SendEventToDevice(deviceArray[0], "MakeActive", "");
                }
            }
        }


        public override void GameLogicUpdate()
        {

        }


        public override void EventFromDevice(ExactTileBase exactTileBase, string eventName, string eventData)
        {
            if (eventName == "Tapped")
            {
                List<ExactTileBase> clone = new List<ExactTileBase>(deviceList); // Make a clone
                if (clone.Contains(exactTileBase))
                {
                    clone.Remove(exactTileBase);
                    ExactTileBase[] passiveDevices = clone.ToArray();
                    debugArray(passiveDevices);
                    if (passiveDevices.Length > 0)
                    {
                        System.Random r = new System.Random();
                        int nextActive = r.Next(0, passiveDevices.Length);
                        Debug.Log(nextActive);
                        ExactTileBase nextActiveDevice = passiveDevices[nextActive];
                        SendEventToDevice(exactTileBase, "MakePassive", "");
                        SendEventToDevice(nextActiveDevice, "MakeActive", "");
                    }
                }
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
