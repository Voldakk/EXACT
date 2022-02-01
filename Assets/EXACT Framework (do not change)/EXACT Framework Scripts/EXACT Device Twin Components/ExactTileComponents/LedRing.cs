using UnityEngine;

namespace ExactFramework
{
    ///<summary>
    ///Digital representation of a ring light component.
    ///</summary>
    public class LedRing : DeviceComponent
    {

        ///<summary>
        ///Change this variable for a uniform color of the ring light.
        ///</summary>
        private Color color = Color.black;

        ///<summary>
        ///Variable for the maximum number of LEDs for this ring light object. Set once when initialized.
        ///</summary>
        private readonly int maxNumLeds;

        ///<summary>
        ///Variable for number of active LEDs for the ring light. Default 24.
        ///</summary>
        private int numOfLeds = 24;

        public override void Start()
        {
            base.Start();
            color = new Color(0, 0, 0);
            Init(numOfLeds);
        }

        ///<summary>
        ///Initialization method for the ring light. Should be called once, and takes the number of LEDs expected for this ring light.
        ///</summary>
        ///<param name="numberOfLeds">Sets number of LEDs active to start with and the number of LEDs max.</param>
        public void Init(int numberOfLeds)
        {
        }


        ///<summary>
        ///Sets the uniform color of the ring light. Sends the color over MQTT to the device.
        ///</summary>
        ///<param name="color">Unity Color object.</param>
        public void SetColor(Color color)
        {
            byte cr = (byte)(color.r * 255);
            byte cg = (byte)(color.g * 255);
            byte cb = (byte)(color.b * 255);

            byte tcr = (byte)(this.color.r * 255);
            byte tcg = (byte)(this.color.g * 255);
            byte tcb = (byte)(this.color.b * 255);

            if ((cr != tcr) || (cg != tcr) || (cb != tcb))
            {
                this.color = color;
                string colorString = cr.ToString() + "/" + cg.ToString() + "/" + cb.ToString();
                device.SendActionMessage("led_ring/set_color_all_leds", colorString);
            }
        }

        ///<summary>
        ///Sets the color of an individual LED on the ring light.
        ///</summary>
        ///<param name="color">Unity Color object.</param>
        ///<param name="i">Index of the LED to change in the LED list.</param>
        public void SetIndividualColor(Color color, int i)
        {
            string colorString = i.ToString() + "/" + ((byte)(color.r * 255)).ToString() + "/" +
                   ((byte)(color.g * 255)).ToString() + "/" + ((byte)(color.b * 255)).ToString();
            device.SendActionMessage("led_ring/set_color_one_led", colorString);
        }

        public void SetIntensity(int i)
        {
            string colorString = i.ToString();
            device.SendActionMessage("led_ring/set_intensity", colorString);
        }

        public void StartFading(Color fadeColor, int from, int to, long pulseLengthMs)
        {
            Debug.Log("Ledring fade");
            Debug.Log(fadeColor.ToString());
            string colorString = ((byte)(fadeColor.r * 255)).ToString() +
                          "/" + ((byte)(fadeColor.g * 255)).ToString() +
                          "/" + ((byte)(fadeColor.b * 255)).ToString() +
                          "/" + from.ToString() +
                          "/" + to.ToString() +
                          "/" + pulseLengthMs.ToString();
            device.SendActionMessage("led_ring/start_fading", colorString);
        }

        public void StartPulse(Color fadeColor, int from, int to, long pulseLengthMs)
        {
            Debug.Log("Ledring pulse");
            Debug.Log(fadeColor.ToString());
            string colorString = ((byte)(fadeColor.r * 255)).ToString() +
                          "/" + ((byte)(fadeColor.g * 255)).ToString() +
                          "/" + ((byte)(fadeColor.b * 255)).ToString() +
                          "/" + from.ToString() +
                          "/" + to.ToString() +
                          "/" + pulseLengthMs.ToString();
            device.SendActionMessage("led_ring/start_pulse", colorString);
        }

        public void StopPulse()
        {
            Debug.Log("Ledring stop pulse");
            device.SendActionMessage("led_ring/stop_pulse", "");
        }
        ///<summary>
        ///Gets the uniform color of the ring light.
        ///</summary>
        ///<returns>Unity Color object.</returns>
        public Color GetColor()
        {
            return color;
        }
    }
}
