using UnityEngine;
using NaughtyAttributes;

using System.Collections;

namespace Exact.Example
{
    [RequireComponent(typeof(Device))]
    public class TonePlayer : DeviceComponent
    {
        public override string GetComponentType() { return "tone_player"; }

        [SerializeField, OnValueChanged("OnVolumeChanged"), Range(0, 1)]
        float volume = 1;

        protected override void Awake()
        {
            base.Awake();

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0; // Force 2D sound
            audioSource.Stop(); // Avoids the audiosource starting to play automatically

            audioSource.volume = volume;
        }

        public void OnConnect()
        {
            SetVolume(volume, true);
        }

        ///<summary>
        /// Plays a tone with the given frequency.
        /// Sends the frequency and duration to the physical tone player, making it play a tone at that frequency for the given duration.
        ///</summary>
        ///<param name="frequency">Frequency of the tone to play in Hz.</param>
        ///<param name="duration">Duration of the tone to play in seconds.</param>
        public void PlayTone(int frequency, float duration)
        {
            this.frequency = frequency;
            string payload = frequency.ToString() + "/" + Mathf.RoundToInt(duration * 1000).ToString();
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

        /// <summary>
        /// Sets the volume of the tone player
        /// </summary>
        /// <param name="volume">The volume as a value from 0 to 1</param>
        public void SetVolume(float volume, bool forceUpdate = false)
        {
            if (this.volume != volume || forceUpdate)
            {
                this.volume = volume;
                audioSource.volume = volume;
                SendAction("set_volume", Mathf.RoundToInt(volume * 100));
            }
        }

        private IEnumerator StopAudioAfterDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            audioSource.Stop();
        }

        //
        // Value changed callbacks
        //

        private void OnVolumeChanged()
        {
            SetVolume(volume, true);
        }

        //
        // Audio player and sine wave generator 
        //

        public float sampleRate = 44100;
        public float waveLengthInSeconds = 2.0f;

        AudioSource audioSource;
        int timeIndex = 0;
        int frequency;

        /// <summary>
        /// If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.
        /// OnAudioFilterRead is called every time a chunk of audio is sent to the filter.
        /// </summary>
        private void OnAudioFilterRead(float[] data, int channels)
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

        private float CreateSine(int timeIndex, int frequency, float sampleRate)
        {
            return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
        }
    }
}
