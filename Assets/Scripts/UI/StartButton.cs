using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class StartButton : MonoBehaviour {

    public GameManager gameManager;
    public Canvas gameUI;
    public Canvas mainMenu;

    private AudioClip loadingMusic;
    //public Button startButton;

    void Start()
    {
        Button btnComponent = gameObject.GetComponent<Button>();
        btnComponent.onClick.AddListener(TaskOnClick);
    }
    
    void Update()
    {
        if(loadingMusic)
        {
            CheckLoading();
        }
    }

    public void TaskOnClick()
    {
        ChoseMusic();
    }

    private void ChoseMusic()
    {
        string format = (SystemInfo.deviceType == DeviceType.Desktop) ? "wav" : "mp3"; // WWW can read mp3, but only on phone.
        string path = EditorUtility.OpenFilePanel("PLEASE CHOSE YOUR MUSIC ^^ ", "", format);
        if (path.Length != 0)
        {
            WWW musicLoader = new WWW("file:///" + path);
            loadingMusic = musicLoader.GetAudioClip(true);
        }
    }

    private void CheckLoading()
    {
        switch (loadingMusic.loadState)
        {
            case AudioDataLoadState.Loaded:
                gameManager.GetComponent<AudioSource>().clip = loadingMusic;
                mainMenu.gameObject.SetActive(false);
                gameManager.StartGame();
                gameUI.gameObject.SetActive(false);
                loadingMusic = null;
                break;
            case AudioDataLoadState.Failed:
                // TODO : error message
                break;
            case AudioDataLoadState.Loading:
                // TODO : loading bar
                break;
        }
    }
}
