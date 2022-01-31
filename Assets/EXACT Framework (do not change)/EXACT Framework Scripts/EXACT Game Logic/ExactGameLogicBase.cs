using ExactFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework
{
    public class ExactGameLogicBase : MonoBehaviour
    {

        private MQTTHandler mqttHandler = null;

        public string MQTTServerAddress = "192.168.4.1"; //Change this if you haven't in the inspector
        protected bool waitForAllConnected;
        protected bool allDevicesConnected;
        public bool debugMode = true;

        public List<TwinObject> devicesInScene = new List<TwinObject>();

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Game start: ");
            mqttHandler = new MQTTHandler(MQTTServerAddress, 1883, debugMode);
            TwinObject[] objectsInScene = GameObject.FindObjectsOfType<TwinObject>();
            foreach (TwinObject to in objectsInScene)
            {
                if (!devicesInScene.Contains(to))
                {
                    devicesInScene.Add(to);
                }
            }
            foreach (TwinObject to in devicesInScene)
            {
                Debug.Log(to);
                if (to != null)
                {
                    mqttHandler.AddTwinObject(to);
                }
            }
        }

        public virtual void GameLogicStart()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount == 1)  // Let all devices update before this..
            {
                GameLogicStart();
            }
            GameLogicUpdate();
            mqttHandler.Update();
            if (waitForAllConnected)
            {
                if (mqttHandler.AllDevicesConnected())
                {
                    allDevicesConnected = true;
                }
            }
        }

        public virtual void GameLogicUpdate()
        {
         //   Debug.Log("Game update");
        }



    public List<T> GetDevicesWithBehavior<T>()
        {
            List<T> listOfObjects = new List<T>();
            foreach (TwinObject to in devicesInScene)
            {
                T temp = to.GetComponent<T>();
                if (temp != null)
                {
                    listOfObjects.Add(temp);
                }
            }
            return listOfObjects;
        }

        public List<ExactTileBase> GetDeviceList()  // DS
        {
            List<ExactTileBase> deviceList = new List<ExactTileBase>();
            if (mqttHandler != null)
            {
                foreach (TwinObject obj in mqttHandler.GetTwinObjectList())
                {
                    deviceList.Add((ExactTileBase)obj);
                }
                return deviceList;  // Shallow clone, but that is fine...
            }
            else
            {
                return new List<ExactTileBase>();
            }
        }

        public virtual void EventFromDevice(ExactTileBase exactTileBase, string eventName, string eventData)
        {

        }

        public virtual void EventFromDevice(ExactTileBase exactTileBase, string eventName, int eventData)
        {

        }

        public virtual void EventFromDevice(ExactTileBase exactTileBase, string eventName, Color eventData)
        {

        }

        public void SendEventToDevice(ExactTileBase device, string eventName, string eventData)
        {
            if (device != null)
            {
                device.EventFromGameLogic(eventName, eventData);
            }
        }

        public void SendEventToDevice(ExactTileBase device, string eventName, int eventData)
        {
            if (device != null)
            {
                device.EventFromGameLogic(eventName, eventData);
            }
        }

        public void SendEventToDevice(ExactTileBase device, string eventName, Color c)
        {
            if (device != null)
            {
                device.EventFromGameLogic(eventName, c);
            }
        }

        void OnApplicationQuit()
        {
            if (mqttHandler != null)
            {
                mqttHandler.SendDeviceMessage("exact/all_devices/reset_all_components/unity_quit", new byte[] { (byte)0 });
                mqttHandler.AbortThread();
            }
            Debug.Log("Application ending after " + Time.time + " seconds");
        }
    }
}
