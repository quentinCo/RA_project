using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    //public List<GameObject> objectToGenerate; // TODO
    public GameObject objectToGenerate;

    static private Color[] ennemiColor = {
        new Color(1, 0, 0), new Color(1, 0.5f, 0),
        new Color(1, 1, 0), new Color(0, 1, 0),
        new Color(0, 1, 1), new Color(0, 0, 1),
        new Color(1, 0, 1)
    };

    void Awake()
    {
        //CreateEnnemi();//TODO : remove after test
    }

    // Use this for initialization
    void Start () {
        //CreateEnnemi();//TODO : remove after test
    }
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * 300, Space.Self);
	}

    public void CreateEnnemi(int note)
    {
        GameObject newObj = Instantiate(objectToGenerate, transform.position, Quaternion.identity);
        var newEnnemi = newObj.GetComponent<Ennemi>();
        newEnnemi.SetTarget(Camera.main);
        newEnnemi.SetLife(1);

        SetEnnemiColor(newObj, note);
    }

    private void SetEnnemiColor(GameObject ennemi, int note)
    {
        Color color = ennemiColor[note % 7]; // 7 {do, re, mi, fa, sol, la, si}
        ennemi.GetComponent<Renderer>().material.color = color;
    }
}
