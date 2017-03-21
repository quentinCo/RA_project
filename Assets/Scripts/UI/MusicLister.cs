using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MusicLister : MonoBehaviour {

    public string musicDirectoryName;
    public GameObject buttonType;
    public GameManager gameManager;

    public float posXbutton = 7.5f;
    public float posYbutton = -28f;

    private string appSoundDirectory;
    private List<string> musicList = new List<string>();

	// Use this for initialization
	void Start ()
    {
    //#if UNITY_ANDROID
        appSoundDirectory = /*"jar:file://" +*/ Application.dataPath + "!/assets/" + musicDirectoryName +"/";
    /*#else
        appSoundDirectory = Application.streamingAssetsPath + "/" + musicDirectoryName + "/";
    #endif*/
        Debug.Log("appSoundDirectory = " + appSoundDirectory);
        GetSoundList();
        GenerateButtons();
    }

    // Recupere la liste des musique dans le dossier
    private void GetSoundList()
    {
        DirectoryInfo info = new DirectoryInfo(appSoundDirectory);
        FileInfo[] fileInfo = info.GetFiles();

        //string extension = (SystemInfo.deviceType == DeviceType.Desktop) ? ".wav" : ".mp3"; // WWW can read mp3, but only on phone.

        foreach (FileInfo file in fileInfo)
        {
            //if (file.Extension == extension)
                musicList.Add(file.Name);
            Debug.Log("file.Name" + file.Name);
        }
    }

    // Genere les bouttons pour la selection
    private void GenerateButtons()
    {
        var musicButtons = GameObject.Find("MusicButtonsContent");
        
        for (int i = 0; i < musicList.Count; ++i)
        {
            // Crea boutton
            GameObject newObj = Instantiate(buttonType);
            newObj.transform.SetParent(musicButtons.gameObject.transform);
            newObj.transform.localPosition = new Vector3(posXbutton, posYbutton * i, 0);

            // Redimensionnement du conteneur
            RectTransform obj = (RectTransform)newObj.transform;
            RectTransform content = (RectTransform)musicButtons.transform;
            if (Mathf.Abs(newObj.transform.localPosition.y) > content.rect.height)
               musicButtons.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Abs(content.rect.height - newObj.transform.localPosition.y));

            // Initialisation des parametrer du boutton
            SelectMusicButton buttonComponent = newObj.GetComponent<SelectMusicButton>();
            buttonComponent.SetManager(gameManager);
            buttonComponent.SetMusicPath(appSoundDirectory + musicList[i]);
            buttonComponent.SetMusicName(musicList[i].Substring(0, musicList[i].Length - 4)); // 4 size of ".mp3" | ".wav"
        }
    }
}
