using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : MonoBehaviour {

    public float speed = 2;
    private int life = 1;
    private Behaviour target;
    private Vector3 targetOldPosition;
    
	void Start () {
        SetTarget(Camera.main); // TODO :remove it after test and RA
        transform.Rotate(Vector3.left * 90, Space.Self);
    }
	
	void Update () {
        if (targetOldPosition != target.transform.position)
            UpdateDirection();
    }

    void OnMouseDown()
    {

        /*RaycastHit lhit = new RaycastHit();
        Ray lray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(lray, out lhit))
        {
            Debug.Log("Hit something: " + lhit.collider.gameObject.name);
            if (lhit.collider.gameObject == this.gameObject)
            {
                Debug.Log("Hit the health up button");
                --life;
                if (life == 0)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().IncrementScore(1);
                    Destroy(gameObject);
                }
            }

        }*/

        
        --life;
        if (life == 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().IncrementScore(1);
            Destroy(gameObject);
        }
    }

    public void SetLife(int life)
    {
        this.life = life;
    }

    public void SetTarget(Behaviour target)
    {
        this.target = target;
        targetOldPosition = target.transform.position;
        UpdateDirection();
    }

    private void UpdateDirection()
    {
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
