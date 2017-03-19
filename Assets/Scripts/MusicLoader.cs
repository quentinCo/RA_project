using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using System.IO;

public class MusicLoader : MonoBehaviour {
    public GameManager gameManager;

    private AudioClip loadingMusic;
    private WWW musicLoader;

    void Update()
    {
        if (loadingMusic)
            CheckLoading();
    }

    public void LoadMusic(string path)
    {
        musicLoader = new WWW("file:///" + path);
        loadingMusic = musicLoader.GetAudioClip(true);
        //this.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    // check the loading state
    private void CheckLoading()
    {
        switch (loadingMusic.loadState)
        {
            case AudioDataLoadState.Loaded:
                gameManager.GetComponent<AudioSource>().clip = loadingMusic;
                gameManager.StartGame();
                loadingMusic = null;
                break;
            case AudioDataLoadState.Loading:
                var rectTransform = gameObject.GetComponentInChildren<Image>().transform as RectTransform;
                rectTransform.localScale = new Vector3(musicLoader.progress, 1, 0);
                break;
         }
    }
}
