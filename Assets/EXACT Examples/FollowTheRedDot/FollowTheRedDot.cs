using UnityEngine;

using System.Collections;

namespace Exact.Example
{
    public class FollowTheRedDot : MonoBehaviour
    {
        [SerializeField]
        private DeviceManager deviceManager;

        Device active = null;

        void Start()
        {
            StartCoroutine(Startup());
        }

        IEnumerator Startup()
        {
            while (active == null)
            {
                yield return null;
                var devices = deviceManager.GetDevicesWithComponent<Device>();
                if (devices.Count > 0)
                {
                    active = devices[0];
                    active.GetComponent<LedRing>().SetColor(Color.red);
                }
            }
            Debug.Log("Startup complete");
        }

        public void OnTapped(Device device)
        {
            if (device != active) { return; }

            var devices = deviceManager.GetDevicesWithComponent<Device>();
            if (devices.Count <= 1) { return; }

            active.GetComponent<LedRing>().SetColor(Color.black);
            devices.Remove(active);

            int i = Random.Range(0, devices.Count);
            active = devices[i];
            active.GetComponent<LedRing>().SetColor(Color.red);
            active.GetComponent<TonePlayer>().PlayTone(500, 0.1f);
        }
    }
}
