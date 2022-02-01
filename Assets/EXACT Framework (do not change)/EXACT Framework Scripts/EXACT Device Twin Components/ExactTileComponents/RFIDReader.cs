namespace ExactFramework
{
    public class RFIDReader : DeviceComponent
    {

        byte[] lastReadID;

        public byte[] GetLastReadID()
        {
            return lastReadID;
        }

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            if (eventType == "rfid_enter") //Subject to change
            {
                lastReadID = payload;
                device.InvokeEvent("rfid_reader.rfid_enter");
            }
            else if (eventType == "rfid_leave") //Subject to change
            {
                lastReadID = payload;
                device.InvokeEvent("rfid_reader.rfid_leave");
            }
        }

        public void EnableRFID()
        {
            device.SendActionMessage("rfid_reader/enable_rfid", 0);
        }

        public void DisableRFID()
        {
            device.SendActionMessage("rfid_reader/disable_rfid", 0);
        }

    }
}
