using UnityEngine;
using UnityEngine.UI;

namespace Exact.Example
{
    [RequireComponent(typeof(Text))]
    public class DiceReader : MonoBehaviour
    {
        Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
            text.text = "";
        }

        public void SetDiceNumber(int number)
        {
            if (number < 1 || number > 6) 
            { 
                text.text = ""; 
            }
            else 
            { 
                text.text = number.ToString(); 
            }
        }
    }
}
