using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponentController : MonoBehaviour {

    //public GameObject cannonBallExplode;
    //public GameObject shipCollisionExplode;

    private Ship ship;
    private GameController gameController;

	// Use this for initialization
	void Start ()
    {
        ship = this.GetComponentInParent<Ship>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void explode(GameObject otherObj, bool explodeSound=false)
    {
        ship.hit(this.gameObject, explodeSound);
        
        if(otherObj.tag == "CannonBall")
        {
            GameObject.Destroy(otherObj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger: " + this.name + "->" + other.gameObject.name);
        //Debug.Log("Trigger Enter!");
        if(other.tag == "CannonBall" && other.GetComponent<CannonBall>().shipFired != ship && ship.alive)
        {
            if(other.GetComponent<CannonBall>().shipFired.isPlayer)
            {
                GameController.levelGold += 5;
            }
            explode(other.gameObject, true);
        }
        else if (other.tag == "ShipComponent" && other.GetComponent<Ship>() != ship && ship.alive)
        {
            explode(other.gameObject, ship.isPlayer || Vector3.Distance(ship.transform.position, gameController.playerShip.transform.position) < 50);
        }
        else if (other.tag == "Rock")
        {
            explode(other.gameObject, ship.isPlayer || Vector3.Distance(ship.transform.position, gameController.playerShip.transform.position) < 50);
        }
        else if (other.tag == "Objective" && ship.isPlayer && ship.alive)
        {
            ship.win();
        }

    }
}
