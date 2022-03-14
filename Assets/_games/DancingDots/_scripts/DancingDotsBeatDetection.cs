using Antura.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsBeatDetection : MonoBehaviour
    {
        public float minInterval = 0.3f, pitchThreshold = 2000f, RmsThreshold = 0.19f;
        DancingDotsQuadManager disco;
        //DancingDotsGame game;
        bool canBeat = true;

        public float RmsValue;
        public float DbValue;
        public float PitchValue;

        private const int QSamples = 2048;
        private const float RefValue = 0.1f;
        private const float Threshold = 0.02f;

        float[] _samples;
        private float[] _spectrum;
        private float _fSample;

        AudioSourceWrapper sourceWrapper;

        void Start()
        {
            //AudioProcessor processor = FindObjectOfType<AudioProcessor>();
            //processor.addAudioCallback(this);

            disco = GameObject.Find("Quads").GetComponent<DancingDotsQuadManager>();
            //game = GameObject.Find("Dancing Dots Game Manager").GetComponent<DancingDotsGame>();
            _samples = new float[QSamples];
            _spectrum = new float[QSamples];
            _fSample = AudioSettings.outputSampleRate;
        }

        IEnumerator reset()
        {
            yield return new WaitForSeconds(minInterval);
            canBeat = true;

        }

        void Update()
        {
            if (!AnalyzeSound())
                return;

            if (PitchValue > pitchThreshold || RmsValue > RmsThreshold)
            {
                if (canBeat)
                {
                    canBeat = false;
                    StartCoroutine(reset());
                    disco.swap();
                    disco.swap();
                    disco.swap();
                    disco.swap();
                    /*disco.swap(game.DDBackgrounds);
                    disco.swap(game.DDBackgrounds);
                    disco.swap(game.DDBackgrounds);
                    disco.swap(game.DDBackgrounds);*/
                }
            }

            // string text = "RMS: " + RmsValue.ToString("F2") +
            // " (" + DbValue.ToString("F1") + " dB)" +
            // "Pitch: " + PitchValue.ToString("F0") + " Hz";
            //Debug.LogWarning(text);
        }

        bool AnalyzeSound()
        {
            if (sourceWrapper == null)
                return false;

            var source = sourceWrapper.CurrentSource == null ? null : sourceWrapper.CurrentSource.audioSource;

            if (source == null)
                return false;

            source.GetOutputData(_samples, 0); // fill array with samples
            int i;
            float sum = 0;
            for (i = 0; i < QSamples; i++)
            {
                sum += _samples[i] * _samples[i]; // sum squared samples
            }
            RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
            DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
            if (DbValue < -160)
                DbValue = -160; // clamp it to -160dB min
                                // get sound spectrum
            source.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
            float maxV = 0;
            var maxN = 0;
            for (i = 0; i < QSamples; i++)
            { // find max
                if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                    continue;

                maxV = _spectrum[i];
                maxN = i; // maxN is the index of max
            }
            float freqN = maxN; // pass the index to a float variable
            if (maxN > 0 && maxN < QSamples - 1)
            { // interpolate index using neighbours
                var dL = _spectrum[maxN - 1] / _spectrum[maxN];
                var dR = _spectrum[maxN + 1] / _spectrum[maxN];
                freqN += 0.5f * (dR * dR - dL * dL);
            }
            PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency

            return true;
        }

        public void Initialize(AudioSourceWrapper source)
        {
            this.sourceWrapper = source;
        }
    }
}
