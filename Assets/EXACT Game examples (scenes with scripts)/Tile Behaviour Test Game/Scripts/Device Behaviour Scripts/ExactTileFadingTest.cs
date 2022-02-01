using UnityEngine;

namespace ExactFramework
{
    // Used class methods:
    //   public void SetColor(Color)
    //   public void SetIntensity(intensity)  // 0..100
    //   public void StartFading(Color, fromIntensity, toIntensity, durationMs);

    public class ExactTileFadingTest : ExactTileBase
    {

        public bool lightIsOn = true;

        // Start is called before the first frame update
        public override void DeviceStart()
        {
            SetColor(Color.yellow);
            SetIntensity(70);
        }

        public override void DeviceUpdate()
        {

        }

        public override void OnTapped()
        {
            PlayTone(500, 50);
            lightIsOn = !lightIsOn;
            if (lightIsOn)
            {
                StartFading(Color.yellow, 0, 70, 2000);
            }
            else
            {
                StartFading(Color.yellow, 70, 0, 500);
            }
        }
    }
}
