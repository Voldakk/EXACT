using UnityEngine;

using System.Collections;

namespace Exact.Example
{
    [RequireComponent(typeof(Device))]
    public class TonePlayer : DeviceComponent
    {
        public override string GetComponentType()
        {
            return "tone_player";
        }

        private int frequency;

        public float sampleRate = 44100;
        public float waveLengthInSeconds = 2.0f;

        AudioSource audioSource;
        int timeIndex = 0;

        protected override void Awake()
        {
            base.Awake();

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0; // Force 2D sound
            audioSource.Stop(); // Avoids the audiosource starting to play automatically
        }

        ///<summary>
        /// Plays a tone with the given frequency.
        /// Sends the frequency and duration to the physical tone player, making it play a tone at that frequency for the given duration.
        ///</summary>
        ///<param name="frequency">Frequency of the tone to play in Hz.</param>
        ///<param name="duration">Duration of the tone to play in seconds.</param>
        public void PlayTone(int frequency, float duration)
        {
            string payload = frequency.ToString() + "/" + ((int)(duration * 1000)).ToString();
            SendAction("tone", payload);
            audioSource.Play();
            StartCoroutine(StopAudioAfterDuration(duration));
        }

        ///<summary>
        /// Sends a stop signal to the physical tone player, making it stop playing whatever tone is currently playing.
        ///</summary>
        public void StopTone()
        {
            SendAction("no_tone", 0);
            audioSource.Stop();
        }

        IEnumerator StopAudioAfterDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            audioSource.Stop();
        }

        /// <summary>
        /// If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.
        /// OnAudioFilterRead is called every time a chunk of audio is sent to the filter.
        /// </summary>
        void OnAudioFilterRead(float[] data, int channels)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                float value = CreateSine(timeIndex, frequency, sampleRate);
                data[i] = value;

                if (channels == 2)
                    data[i + 1] = value;

                timeIndex++;

                // If timeIndex gets too big, reset it to 0
                if (timeIndex >= (sampleRate * waveLengthInSeconds))
                {
                    timeIndex = 0;
                }
            }
        }

        public float CreateSine(int timeIndex, int frequency, float sampleRate)
        {
            return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
        }
    }
}
