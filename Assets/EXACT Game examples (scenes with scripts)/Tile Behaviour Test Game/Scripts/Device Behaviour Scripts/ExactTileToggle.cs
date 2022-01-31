using UnityEngine;

namespace ExactFramework {

    // Used class methods:
    //   public void SetColor(Color)
    //   public void SetIntensity(int)
    //
 
    public class ExactTileToggle : ExactTileBase
    {

        public bool redGreen = false;

        // Start is called before the first frame update
        public override void DeviceStart()
        {
            SetIntensity(20);
            SetColor(Color.yellow);
        }

        public override void DeviceUpdate()
        {

        }

        public override void OnTapped()
        {
            redGreen = !redGreen;
            if (redGreen) {
                SetColor(Color.blue);  
            }
            else
            {
                SetColor(Color.yellow);
            }
        }
    }
}
