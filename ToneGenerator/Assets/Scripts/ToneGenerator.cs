/*
 * Michael T. Miyoshi
 * 
 * (school project)
 * 
 * Single tone generation
 * 
 * from:
 *  http://www.benjaminoutram.com/blog/2018/7/13/procedural-audio-in-unity-noise-and-tone
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
public class ToneGenerator : MonoBehaviour
{
//    [Range(1, 20000)]
//    public float frequency1;

//    [Range(1, 20000)]
//    public float frequency2;

    //public float sampleRate = 44100;

    private float sampleFrequency = 48000;
    [Range(0f, 1f)]
    public float noiseRatio = 0.5f;

    // Noise
    [Range(-1f, 1f)]
    public float offset;
    public float cutoffOn = 800;
    public float cutoffOff = 100;
    public bool cutOff;

    // Tonal
    public float frequency = 440f;
    public float gain = 5.0f; //0.05f;  //0.05f gives too much noise

    private float increment;
    private float phase;

    System.Random rand = new System.Random();
    AudioLowPassFilter lowPassFilter;

    //    public float waveLength = 2.0f;

    //    AudioSource audioSource;
    //    TreeInstance timeIndex = 0;

    private void Awake()
    {
        sampleFrequency = AudioSettings.outputSampleRate;
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        Update();
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        float tonalPart = 0f;
        float noisePart = 0f;

        increment = frequency * 2f * Mathf.PI / sampleFrequency;

        for(int i = 0; i < data.Length; i++)
        {
            noisePart = noiseRatio * (float)(rand.NextDouble() * 2.0 - 1.0 + offset);

            phase = phase + increment;
            if (2 * Mathf.PI < phase) { phase = 0; }

            tonalPart = (1f - noiseRatio) * (float)(gain * Mathf.Sin(phase));

            data[i] = noisePart + tonalPart;

            if(channels == 2)
            {
                data[i + 1] = data[i];
                i++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        lowPassFilter.cutoffFrequency = cutOff ? cutoffOn : cutoffOff;
    }
}
