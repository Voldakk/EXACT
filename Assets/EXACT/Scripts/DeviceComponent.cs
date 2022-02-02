using UnityEngine;

namespace Exact
{
    ///<summary>
    /// A device component class representing a physical component connected to the device.
    ///</summary>
    [RequireComponent(typeof(Device))]
    public abstract class DeviceComponent : MonoBehaviour
    {
        ///<summary>
        /// Twin object variable reference. Used by component to call message sending, etc.
        ///</summary>
        public Device device { get; private set; }

        ///<summary>
        /// The type of the component used for message sending and updates.
        ///</summary>
        public abstract string GetComponentType();
        private string componentType;

        /// <summary>
        /// Awake is called when the script instance in being loaded
        /// </summary>
        protected virtual void Awake()
        {
            device = GetComponent<Device>();
            componentType = GetComponentType();
        }

        public virtual void OnConnect()
        {

        }

        public virtual void OnDisconnect()
        {

        }


        ///<summary>
        /// Called by the twin device on incoming messages to update the component. 
        ///</summary>
        ///<param name="eventType">Name of the variable on the component.</param>
        ///<param name="payload">The variables value</param>
        public virtual void OnValue(string variable, byte[] value) { }

        ///<summary>
        /// Called by the twin device on incoming messages to update the component. 
        ///</summary>
        ///<param name="eventType">Name of the event type on the component.</param>
        ///<param name="payload">Payload of the MQTT message</param>
        public virtual void OnEvent(string eventType, byte[] payload) { }


        protected void SendAction(string action, byte[] payload)
        {
            device.SendAction(componentType, action, payload);
        }

        protected void SendAction(string action, string payload)
        {
            SendAction(action, System.Text.Encoding.Default.GetBytes(payload));
        }

        protected void SendAction(string action, int payload)
        {
            SendAction(action, new byte[] { (byte)payload });
        }

        protected void SendAction(string action, bool payload)
        {
            int value = payload ? 1 : 0;
            SendAction(action, new byte[] { (byte)value });
        }
    }
}