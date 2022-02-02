using UnityEngine;

using System.Collections.Generic;

namespace Exact
{
    public class DeviceManager : MonoBehaviour
    {
        [SerializeField]
        private string MQTTServerAddress = "192.168.0.100";

        [SerializeField]
        private bool waitForAllConnected = true;

        private MQTTHandler mqttHandler = null;

        List<Device> devicesInScene = new List<Device>();

        private void Awake()
        {
            Debug.Log("Creating MQTTHandler");
            mqttHandler = new MQTTHandler(MQTTServerAddress, 1883);

            foreach (Device to in FindObjectsOfType<Device>())
            {
                Debug.Log(to);
                devicesInScene.Add(to);
                mqttHandler.AddTwinObject(to);
            }
        }

        private void OnDestroy()
        {
            if (mqttHandler != null)
            {
                mqttHandler.SendMessageImediate("exact/all_devices/reset_all_components/unity_quit");
                mqttHandler.Shutdown();
            }
            Debug.Log("Application ending after " + Time.time + " seconds");
        }

        void Update()
        {
            if (!waitForAllConnected || mqttHandler.AllDevicesConnected())
            {
                mqttHandler.Update();
            }
        }

        public List<T> GetDevicesWithComponent<T>(bool onlyLinked = true) where T : MonoBehaviour
        {
            List<T> devices = new List<T>();
            var allDevices = onlyLinked ? GetConnectedDevicesInScene() : devicesInScene;
            foreach (Device device in allDevices)
            {
                T comp = device.GetComponent<T>();
                if (comp != null)
                {
                    devices.Add(comp);
                }
            }
            return devices;
        }

        public List<Device> GetDevicesInScene()
        {
            return new List<Device>(devicesInScene);
        }

        public List<Device> GetConnectedDevicesInScene()
        {
            var devices = new List<Device>();
            foreach (Device device in devicesInScene)
            {
                if (device.linked) { devices.Add(device); }
            }
            return devices;
        }
    }
}
