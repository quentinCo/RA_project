using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {

    void Start()
    {
        Button btnComponent = gameObject.GetComponent<Button>();
        btnComponent.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
        //#endif
        //#if UNITY_STANDALONE
            Application.Quit();
        #endif
    }
}
