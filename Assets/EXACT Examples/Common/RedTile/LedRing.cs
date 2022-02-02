using UnityEngine;

namespace Exact.Example
{
    [RequireComponent(typeof(Device))]
    public class LedRing : DeviceComponent
    {
        public override string GetComponentType()
        {
            return "led_ring";
        }

        [SerializeField]
        Color uniformColor = Color.black;
        bool uniform = true;

        [SerializeField]
        int numLeds = 24;

        [SerializeField]
        ColorRingBase colorRing;

        protected override void Awake()
        {
            base.Awake();

            colorRing.SetNumberOfSegments(numLeds);

            SetColor(uniformColor, true);
        }

        public override void OnConnect()
        {
            SetColor(uniformColor, true);
        }

        public void SetColor(Color color, bool forceUpdate = false)
        {
            if (uniformColor != color || !uniform || forceUpdate)
            {
                uniform = true;
                uniformColor = color;

                colorRing.SetUniformColor(uniformColor);

                if (device != null && device.linked)
                {
                    byte cr = (byte)(uniformColor.r * 255);
                    byte cg = (byte)(uniformColor.g * 255);
                    byte cb = (byte)(uniformColor.b * 255);

                    string payload = cr.ToString() + "/" + cg.ToString() + "/" + cb.ToString();
                    SendAction("set_color_all_leds", payload);
                }
            }
        }

        public void SetColor(int led, Color color, bool forceUpdate = false)
        {
            if (colorRing.GetColor(led) != color || forceUpdate)
            {
                uniform = false;
                colorRing.SetSegmentColor(led, color);

                if (device != null && device.linked)
                {
                    byte cr = (byte)(color.r * 255);
                    byte cg = (byte)(color.g * 255);
                    byte cb = (byte)(color.b * 255);

                    string payload = ((byte)led).ToString() + "/" + cr.ToString() + "/" + cg.ToString() + "/" + cb.ToString();
                    SendAction("set_color_one_led", payload);
                }
            }
        }
    }
}
