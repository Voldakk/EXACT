using UnityEngine;
using System;

using System.Threading;
using System.Collections.Generic;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ExactFramework
{
    ///<summary>
    /// MQTT handler class for receiving and sending MQTT messages to the system.
    /// Holds references to all the twin objects in the game client and sends mqtt message updates to them accordingly.
    ///</summary>
    public class MQTTHandler
    {

        ///<summary>
        ///MQTTClient class object from the MQTT Library used.
        ///</summary>
        private MqttClient client = null;
        ///<summary>
        ///List of the twin objects added to the system.
        ///<summary>
        private List<TwinObject> twinObjects = new List<TwinObject>();
        ///<summary>
        ///Message buffer list used for incoming messages
        ///</summary>
        private List<MessagePair> msgBuffer = new List<MessagePair>();

        private int delay = 0;///Delay used when sending messages.


        private Thread connectThread = null;
        ///<summary>
        /// Constructor and initialization of the MQTT handler object. Creates the MQTT Client object with the host address and port. 
        /// Sets the message received callback method, and the client's id on the network.
        /// Sets the client to subscribe to the unity topic on the MQTT network.
        ///</summary>
        ///<param name="hostaddress">MQTT broker's ip on the network. Defaults to 127.0.0.1.</param>
        ///<param name="port">MQTT broker's port on the network. Defaults to 1883.</param>
        public MQTTHandler(string hostaddress = "127.0.0.1", int port = 1883, bool debug = false)
        {
            Debug.Log("Before thread: ");
            if (!debug)
            { // Added DS..
                connectThread = new Thread(() =>
                {
                    // DS client = new MqttClient(IPAddress.Parse(hostaddress), port, false, null); //Initializing the MQTT Class with ip, port, SSL level.
                    client = new MqttClient(hostaddress, port, false, null, null, MqttSslProtocols.None); //Initializing the MQTT Class with ip, port, SSL level.
                    client.MqttMsgPublishReceived += HandleMQTTMessage; //Setting up the function triggered on received messages
                    Debug.Log("Connecting: ");
                    client.Connect("Unity_Client", null, null, false, MqttMsgConnect.QOS_LEVEL_AT_MOST_ONCE, true,
                                             "exact/all_devices/reset_all_components", "mqtt_broker", true, MqttMsgConnect.KEEP_ALIVE_PERIOD_DEFAULT);
                    Debug.Log("Connected: ");
                    client.Subscribe(new string[] { "exact/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); //Subscribing to the topic
                    client.Publish("exact/all_devices/are_you_connected", new byte[] { 0 });
                }); // Start the Thread
                connectThread.Start();
            }
            Debug.Log("Outside thread: ");
        }

        public void AbortThread()
        {
            if (connectThread != null)
            {
                connectThread.Abort();
            }
        }

        ///<summary>
        ///Sends a simple message to the MQTT network. Message built on TwinObject.
        ///</summary>
        ///<param name="topic">Topic of the MQTT message sent.</param>
        ///<param name="payload">Payload of the MQTT message sent.</param>
        internal void SendDeviceMessage(string topic, byte[] payload)
        {
            if (client != null)
            {
                client.Publish(topic, payload);
                if (delay != 0)
                {
                    System.Threading.Thread.Sleep(delay);
                    delay = 0;
                }
            }
        }

        ///<summary>
        ///MQTT message handler. Handles incoming MQTT messages and creates a message pair object with the topic and payload. The message pair is added to the msgBuffer object which is called in the standard unity thread for messages to be further handled.
        ///<summary>
        ///<param name="sender"></param>
        ///<param name="e">Message object created by the MQTT client. Contains the topic and payload.</param>
        void HandleMQTTMessage(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.Log("Received: " + e.Topic);
            msgBuffer.Add(new MessagePair(e.Topic, e.Message));
        }

        /*
        Update function called from unity thread to handle messages in the unity thread.
        */
        public void Update()
        {
            //if (AllDevicesConnected())
            //{
            foreach (TwinObject obj in twinObjects)
            {
                if (obj.GetLinkStatus())
                {
                    if (obj.actionMsgBuffer.Count > 0)
                    {
                        delay = obj.actionMsgBuffer.Count * 50;
                        string actionMessageString = "";
                        List<byte> payloads = new List<byte>();
                        actionMessageString += "exact/" + obj.GetDeviceID() + "/action";
                        foreach (MessagePair mp in obj.actionMsgBuffer)
                        {
                            // DS   actionMessageString += "/" + mp.topic;
                            // DS   payloads.AddRange(mp.payload);
                            delay = 50; // DS
                            SendDeviceMessage(actionMessageString + "/" + mp.topic, mp.payload);
                        };
                        obj.actionMsgBuffer.Clear();
                    }
                    // DS obj.actionMsgBuffer = new List<MessagePair>();
                    // DS SendDeviceMessage(actionMessageString, payloads.ToArray());

                    if (obj.getMsgBuffer.Count > 0)
                    {
                        delay = obj.getMsgBuffer.Count * 20;
                        string getMessageString = "";
                        getMessageString += "exact/" + obj.GetDeviceID() + "/get";
                        foreach (MessagePair mp in obj.getMsgBuffer)
                        {
                            //DS getMessageString += "/" + mp.topic;
                            //DS obj.getMsgBuffer.Remove(mp);
                            delay = 50; // DS
                            SendDeviceMessage(getMessageString + "/" + mp.topic, mp.payload);
                        };
                        obj.getMsgBuffer.Clear();

                        // DS obj.getMsgBuffer = new List<MessagePair>();
                        // DS SendDeviceMessage(getMessageString, new byte[] { 0 });
                    }
                }
            }
            //}

            while (msgBuffer.Count != 0)
            {
                MessagePair message = msgBuffer[0];
                msgBuffer.RemoveAt(0);
                String[] topicSplit = { " ", " ", " ", " ", " " };
                topicSplit = message.topic.Split('/');
                if (topicSplit[0] == "exact")
                {
                    if (topicSplit[1] == "connected")
                    {
                        Debug.Log("Connect message received!");
                        DeviceConnect(topicSplit);
                    }
                    else if (topicSplit[1] == "disconnected")
                    {
                        DeviceDisconnect(topicSplit[2]);
                    }
                    else if (topicSplit[1] == "device_message")
                    {
                        if (topicSplit[3] == "event")
                        {
                            DeviceEvent(topicSplit, message.payload);
                        }
                        else if (topicSplit[3] == "value")
                        {
                            DeviceValue(topicSplit, message.payload);
                        }
                        //   else if (topicSplit[3] == "ping")
                        //   {
                        //       DevicePing(topicSplit[2]);
                        //   }
                    }
                }
            }
        }

        ///<summary>
        ///Method called when a value type MQTT message is received. Sends the split topic and payload to the corresponding twin object.
        ///</summary>
        ///<param name="topicSplit">MQTT message topic split to a string array.</param>
        ///<param name="payload">MQTT message payload.</param>
        private void DeviceValue(string[] topicSplit, byte[] payload)
        {
            TwinObject to = GetObjectByID(topicSplit[2]);
            if (to != null)
            {
                to.ValueMessage(topicSplit, payload);
            }
        }

        ///<summary>
        ///Method called when an event type MQTT message is received. Sends the split topic and payload to the corresponding twin object.
        ///</summary>
        ///<param name="topicSplit">MQTT message topic split to a string array.</param>
        ///<param name="payload">MQTT message payload.</param>
        private void DeviceEvent(string[] topicSplit, byte[] payload)
        {
            TwinObject to = GetObjectByID(topicSplit[2]);
            if (to != null)
            {
                to.EventMessage(topicSplit, payload);
            }
        }

        ///<summary>
        ///Method called when a ping type MQTT message is received. Update's the twin object's link status if needed and calls the PingResponse method on the twin object.
        ///</summary>
        ///<param name="deviceID">Message sender's ID.</param>
        private void DevicePing(string deviceID)
        {
            TwinObject to = GetObjectByID(deviceID);
            if (to != null)
            {
                if (!to.GetLinkStatus())
                {
                    to.SetLinkStatus(true);
                }
                to.PingResponse();
            }
        }


        private void DeviceDisconnect(string deviceID)
        {
            TwinObject to = GetObjectByID(deviceID);
            if (to != null)
            {
                if (to.GetLinkStatus())
                {
                    to.SetLinkStatus(false);
                }
            }
        }



        ///<summary>
        ///Called when a new device connects. Extracts the configuration and ID of the device.
        ///If the ID exists, it reconnects the device, if not it checks to find a configuration that fits and links that.
        ///</summary>
        ///<param name="topicSplit">String array of the split topic from the MQTT connect message</param>
        /// unity/connect/devicemacid/devicetype/devicename
        private void DeviceConnect(string[] topicSplit)
        {
            Debug.Log(topicSplit[2]);
            Debug.Log("New Device detected!");


            foreach (TwinObject obj in twinObjects)
            {
                if (obj.GetLinkStatus() == false)
                {
                    Debug.Log("Found object without link!");
                    if (obj.GetDeviceID() == topicSplit[2])
                    {
                        Debug.Log("ID Connect!");
                        obj.SetLinkStatus(true);
                        obj.LinkDevice(topicSplit[2]);
                        return;
                    }
                }
            }

            Debug.Log("No MAC_ID connection!");
            foreach (TwinObject obj1 in twinObjects)
            {
                if (obj1.GetLinkStatus() == false)
                {
                    if (obj1.useDeviceName)
                    {
                        Debug.Log("Object is using name DS!");
                        Debug.Log(obj1.GetDeviceName());
                        Debug.Log(topicSplit[4]);
                        if (obj1.GetDeviceName() == topicSplit[4])
                        {
                            Debug.Log("Name Connect!");
                            obj1.LinkDevice(topicSplit[2]);
                            return;
                        }
                    }
                }
            }


            Debug.Log("No device connection!");
            foreach (TwinObject obj2 in twinObjects)
            {
                if (obj2.GetLinkStatus() == false)
                {
                    if (obj2.useConfigName && (obj2.GetConfigName() == topicSplit[3]))
                    {
                        Debug.Log("Config Connect!");
                        obj2.LinkDevice(topicSplit[2]);
                        obj2.SetDeviceName(topicSplit[4]);
                        return;
                    }
                }
            }


            Debug.Log("Link not possible!");
            //   SendDeviceMessage(topicSplit[2] + "/ping", new byte[] { 0 });
        }

        ///<summary>
        ///Checks the list of TwinObjects and returns the one with the corresponding device ID.
        ///</summary>
        ///<param name="deviceID">Device ID of twin object to get.</param>
        ///<returns>Returns a TwinObject object with the ID.</returns>
        private TwinObject GetObjectByID(string deviceID)
        {
            foreach (TwinObject obj in twinObjects)
            {
                if (obj.GetDeviceID() == deviceID)
                {
                    return obj;
                }
            }
            return null;
        }

        ///<summary>
        ///Returns the list of TwinObjects in the MQTT Handler.
        ///</summary>
        ///<returns>List of TwinObjects</returns>
        public List<TwinObject> GetTwinObjectList()
        {
            return twinObjects;
        }

        ///<summary>
        ///Returns the list of TwinObjects that have been connected to a physical device.
        ///</summary>
        ///<returns>List of connected TwinObjects</returns>
        public List<TwinObject> GetConnectedTwinObjects()
        {
            List<TwinObject> temp = new List<TwinObject>();
            foreach (TwinObject obj in twinObjects)
            {
                if (obj.GetLinkStatus())
                {
                    temp.Add(obj);
                }
            }
            return temp;
        }

        ///<summary>
        /// Adds a Twin Object to the list of TwinObjects to be controlled and updated by the MQTT Handler.
        ///</summary>
        ///<param name="obj">A TwinObject object.</param>
        public void AddTwinObject(TwinObject obj)
        {
            obj.SetMQTTHandler(this);
            if (obj.useDeviceName)
            {
                twinObjects.Insert(0, obj);
            }
            else
            {
                twinObjects.Add(obj);
            }
        }

        ///<summary>
        ///Checks to see if all TwinObjects in the list has connected to a physical device.
        ///</summary>
        ///<returns>Boolean value.</returns>
        public bool AllDevicesConnected()
        {
            bool connected = true;
            foreach (TwinObject obj in twinObjects)
            {
                if (obj.GetLinkStatus() == false)
                {
                    connected = false;
                    break;
                }
            }
            return connected;
        }
    }
}