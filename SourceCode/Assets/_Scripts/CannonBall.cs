using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

    private float speed = 100f;
    //private float speed = 0.1f;
    private float timeToLive = 3;
    private float timeToDie;

    public Ship shipFired;

	// Use this for initialization
	void Start () {
        timeToDie = Time.time + 3;		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(timeToDie < Time.time)
        {
            Destroy(this.gameObject);
            return;
        }

        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Rock")
        {
            GameObject explodeCube = GameObject.Instantiate(GameObject.Find("GameController").GetComponent<GameController>().playerShip.explodeCubePrefab);
            explodeCube.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
