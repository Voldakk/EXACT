using UnityEngine;
using NaughtyAttributes;

using System.Collections;

namespace Exact.Example
{
    public class FollowTheRedDot : MonoBehaviour
    {
        [SerializeField, Required]
        ExactManager exactManager;

        [SerializeField]
        bool waitForAllConnected = false;

        Device active = null;

        void Start()
        {
            StartCoroutine(Startup());
        }

        IEnumerator Startup()
        {
            if(waitForAllConnected)
            {
                Debug.Log("Waiting for devices");
                while (!exactManager.AllDevicesConnected())
                {
                    yield return null;
                }
            }
            
            while (active == null)
            {
                yield return null;
                var devices = exactManager.GetConnectedDevices();
                if (devices.Count > 0)
                {
                    SetActive(devices[0]);
                }
            }
            Debug.Log("Startup complete");
        }

        public void OnTapped(Device device)
        {
            if (device != active) { return; }

            var devices = exactManager.GetDevicesWithComponent<Device>();
            if (devices.Count <= 1) { return; }

            active.GetComponent<LedRing>().SetColor(Color.black);
            devices.Remove(active);

            int i = Random.Range(0, devices.Count);
            SetActive(devices[i]);
        }

        private void SetActive(Device device)
        {
            active = device;
            active.GetComponent<LedRing>().SetColor(Color.red);
            active.GetComponent<TonePlayer>().PlayTone(500, 0.1f);
        }
    }
}
