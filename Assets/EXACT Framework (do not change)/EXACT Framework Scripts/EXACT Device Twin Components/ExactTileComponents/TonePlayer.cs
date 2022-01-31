using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExactFramework
{
    ///<summary>
    ///Digital representation of a tone player component for Arduinos.
    ///</summary>
    public class TonePlayer : DeviceComponent
    {

        ///<summary>
        ///Sends a frequency to the physical tone player, making it play a continuous tone at that frequency.
        ///</summary>
        ///<param name="frequency">Frequency of the tone to play.</param>
        public void PlayTone(int frequency)
        {
            PlayTone(frequency, 0);
        }

        ///<summary>
        ///Sends a frequency and duration to the physical tone player, making it play a tone at that frequency for the given duration.
        ///</summary>
        ///<param name="frequency">Frequency of the tone to play.</param>
        ///<param name="duration">Duration of the tone to play.</param>
        public void PlayTone(int frequency, int duration)
        {
            byte[] freqbytes = BitConverter.GetBytes(frequency);
            byte[] durbytes = BitConverter.GetBytes(duration);
            List<byte> bytesArray = new List<byte>();
            bytesArray.AddRange(freqbytes);
            bytesArray.AddRange(durbytes);
            string freqDurString = frequency.ToString() + "/" + duration.ToString();
            device.SendActionMessage("tone_player/tone", freqDurString);
        }

        ///<summary>
        ///Sends a stop signal to the physical tone player, making it stop playing whatever tone is currently playing.
        ///</summary>
        public void StopTone()
        {
            device.SendActionMessage("tone_player/no_tone", 0);
        }
    }
}
