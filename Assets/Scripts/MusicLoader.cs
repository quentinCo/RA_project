using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class MusicLoader : MonoBehaviour {
    public GameManager gameManager;

    private AudioClip loadingMusic;
    private WWW musicLoader;
    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if (loadingMusic)
        {
            CheckLoading();
        }
    }

    public void LoadMusic(string path)
    {
        musicLoader = new WWW("file:///" + path);
        loadingMusic = musicLoader.GetAudioClip(true);
        this.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void CheckLoading()
    {
        switch (loadingMusic.loadState)
        {
            case AudioDataLoadState.Loaded:
                gameManager.GetComponent<AudioSource>().clip = loadingMusic;
                gameManager.StartGame();
                loadingMusic = null;
                break;
            case AudioDataLoadState.Failed:
                if (!EditorUtility.DisplayDialog("Error", "There is a problem with the music that you have chosen :( .\nPlease Select another music.", "Chose a music", "Quit"))
                {
                #if UNITY_STANDALONE
                            //Quit the application
                            Application.Quit();
                #endif

                            //If we are running in the editor
                #if UNITY_EDITOR
                            //Stop playing the scene
                            UnityEditor.EditorApplication.isPlaying = false;
                #endif
                }
                else
                    ChoseMusic();

                break;
            case AudioDataLoadState.Loading:
                var rectTransform = gameObject.GetComponentInChildren<Image>().transform as RectTransform;
                rectTransform.localScale = new Vector3(musicLoader.progress, 1, 0);
                break;*/
         }
    }

    private void ChoseMusic()
    {
        string format = (SystemInfo.deviceType == DeviceType.Desktop) ? "wav" : "mp3"; // WWW can read mp3, but only on phone.
        string path = EditorUtility.OpenFilePanel("PLEASE CHOSE YOUR MUSIC ^^ ", "", format);
        if (path.Length != 0)
            this.LoadMusic(path);
    }
}
