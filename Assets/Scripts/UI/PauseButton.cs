using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {

    public GameManager gameManager;
    public GameObject pauseScreen;

    public Sprite pauseSprite;
    public Sprite lectureSprite;

    private bool pause = false;

	// Use this for initialization
	void Start () {
        //Init();
        Button btnComponent = gameObject.GetComponent<Button>();
        btnComponent.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        pause = !pause;
        gameManager.Pause(pause);
        if(pause)
            gameObject.GetComponent<Image>().sprite = lectureSprite;
        else
            gameObject.GetComponent<Image>().sprite = pauseSprite;

        pauseScreen.SetActive(pause);
    }

    public void Init()
    {
        Image image = gameObject.GetComponent<Image>();
        image.sprite = pauseSprite;

        pause = false;

        pauseScreen.SetActive(false);
    }
}
