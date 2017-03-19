using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using System.IO;

public class StartButton : MonoBehaviour {

    public GameManager gameManager;

    void Start()
    {
        Button btnComponent = gameObject.GetComponent<Button>();
        btnComponent.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        gameManager.DisplayMusicList();
    }
    /*
    public void Init()
    {
        gameObject.transform.parent.gameObject.SetActive(true);
    }
    */
}
