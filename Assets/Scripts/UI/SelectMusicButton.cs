using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMusicButton : MonoBehaviour {

    private string musicName = "";
    private string musicPath = "";
    public MusicLoader musicLoader;

    void Start()
    {
        Button btnComponent = gameObject.GetComponent<Button>();
        btnComponent.GetComponentInChildren<Text>().text = musicName;
        btnComponent.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        Debug.Assert(musicLoader);
        musicLoader.LoadMusic(musicPath);
    }

    public void SetMusicName(string name)
    {
        musicName = name;
        gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text = musicName;
    }

    public void SetMusicPath(string path)
    {
        musicPath = path;
    }

    public void SetMusicLoader(MusicLoader gm)
    {
        musicLoader = gm;
    }

}
