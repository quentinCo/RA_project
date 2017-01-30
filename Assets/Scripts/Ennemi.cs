using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : MonoBehaviour {

    public float speed = 2;
    private int life = 1;
    private Behaviour target;
    private Vector3 targetOldPosition;

	// Use this for initialization
	void Start () {
        SetTarget(Camera.main); // TODO :remove it after test
    }
	
	// Update is called once per frame
	void Update () {
        if (targetOldPosition != target.transform.position)
            UpdateDirection();
    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        --life;
        if(life == 0)
            Destroy(gameObject);

        //TODO : add score count
    }

    public void SetLife(int life)
    {
        this.life = life;
    }

    public void SetTarget(Behaviour target)
    {
//        Debug.Log("SetTarget()");
        this.target = target;
        targetOldPosition = target.transform.position;
        UpdateDirection();
    }

    private void UpdateDirection()
    {
//        Debug.Log("UpdateDirection()");
        // Compute new target
        Vector3 toTarget = target.transform.position - transform.position;

        // Update the visual direction (rotation)
        Vector3 newDir = Vector3.RotateTowards(transform.forward, toTarget, 2 * Mathf.PI, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        Quaternion rotation = Quaternion.LookRotation(newDir);
        transform.rotation = rotation;

        // Update the moving direction (velocity)
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    
}
