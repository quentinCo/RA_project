using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour {

    public GameObject generator;
    public int minOctave = 1;
    public float deltaTime = 0.25f;

    public int qSamples = 1024;  // array size
    public float refValue = 0.1f; // RMS value for 0 dB
    public float threshold = 0.02f;      // minimum amplitude to extract pitch

    private List<GameObject> generators = new List<GameObject>();
    private int[] notes = { 0, 2, 4, 5, 7, 9, 11}; // Notes without # notes
    /*
    // Music Parameter
    [DllImport("AudioPluginDemo")]
    private static extern float PitchDetectorGetFreq(int index);

    [DllImport("AudioPluginDemo")]
    private static extern int PitchDetectorDebug(float[] data);
    */
    private int octave = 0; //TODO: remove, probably temp
    private int note = 0; //TODO: remove, probably temp
    private bool noteDetected = false;

    private float fSample;

    private int currentNote = 0;
    private float startTime = 0;

    private void Awake()
    {
        Debug.Log("Awake");
        // TODO : remove rand generation by card detection.
        for(int i = 0; i < 7; ++i)  // 7 note nomber
        {
            float x = UnityEngine.Random.Range(-80, 80); // Caution to conflict between System and UnityEngine Random 
            float y = UnityEngine.Random.Range(-30, 30);
            float z = UnityEngine.Random.Range(50, 100);
            this.CreateGenerator(new Vector3(x, y, z), new Vector3(-90, 0, 0)); //TODO : change rotation for card inclinaison
        }
    }
    
	// Use this for initialization
	void Start () {
        fSample = AudioSettings.outputSampleRate;
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if(noteDetected == true)
        {
            CreateEnnemi();
        }
        else
        {
            GetNote();
        }
    }

    private void GetNote()
    {
        float dTime = (Time.time - startTime);
        Debug.Log("DeltaTime = " + dTime);
        if(dTime > deltaTime)
        {
            //float currentFrequency = PitchDetectorGetFreq(0);
            float currentFrequency = GetFrenquency();
            if (currentFrequency > 0.0f)
            {
                float noteVal = 57.0f + 12.0f * Mathf.Log10(currentFrequency / 440.0f) / Mathf.Log10(2.0f);
                float f = Mathf.Floor(noteVal + 0.5f);
                note = (int)f % 12;
                octave = (int)Mathf.Floor((noteVal + 0.5f) / 12.0f);
                if (octave >= minOctave && Array.IndexOf(notes, note) != -1 && note != currentNote)
                {
                    noteDetected = true;
                    currentNote = note;
                    startTime = Time.time;
                }
                else
                    noteDetected = false;
            }
            else
                noteDetected = false;
        }
    }

    private float GetDb()
    {
        float[] samples = new float[qSamples];
        float rmsValue;
        float dbValue;
        GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples

        int i;
        float sum = 0;
        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
                                            // get sound spectrum
        return dbValue;
    }

    private float GetFrenquency()
    {
        int i;
        float[] spectrum = new float[qSamples];
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        int maxN = 0;
        for (i = 0; i < qSamples; i++)
        { // find max 
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < qSamples - 1)
        { // interpolate index using neighbours
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        return freqN * (fSample / 2) / qSamples; // convert index to frequency
    }

    private void CreateEnnemi()
    {
        int indexGenerator = UnityEngine.Random.Range(0, generators.Count);
        //int indexGenerator = currentNote % generators.Count;
        Generator generatorObject = generators[indexGenerator].GetComponent<Generator>();
        generatorObject.CreateEnnemi(currentNote);
        noteDetected = false;
    }

    private void CreateGenerator(Vector3 position, Vector3 rotation)
    {
//        Debug.Log("CreateGenerator");
        Quaternion quaterRotation = Quaternion.identity;
        quaterRotation.eulerAngles = rotation;
        var newGenerator = Instantiate(generator, position, quaterRotation);
        generators.Add(newGenerator);
    }
}
