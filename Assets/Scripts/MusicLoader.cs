using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                // TODO : error message
                this.gameObject.SetActive(true);
                break;
            case AudioDataLoadState.Loading:
                // TODO : loading bar
                var rectTransform = gameObject.GetComponentInChildren<Image>().transform as RectTransform;
                rectTransform.localScale = new Vector3(musicLoader.progress, 1, 0);
                Debug.Log("Progress = " + musicLoader.progress);
                break;
        }
    }
}
