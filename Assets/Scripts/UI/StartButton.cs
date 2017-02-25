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
    public GameObject loadingBar;

    //public Button startButton;

    void Start()
    {
        Button btnComponent = gameObject.GetComponent<Button>();
        btnComponent.onClick.AddListener(TaskOnClick);
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
            loadingBar.SetActive(true);
            loadingBar.GetComponent<MusicLoader>().LoadMusic(path);
        }
    }
    
    
}
