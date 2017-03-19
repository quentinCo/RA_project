using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MusicLister : MonoBehaviour {

    public string musicDirectoryName;
    public GameObject buttonType;

    private string appSoundDirectory;
    private List<string> musicList = new List<string>();

    private float posXbutton = 7.5f;
    private float posYbutton = -28f;

	// Use this for initialization
	void Start () {
        appSoundDirectory = Application.dataPath + "/" + musicDirectoryName;
        GetSoundList();
        GenerateButtons();
    }

    // Recupere la liste des musique dans le dossier
    private void GetSoundList()
    {
        var info = new DirectoryInfo(appSoundDirectory);
        var fileInfo = info.GetFiles();

        string extension = (SystemInfo.deviceType == DeviceType.Desktop) ? ".wav" : ".mp3"; // WWW can read mp3, but only on phone.

        foreach (var file in fileInfo)
        {
            if (file.Extension == extension)
                musicList.Add(file.Name);
        }
    }

    // Genere les bouttons pour la selection
    private void GenerateButtons()
    {
        MusicLoader musicLoader = null;// = GetComponentInChildren<MusicLoader>();
        var trs = GetComponentsInChildren(typeof(Transform), true);
        foreach (Transform t in trs)
        {
            if (t.GetComponent<MusicLoader>())
            {
                musicLoader = t.GetComponent<MusicLoader>();
                break;
            }
        }
        var musicButtons = GameObject.Find("MusicButtonsContent");
        /*for (int i = 0; i < 5; i++)
        {
            GameObject goButton = (GameObject)Instantiate(buttonType);
            //goButton.transform.SetParent(ParentPanel, false);
            goButton.transform.localScale = new Vector3(1, 1, 1);

            Button tempButton = goButton.GetComponent<Button>();
            int tempInt = i;
        }*/
        for (int i = 0; i < musicList.Count; ++i)
        {
            GameObject newObj = Instantiate(buttonType);
            newObj.transform.SetParent(musicButtons.gameObject.transform);
            newObj.transform.localPosition = new Vector3(posXbutton, posYbutton * i, 0);
            RectTransform obj = (RectTransform)newObj.transform;
            RectTransform content = (RectTransform)musicButtons.transform;

            if (Mathf.Abs(newObj.transform.localPosition.y) > content.rect.height)
               musicButtons.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Abs(content.rect.height - newObj.transform.localPosition.y));

            SelectMusicButton buttonComponent = newObj.GetComponent<SelectMusicButton>();
            buttonComponent.SetMusicLoader(musicLoader);
            buttonComponent.SetMusicPath(appSoundDirectory + "/" + musicList[i % 2]);
            buttonComponent.SetMusicName(musicList[i % 2].Substring(0, musicList[i % 2].Length - 4)); // 4 size of ".mp3" | ".wav"
        }
    }
}
