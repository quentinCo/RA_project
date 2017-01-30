using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    //public List<GameObject> objectToGenerate; // TODO
    public GameObject objectToGenerate;

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
		
	}

    public void CreateEnnemi()
    {
        GameObject newObj = Instantiate(objectToGenerate, transform.position, Quaternion.identity);
        var newEnnemi = newObj.GetComponent<Ennemi>();
        newEnnemi.SetTarget(Camera.main);
        newEnnemi.SetLife(1);
    }
}
