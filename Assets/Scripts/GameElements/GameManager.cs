using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour {

    public GameObject generator;
    public int minOctave = 1;
    public float deltaTime = 0.25f;

    public int qSamples = 1024;  // array size
    public float refValue = 0.1f; // RMS value for 0 dB
    public float threshold = 0.02f;      // minimum amplitude to extract pitch

    private List<GameObject> generators = new List<GameObject>();
    //private int[] notes = { 0, 2, 4, 5, 7, 9, 11}; // Notes without # notes
    private int[] notes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }; // Notes with # notes

    private int octave = 0; //TODO: remove, probably temp
    private int note = 0; //TODO: remove, probably temp
    private bool noteDetected = false;

    private float fSample;

    private int currentNote = 0;
    private float startTime = 0;

    private bool isInit = false;
    private bool pause = false;

    private int score = 0;
    private int hightScore = 0;

    private void Awake()
    {
        // TODO : remove rand generation by card detection.
//        StartGame();
    }
    
	// Use this for initialization
	void Start () {
        fSample = AudioSettings.outputSampleRate;
        GameObject[] generatorObjects = GameObject.FindGameObjectsWithTag("Generator");
        foreach (GameObject generator in generatorObjects) {
            generators.Add(generator);
        }

        Debug.Log(generators.Count);
    }
	
	// Update is called once per frame
	void Update () {
        if(noteDetected == true)
            CreateEnnemi();
        else
            GetNote();
    }
    /* State Management */
    public void StartGame()
    {
        pause = false;
        if(isInit)
        {
            ResetEnnemies();
            /*for (int i = 0; i < 7; ++i)  // 7 note nomber
            {
                // TODO : change generator initiation with RA
                float x = UnityEngine.Random.Range(-80, 80); // Caution to conflict between System and UnityEngine Random 
                float y = UnityEngine.Random.Range(-30, 30);
                float z = UnityEngine.Random.Range(50, 100);
                this.CreateGenerator(new Vector3(x, y, z), new Vector3(-90, 0, 0)); //TODO : change rotation for card inclinaison
            }
            isInit = true;*/
        }
        else{
            isInit = true;
        }
            
        startTime = Time.time;

        InitGameUI();
        PlayAtBegin();

        Time.timeScale = 1;
    }

    public void Pause(bool pause)
    {
        if (this.pause == pause)
            return;

        this.pause = pause;

        if(!pause)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;

        PauseMusic(pause);
    }

    /* UI Management */
    private void InitGameUI()
    {
        var gameUI = UnactiveUiElements("GameUI");
        gameUI.GetComponentInChildren<PauseButton>().Init();

        SetScores();
    }

    private GameObject FindGameObject(Component[] parentObjectChilds, string name)
    {
        GameObject objectFinding = null;

        foreach(Component gm in parentObjectChilds)
        {
            if (gm.name == name)
                return gm.gameObject;
        }

        return objectFinding;
    }

    private GameObject UnactiveUiElements(string nameExecption = null)
    {
        GameObject mainUIObject = GameObject.Find("UI");
        Component[] uiChilds = mainUIObject.GetComponentsInChildren(typeof(Transform), true);
        foreach (var uiChild in uiChilds)
        {
            if (uiChild.transform.parent == mainUIObject.transform)
                uiChild.gameObject.SetActive(false);
        }

        if(nameExecption != null)
        {
            var toActivate = FindGameObject(uiChilds, nameExecption);
            Debug.Assert(toActivate);
            toActivate.gameObject.SetActive(true);
            return toActivate;
        }
        return null;
    }
    
    public void  DisplayMusicList()
    {
        GameObject mainUIObject = GameObject.Find("UI");
        Component[] uiChilds = mainUIObject.GetComponentsInChildren(typeof(Transform), true);
        
        var musicList = FindGameObject(uiChilds, "MusicList");
        Debug.Assert(musicList);
        musicList.gameObject.SetActive(true);
    }
    
    /* Music Control */
    public void LoadMusic(string pathMusic)
    {
        GameObject musicLoading = UnactiveUiElements("MusicLoading");
        musicLoading.GetComponentInChildren<MusicLoader>().LoadMusic(pathMusic);
    }

    public void PlayAtBegin()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.Play();
    }

    private void PlayMusic()
    {
        GetComponent<AudioSource>().Play();
    }

    private void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }

    private void PauseMusic(bool pause)
    {
        if (pause)
            GetComponent<AudioSource>().Pause();
        else
            GetComponent<AudioSource>().UnPause();
    }

    /* Audio Analysis */
    private void GetNote()
    {
        float dTime = (Time.time - startTime);
        if(dTime > deltaTime)
        {
            //float currentFrequency = PitchDetectorGetFreq(0);
            float currentFrequency = GetFrenquency();
            if (currentFrequency > 0.0f)
            {
                float noteVal = Mathf.Abs(12.0f * Mathf.Log10(currentFrequency / 440.0f) / Mathf.Log10(2.0f));
                Debug.Log("note = " + noteVal);
                float f = Mathf.Floor(noteVal + 0.5f);
                note = (int)f % 12;
                octave = (int)Mathf.Floor((noteVal + 0.5f) / 12.0f);
                Debug.Log("octave = " + octave);
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

    /* Game Object Management */
    private void CreateEnnemi()
    {
        int indexGenerator = UnityEngine.Random.Range(0, generators.Count);
        //int indexGenerator = currentNote % generators.Count;
        Generator generatorObject = generators[indexGenerator].GetComponent<Generator>();
        generatorObject.CreateEnnemi(currentNote);
        noteDetected = false;
    }

    public GameObject CreateGenerator(Vector3 position, Vector3 rotation)
    {
        Quaternion quaterRotation = Quaternion.identity;
        quaterRotation.eulerAngles = rotation;
        var newGenerator = Instantiate(generator, position, quaterRotation);
        generators.Add(newGenerator);
        return newGenerator;
    }

    public void RemoveGenerator(GameObject generator)
    {
        generators.Remove(generator);
        Destroy(generator);
        Debug.Log(generators.Count);
    }

    private void ResetEnnemies()
    {
        Ennemi[] ennemies = GameObject.FindObjectsOfType<Ennemi>();
        foreach(var ennemi in ennemies)
            Destroy(ennemi.gameObject);
    }

    /* Score Management */
    private void SetScores()
    {
        if (score > hightScore)
            hightScore = score;

        score = 0;

        GameObject hightScoreObject = GameObject.Find("HightScore");
        Debug.Assert(hightScoreObject, "hightScoreObject is null");
        Text hightScoreText = hightScoreObject.GetComponentInChildren<Text>();
        hightScoreText.text = hightScore.ToString();

        GameObject scoreObject = GameObject.Find("Score");
        Debug.Assert(scoreObject, "scoreObject is null");
        Text scoreText = scoreObject.GetComponentInChildren<Text>();
        scoreText.text = score.ToString();
    }

    public void IncrementScore(int increment)
    {
        score += increment;

        GameObject scoreObject = GameObject.Find("Score");
        Debug.Assert(scoreObject, "scoreObject is null");
        Text scoreText = scoreObject.GetComponentInChildren<Text>();
        scoreText.text = score.ToString();
    }
}
