using UnityEngine;
using UnityEngine.Events;

namespace Exact.Example
{
    [RequireComponent(typeof(Device))]
    public class IMU : DeviceComponent
    {
        public override string GetComponentType()
        {
            return "imu";
        }
        public UnityEvent OnTap;

        public override void OnEvent(string eventType, byte[] payload)
        {
            switch (eventType)
            {
                case "tapped":
                    Tap();
                    break;
                default: break;
            }
        }

        public void Tap()
        {
            Debug.Log("Tap!");
            OnTap.Invoke();
        }
    }
}
